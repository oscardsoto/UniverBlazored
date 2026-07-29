using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Tests;

public sealed class BulkOperationsTests
{
    [Fact]
    public async Task Many_operations_on_same_sheet_maintain_order()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var count = 100;
        var results = new int[count];

        var tasks = new Task[count];
        for (var i = 0; i < count; i++)
        {
            var idx = i;
            var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet, OperationId: idx);
            tasks[idx] = scheduler.EnqueueSheet(ctx, async () =>
            {
                await Task.Yield();
                results[idx] = idx;
            });
        }

        await Task.WhenAll(tasks);

        for (var i = 0; i < count; i++)
            Assert.Equal(i, results[i]);
    }

    [Fact]
    public async Task Bulk_global_operations_are_serialized()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var count = 50;
        var results = new List<int>();

        var tasks = new Task[count];
        for (var i = 0; i < count; i++)
        {
            var idx = i;
            var ctx = new SpreadsheetOperationContext("inst1", null, OperationKind.Structural, OperationId: idx);
            tasks[idx] = scheduler.EnqueueGlobal(ctx, async () =>
            {
                await Task.Delay(1);
                results.Add(idx);
            });
        }

        await Task.WhenAll(tasks);

        for (var i = 0; i < count; i++)
            Assert.Equal(i, results[i]);
    }

    [Fact]
    public async Task Mixed_sheet_and_global_operations()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var results = new List<string>();

        var sheetTasks = new Task[5];
        for (var i = 0; i < 5; i++)
        {
            var idx = i;
            var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet, OperationId: idx);
            sheetTasks[idx] = scheduler.EnqueueSheet(ctx, async () =>
            {
                results.Add($"s{idx}");
            });
        }

        var globalCtx = new SpreadsheetOperationContext("inst1", null, OperationKind.Structural, OperationId: 99);
        var globalTask = scheduler.EnqueueGlobal(globalCtx, async () =>
        {
            results.Add("global");
        });

        await Task.WhenAll(sheetTasks.Concat([globalTask]).ToArray());

        Assert.Contains("global", results);
        Assert.Equal(6, results.Count);
    }

    [Fact]
    public async Task Concurrent_sheets_do_not_deadlock()
    {
        await using var scheduler = new SpreadsheetCommandScheduler();
        var sheetCount = 10;
        var tasks = new Task[sheetCount];

        for (var i = 0; i < sheetCount; i++)
        {
            var idx = i;
            var ctx = new SpreadsheetOperationContext("inst1", $"sheet-{i}", OperationKind.Sheet, OperationId: idx);
            tasks[idx] = scheduler.EnqueueSheet(ctx, async () =>
            {
                await Task.Delay(idx * 5);
            });
        }

        var timeout = Task.Delay(TimeSpan.FromSeconds(10));
        var all = Task.WhenAll(tasks);
        var completed = await Task.WhenAny(all, timeout);

        Assert.Same(all, completed);
    }

    [Fact]
    public async Task BatchScope_reduces_interop_calls()
    {
        var fake = new FakeUniverJsInterop();

        var scope = new BatchScope(fake, "inst1", "sheet-A");
        scope.SetValue(new URange { startRow = 0, startColumn = 0, endRow = 0, endColumn = 0 }, 1);
        scope.SetValue(new URange { startRow = 1, startColumn = 0, endRow = 1, endColumn = 0 }, 2);
        scope.SetColumnWidth(0, 100);
        await scope.FlushAsync();

        Assert.Single(fake.Actions);
    }
}
