namespace UniverBlazored.Tests;

public sealed class TwoInstancesTests
{
    [Fact]
    public async Task Two_independent_instances_do_not_interfere()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var fake1 = new FakeUniverJsInterop();
        var fake2 = new FakeUniverJsInterop();

        var safe1 = new SafeUniverJsInterop(fake1, scheduler, TestLogger<SafeUniverJsInterop>.Instance);
        var safe2 = new SafeUniverJsInterop(fake2, scheduler, TestLogger<SafeUniverJsInterop>.Instance);

        var agent1 = new UniverSpreadsheetAgent(safe1, "inst-A");
        var agent2 = new UniverSpreadsheetAgent(safe2, "inst-B");

        var ctx1 = new SpreadsheetOperationContext("inst-A", "sheet-1", OperationKind.Sheet, OperationId: 1);
        var ctx2 = new SpreadsheetOperationContext("inst-B", "sheet-1", OperationKind.Sheet, OperationId: 2);

        var t1 = scheduler.EnqueueSheet(ctx1, async () =>
        {
            await safe1.ResolveActionAsync("setValue", "inst-A-value");
        });

        var t2 = scheduler.EnqueueSheet(ctx2, async () =>
        {
            await safe2.ResolveActionAsync("setValue", "inst-B-value");
        });

        await Task.WhenAll(t1, t2);

        Assert.Single(fake1.Actions);
        Assert.Single(fake2.Actions);
    }

    [Fact]
    public async Task Each_instance_has_separate_scheduler_queue()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();

        var ctxA = new SpreadsheetOperationContext("inst-A", "sheet-1", OperationKind.Sheet, OperationId: 1);
        var ctxB = new SpreadsheetOperationContext("inst-B", "sheet-1", OperationKind.Sheet, OperationId: 2);

        var gate = new TaskCompletionSource<bool>();
        var results = new List<string>();

        var tA = scheduler.EnqueueSheet(ctxA, async () =>
        {
            results.Add("A-start");
            await gate.Task;
            results.Add("A-end");
        });

        var tB = scheduler.EnqueueSheet(ctxB, async () =>
        {
            results.Add("B-start");
            gate.TrySetResult(true);
        });

        await Task.WhenAll(tA, tB);

        Assert.Contains("A-start", results);
        Assert.Contains("A-end", results);
        Assert.Contains("B-start", results);
    }

    [Fact]
    public async Task Same_instance_different_sheets_dont_block_each_other()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();

        var ctxA = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet, OperationId: 1);
        var ctxB = new SpreadsheetOperationContext("inst1", "sheet-B", OperationKind.Sheet, OperationId: 2);

        var gate = new TaskCompletionSource<bool>();
        var results = new List<string>();

        var tA = scheduler.EnqueueSheet(ctxA, async () =>
        {
            results.Add("A1");
            await gate.Task;
            results.Add("A2");
        });

        var tB = scheduler.EnqueueSheet(ctxB, async () =>
        {
            results.Add("B");
            gate.TrySetResult(true);
        });

        await Task.WhenAll(tA, tB);

        Assert.Contains("A1", results);
        Assert.Contains("A2", results);
        Assert.Contains("B", results);
        Assert.True(results.IndexOf("B") < results.IndexOf("A2"), "B must appear before A2 (B unblocks the gate A waits on)");
    }
}
