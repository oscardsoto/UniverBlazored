using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;

namespace UniverBlazored.Tests;

public sealed class FakeUniverJsInterop : IUniverJsInterop
{
    public List<RecordedAction> Actions { get; } = new();
    public TimeSpan SimulatedDelay { get; set; } = TimeSpan.Zero;
    public int InitializationCount { get; private set; }

    public Lazy<Task<IJSObjectReference>> moduleTask =>
        new(Task.FromResult<IJSObjectReference>(null!));

    public Queue<UniverQueueValue> actionQueue { get; } = new();

    public IUniverJsInterop SetAction(string action, params object[] args)
    {
        actionQueue.Enqueue(new UniverQueueValue(action, args));
        return this;
    }

    public Task InitializeAsync(string newIdDiv)
    {
        InitializationCount++;
        return Task.CompletedTask;
    }

    public Task InitializeAsync(string instanceId, string newIdDiv)
    {
        InitializationCount++;
        return Task.CompletedTask;
    }

    public Task InitializeAsync(string instanceId, string newIdDiv, UniverInit initConfig)
    {
        InitializationCount++;
        return Task.CompletedTask;
    }

    public Task ResolveAsync()
    {
        Actions.Add(new(RecordedActionKind.Resolve, "default", actionQueue.ToArray()));
        actionQueue.Clear();
        return ApplyDelay();
    }

    public Task<T> ResolveAsync<T>()
    {
        Actions.Add(new(RecordedActionKind.ResolveT, "default", actionQueue.ToArray()));
        actionQueue.Clear();
        return ApplyDelay(Task.FromResult(default(T))!);
    }

    public Task ResolveAsync(Queue<UniverQueueValue> queue)
    {
        Actions.Add(new(RecordedActionKind.ResolveQueue, "default", queue.ToArray()));
        return ApplyDelay();
    }

    public Task<T> ResolveAsync<T>(Queue<UniverQueueValue> queue)
    {
        Actions.Add(new(RecordedActionKind.ResolveQueueT, "default", queue.ToArray()));
        return ApplyDelay(Task.FromResult(default(T))!);
    }

    public Task ResolveAsync(SpreadsheetOperationContext context)
    {
        Actions.Add(new(RecordedActionKind.ResolveContext, context, actionQueue.ToArray()));
        actionQueue.Clear();
        return ApplyDelay();
    }

    public Task<T> ResolveAsync<T>(SpreadsheetOperationContext context)
    {
        Actions.Add(new(RecordedActionKind.ResolveContextT, context, actionQueue.ToArray()));
        actionQueue.Clear();
        return ApplyDelay(Task.FromResult(default(T))!);
    }

    public Task ResolveAsync(SpreadsheetOperationContext context, Queue<UniverQueueValue> queue)
    {
        Actions.Add(new(RecordedActionKind.ResolveContextQueue, context, queue.ToArray()));
        return ApplyDelay();
    }

    public Task<T> ResolveAsync<T>(SpreadsheetOperationContext context, Queue<UniverQueueValue> queue)
    {
        Actions.Add(new(RecordedActionKind.ResolveContextQueueT, context, queue.ToArray()));
        return ApplyDelay(Task.FromResult(default(T))!);
    }

    public Task ExecuteAtomicAsync(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task> operation)
    {
        Actions.Add(new(RecordedActionKind.ExecuteAtomic, context, Array.Empty<UniverQueueValue>()));
        return ApplyDelay(() => operation(this));
    }

    public Task<T> ExecuteAtomicAsync<T>(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task<T>> operation)
    {
        Actions.Add(new(RecordedActionKind.ExecuteAtomicT, context, Array.Empty<UniverQueueValue>()));
        return ApplyDelay(() => operation(this));
    }

    public Task<T> ResolveActionAsync<T>(string name, params object[] args)
    {
        Actions.Add(new(RecordedActionKind.ResolveAction, "default", name, args));
        return ApplyDelay(Task.FromResult(default(T))!);
    }

    public Task ResolveActionAsync(string name, params object[] args)
    {
        Actions.Add(new(RecordedActionKind.ResolveAction, "default", name, args));
        return ApplyDelay();
    }

    public void Clear() => Actions.Clear();

    private async Task ApplyDelay()
    {
        if (SimulatedDelay > TimeSpan.Zero)
            await Task.Delay(SimulatedDelay);
    }

    private async Task<T> ApplyDelay<T>(Task<T> result)
    {
        if (SimulatedDelay > TimeSpan.Zero)
            await Task.Delay(SimulatedDelay);
        return await result;
    }

    private async Task ApplyDelay(Func<Task> operation)
    {
        if (SimulatedDelay > TimeSpan.Zero)
            await Task.Delay(SimulatedDelay);
        await operation();
    }

    private async Task<T> ApplyDelay<T>(Func<Task<T>> operation)
    {
        if (SimulatedDelay > TimeSpan.Zero)
            await Task.Delay(SimulatedDelay);
        return await operation();
    }
}

public enum RecordedActionKind
{
    Resolve,
    ResolveT,
    ResolveQueue,
    ResolveQueueT,
    ResolveContext,
    ResolveContextT,
    ResolveContextQueue,
    ResolveContextQueueT,
    ExecuteAtomic,
    ExecuteAtomicT,
    ResolveAction
}

public sealed record RecordedAction(
    RecordedActionKind Kind,
    string InstanceId,
    UniverQueueValue[] Queue)
{
    public SpreadsheetOperationContext? Context { get; }
    public string? ActionName { get; }
    public object[]? Args { get; }

    public RecordedAction(RecordedActionKind kind, SpreadsheetOperationContext context, UniverQueueValue[] queue)
        : this(kind, context.InstanceId, queue)
    {
        Context = context;
    }

    public RecordedAction(RecordedActionKind kind, string instanceId, string name, object[] args)
        : this(kind, instanceId, Array.Empty<UniverQueueValue>())
    {
        ActionName = name;
        Args = args;
    }
}
