using System.Collections.Concurrent;
using System.Threading.Channels;

namespace UniverBlazored.Spreadsheets.Services;

public sealed class SpreadsheetCommandScheduler : ISpreadsheetCommandScheduler
{
    private readonly ConcurrentDictionary<(string InstanceId, string SheetId), SheetWorker> sheetWorkers = new();
    private readonly AsyncReaderWriterGate workbookGate = new();
    private int disposed;
    private int operationCounter;

    public Task EnqueueSheet(SpreadsheetOperationContext context, Func<Task> operation)
        => EnqueueSheet(context, async () => { await operation(); return true; });

    public Task<T> EnqueueSheet<T>(SpreadsheetOperationContext context, Func<Task<T>> operation)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
        ArgumentNullException.ThrowIfNull(operation);
        if (context.SheetId is null)
            return Task.FromException<T>(new InvalidOperationException("SheetId is required for EnqueueSheet."));

        context.CancellationToken.ThrowIfCancellationRequested();
        var key = (context.InstanceId, context.SheetId);
        var worker = sheetWorkers.GetOrAdd(key, _ => new SheetWorker(workbookGate, context.CancellationToken));
        var opId = Interlocked.Increment(ref operationCounter);
        return worker.Enqueue(opId, operation, context.CancellationToken);
    }

    public Task ExecuteSheetAsync(SpreadsheetOperationContext context, Func<Task> operation)
        => ExecuteSheetAsync(context, async () => { await operation(); return true; });

    public async Task<T> ExecuteSheetAsync<T>(SpreadsheetOperationContext context, Func<Task<T>> operation)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
        ArgumentNullException.ThrowIfNull(operation);
        if (context.SheetId is null)
            return await Task.FromException<T>(new InvalidOperationException("SheetId is required for ExecuteSheetAsync."));

        context.CancellationToken.ThrowIfCancellationRequested();
        var key = (context.InstanceId, context.SheetId);
        var worker = sheetWorkers.GetOrAdd(key, _ => new SheetWorker(workbookGate, context.CancellationToken));
        var opId = Interlocked.Increment(ref operationCounter);
        return await worker.ExecuteAtomic(opId, operation, context.CancellationToken);
    }

    public Task EnqueueGlobal(SpreadsheetOperationContext context, Func<Task> operation)
        => EnqueueGlobal(context, async () => { await operation(); return true; });

    public async Task<T> EnqueueGlobal<T>(SpreadsheetOperationContext context, Func<Task<T>> operation)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed) != 0, this);
        ArgumentNullException.ThrowIfNull(operation);
        await using var lease = await workbookGate.EnterWriteAsync(context.CancellationToken);
        return await operation();
    }

    public int PendingSheetCount(string instanceId, string sheetId)
    {
        var key = (instanceId, sheetId);
        return sheetWorkers.TryGetValue(key, out var worker) ? worker.PendingCount : 0;
    }

    public bool IsDisposed => Volatile.Read(ref disposed) != 0;

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref disposed, 1) != 0)
            return;

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var disposeTasks = sheetWorkers.Values.Select(worker => worker.DisposeAsync().AsTask()).ToArray();
        if (disposeTasks.Length > 0)
        {
            var timeoutTask = Task.Delay(Timeout.Infinite, cts.Token);
            var completed = await Task.WhenAny(Task.WhenAll(disposeTasks), timeoutTask);
            if (completed == timeoutTask)
            {
                foreach (var worker in sheetWorkers.Values)
                    worker.TryComplete();
            }
            else
            {
                await Task.WhenAll(disposeTasks);
            }
        }
        workbookGate.Dispose();
    }

    private sealed class SheetWorker : IAsyncDisposable
    {
        private readonly Channel<WorkItem> channel = Channel.CreateBounded<WorkItem>(
            new BoundedChannelOptions(1000)
            {
                SingleReader = true,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait
            });
        private readonly Task worker;
        private readonly CancellationTokenSource workerCts;
        private int pendingCount;

        public int PendingCount => Volatile.Read(ref pendingCount);

        public SheetWorker(AsyncReaderWriterGate workbookGate, CancellationToken parentToken)
        {
            workerCts = CancellationTokenSource.CreateLinkedTokenSource(parentToken);
            worker = Task.Run(async () =>
            {
                try
                {
                    await foreach (var item in channel.Reader.ReadAllAsync(workerCts.Token))
                    {
                        Interlocked.Decrement(ref pendingCount);
                        await using var lease = await workbookGate.EnterReadAsync(workerCts.Token);
                        if (item.IsAtomic)
                        {
                            await item.Operation(workerCts.Token);
                        }
                        else
                        {
                            await item.Operation(workerCts.Token);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }

        public Task<T> Enqueue<T>(long operationId, Func<Task<T>> operation, CancellationToken ct)
        {
            Interlocked.Increment(ref pendingCount);
            var completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(workerCts.Token, ct);
            var item = new WorkItem(
                false,
                async _ =>
                {
                    using var linked = linkedCts;
                    try
                    {
                        linked.Token.ThrowIfCancellationRequested();
                        completion.TrySetResult(await operation());
                    }
                    catch (OperationCanceledException)
                    {
                        completion.TrySetCanceled();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        completion.TrySetException(ex);
                    }
                });

            if (!channel.Writer.TryWrite(item))
            {
                Interlocked.Decrement(ref pendingCount);
                completion.TrySetException(new ObjectDisposedException(nameof(SpreadsheetCommandScheduler)));
            }
            return completion.Task;
        }

        public async Task<T> ExecuteAtomic<T>(long operationId, Func<Task<T>> operation, CancellationToken ct)
        {
            Interlocked.Increment(ref pendingCount);
            var completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(workerCts.Token, ct);
            var item = new WorkItem(
                true,
                async _ =>
                {
                    using var linked = linkedCts;
                    try
                    {
                        linked.Token.ThrowIfCancellationRequested();
                        completion.TrySetResult(await operation());
                    }
                    catch (OperationCanceledException)
                    {
                        completion.TrySetCanceled();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        completion.TrySetException(ex);
                    }
                });

            if (!channel.Writer.TryWrite(item))
            {
                Interlocked.Decrement(ref pendingCount);
                completion.TrySetException(new ObjectDisposedException(nameof(SpreadsheetCommandScheduler)));
            }
            return await completion.Task;
        }

        public void TryComplete()
        {
            try { channel.Writer.TryComplete(); }
            catch { }
        }

        public async ValueTask DisposeAsync()
        {
            channel.Writer.TryComplete();
            try
            {
                await worker;
            }
            catch (OperationCanceledException)
            {
            }
            workerCts.Dispose();
        }

        private readonly record struct WorkItem(bool IsAtomic, Func<CancellationToken, Task> Operation);
    }

    private sealed class AsyncReaderWriterGate : IDisposable
    {
        private readonly SemaphoreSlim turnstile = new(1, 1);
        private readonly SemaphoreSlim roomEmpty = new(1, 1);
        private readonly SemaphoreSlim readerMutex = new(1, 1);
        private int readers;

        public async ValueTask<IAsyncDisposable> EnterReadAsync(CancellationToken ct = default)
        {
            await turnstile.WaitAsync(ct);
            await readerMutex.WaitAsync(ct);
            try
            {
                if (++readers == 1)
                    await roomEmpty.WaitAsync(ct);
            }
            finally
            {
                readerMutex.Release();
                turnstile.Release();
            }
            return new Lease(ExitRead);
        }

        public async ValueTask<IAsyncDisposable> EnterWriteAsync(CancellationToken ct = default)
        {
            await turnstile.WaitAsync(ct);
            await roomEmpty.WaitAsync(ct);
            return new Lease(() =>
            {
                roomEmpty.Release();
                turnstile.Release();
                return ValueTask.CompletedTask;
            });
        }

        private async ValueTask ExitRead()
        {
            await readerMutex.WaitAsync();
            try
            {
                if (--readers == 0)
                    roomEmpty.Release();
            }
            finally { readerMutex.Release(); }
        }

        public void Dispose()
        {
            turnstile.Dispose();
            roomEmpty.Dispose();
            readerMutex.Dispose();
        }

        private sealed class Lease(Func<ValueTask> release) : IAsyncDisposable
        {
            private Func<ValueTask>? release = release;
            public ValueTask DisposeAsync() => Interlocked.Exchange(ref release, null)?.Invoke() ?? ValueTask.CompletedTask;
        }
    }
}
