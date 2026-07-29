namespace UniverBlazored.Tests;

public sealed class SchedulerCancellationTests
{
    [Fact]
    public async Task Cancel_before_enqueue_throws_OperationCanceledException()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        using var cts = new CancellationTokenSource();
        var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet, CancellationToken: cts.Token);

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            scheduler.EnqueueSheet(ctx, async () => await Task.Yield()));
    }

    [Fact]
    public async Task Dispose_drains_pending_operations()
    {
        var scheduler = new SpreadsheetCommandScheduler();
        var ctx = MakeCtx("inst1", "sheet-A", 1);
        var executed = false;

        var task = scheduler.EnqueueSheet(ctx, async () =>
        {
            await Task.Delay(50);
            executed = true;
        });

        await scheduler.DisposeAsync();
        await task;

        Assert.True(executed);
    }

    [Fact]
    public async Task Dispose_twice_is_safe()
    {
        var scheduler = new SpreadsheetCommandScheduler();
        await scheduler.DisposeAsync();
        await scheduler.DisposeAsync();
    }

    [Fact]
    public async Task Operations_after_dispose_throw()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        await scheduler.DisposeAsync();

        var ctx = MakeCtx("inst1", "sheet-A", 1);
        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            scheduler.EnqueueSheet(ctx, () => Task.CompletedTask));
    }

    [Fact]
    public async Task IsDisposed_returns_true_after_disposal()
    {
        var scheduler = new SpreadsheetCommandScheduler();
        Assert.False(scheduler.IsDisposed);
        await scheduler.DisposeAsync();
        Assert.True(scheduler.IsDisposed);
    }

    [Fact]
    public async Task SheetId_is_required_for_EnqueueSheet()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var ctx = new SpreadsheetOperationContext("inst1", null, OperationKind.Sheet);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            scheduler.EnqueueSheet(ctx, () => Task.CompletedTask));
        Assert.Contains("SheetId", ex.Message);
    }

    [Fact]
    public async Task CancellationToken_propagates_from_context()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        using var cts = new CancellationTokenSource();
        var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet, CancellationToken: cts.Token);

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            scheduler.ExecuteSheetAsync(ctx, async () =>
            {
                await Task.Delay(1000, CancellationToken.None);
            }));
    }

    private static SpreadsheetOperationContext MakeCtx(string instanceId, string sheetId, long opId)
        => new(instanceId, sheetId, OperationKind.Sheet, OperationId: opId);
}
