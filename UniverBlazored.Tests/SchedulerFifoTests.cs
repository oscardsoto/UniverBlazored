namespace UniverBlazored.Tests;

public sealed class SchedulerFifoTests : IAsyncLifetime
{
    private readonly FakeUniverJsInterop fake = new();
    private readonly SpreadsheetCommandScheduler scheduler = new();

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await scheduler.DisposeAsync();

    [Fact]
    public async Task Operations_on_same_sheet_execute_in_order()
    {
        var ctx1 = MakeCtx("inst1", "sheet-A", 1);
        var ctx2 = MakeCtx("inst1", "sheet-A", 2);

        var results = new List<int>();
        var t1 = scheduler.EnqueueSheet(ctx1, async () => { await Task.Yield(); results.Add(1); });
        var t2 = scheduler.EnqueueSheet(ctx2, async () => { await Task.Yield(); results.Add(2); });

        await Task.WhenAll(t1, t2);

        Assert.Equal([1, 2], results);
    }

    [Fact]
    public async Task Operations_on_different_sheets_can_interleave()
    {
        var ctxA = MakeCtx("inst1", "sheet-A", 1);
        var ctxB = MakeCtx("inst1", "sheet-B", 1);

        var gate = new TaskCompletionSource<bool>();
        var results = new List<int>();

        var tA = scheduler.EnqueueSheet(ctxA, async () =>
        {
            results.Add(1);
            await gate.Task;
            results.Add(3);
        });

        var tB = scheduler.EnqueueSheet(ctxB, async () =>
        {
            results.Add(2);
            gate.TrySetResult(true);
        });

        await Task.WhenAll(tA, tB);

        Assert.Contains(1, results);
        Assert.Contains(2, results);
        Assert.Contains(3, results);
        Assert.True(results.IndexOf(1) < results.IndexOf(3));
    }

    [Fact]
    public async Task Global_barrier_awaits_pending_sheet_work()
    {
        var ctxSheet = MakeCtx("inst1", "sheet-A", 1);
        var ctxGlobal = new SpreadsheetOperationContext("inst1", null, OperationKind.Structural, OperationId: 2);

        var enteredSheet = new TaskCompletionSource<bool>();
        var results = new List<int>();

        var sheetTask = scheduler.EnqueueSheet(ctxSheet, async () =>
        {
            results.Add(1);
            enteredSheet.TrySetResult(true);
            await Task.Delay(100);
            results.Add(2);
        });

        await enteredSheet.Task;
        var globalTask = scheduler.EnqueueGlobal(ctxGlobal, async () =>
        {
            results.Add(3);
        });

        await Task.WhenAll(sheetTask, globalTask);

        Assert.Equal([1, 2, 3], results);
    }

    [Fact]
    public async Task Structural_operation_waits_for_all_sheets()
    {
        var ctxA = MakeCtx("inst1", "sheet-A", 1);
        var ctxB = MakeCtx("inst1", "sheet-B", 2);
        var ctxStructural = new SpreadsheetOperationContext("inst1", null, OperationKind.Structural, OperationId: 3);

        var gateA = new TaskCompletionSource<bool>();
        var results = new List<int>();

        var tA = scheduler.EnqueueSheet(ctxA, async () =>
        {
            results.Add(1);
            await gateA.Task;
            results.Add(3);
        });

        var tB = scheduler.EnqueueSheet(ctxB, async () =>
        {
            results.Add(2);
        });

        var tStructural = scheduler.EnqueueGlobal(ctxStructural, async () =>
        {
            results.Add(4);
        });

        await Task.Delay(50);
        gateA.TrySetResult(true);

        await Task.WhenAll(tA, tB, tStructural);

        Assert.Contains(1, results);
        Assert.Contains(2, results);
        Assert.Contains(3, results);
        Assert.Contains(4, results);
        Assert.True(results.IndexOf(1) < results.IndexOf(3));
    }

    [Fact]
    public async Task PendingCount_increases_after_enqueue()
    {
        var ctx = MakeCtx("inst1", "sheet-A", 1);
        var gate = new TaskCompletionSource<bool>();
        var slowCtx = MakeCtx("inst1", "sheet-A", 2);

        var t1 = scheduler.EnqueueSheet(ctx, async () =>
        {
            await Task.Delay(100);
            await gate.Task;
        });

        await Task.Delay(20);

        var t2 = scheduler.EnqueueSheet(slowCtx, async () => { });

        var count = scheduler.PendingSheetCount("inst1", "sheet-A");
        Assert.True(count >= 1, $"Expected at least 1 pending, got {count}");

        gate.TrySetResult(true);
        await Task.WhenAll(t1, t2);
    }

    private static SpreadsheetOperationContext MakeCtx(string instanceId, string sheetId, long opId)
        => new(instanceId, sheetId, OperationKind.Sheet, OperationId: opId);
}
