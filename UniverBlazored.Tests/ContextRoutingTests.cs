using UniverBlazored.Generic.Services;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Tests;

public sealed class ContextRoutingTests
{
    private readonly FakeUniverJsInterop fake = new();
    private readonly SpreadsheetCommandScheduler scheduler = new();

    [Fact]
    public async Task SafeInterop_routes_by_context()
    {
        var safe = CreateSafeInterop();
        var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet);

        await safe.ResolveAsync(ctx);

        var action = fake.Actions.Single();
        Assert.Equal("inst1", action.InstanceId);
        Assert.Equal("sheet-A", action.Context?.SheetId);
    }

    [Fact]
    public async Task SafeInterop_routes_global_when_no_sheet()
    {
        var safe = CreateSafeInterop();
        var ctx = new SpreadsheetOperationContext("inst1", null, OperationKind.Structural);

        await safe.ResolveAsync(ctx);

        var action = fake.Actions.Single();
        Assert.Null(action.Context?.SheetId);
    }

    [Fact]
    public async Task SafeInterop_ExecuteAtomicAsync_uses_scheduler()
    {
        var safe = CreateSafeInterop();
        var ctx = new SpreadsheetOperationContext("inst1", "sheet-B", OperationKind.Sheet);
        var executed = false;

        await safe.ExecuteAtomicAsync(ctx, async inner =>
        {
            await Task.Yield();
            executed = true;
        });

        Assert.True(executed);
    }

    [Fact]
    public async Task ForSheetId_creates_context_with_correct_sheet()
    {
        var agent = new UniverSpreadsheetAgent(fake, "inst2");
        var sheetId = "sheet-X";

        var scope = agent.ForSheetId(sheetId);
        var dataCommands = scope.Data();

        Assert.NotNull(dataCommands);
    }

    [Fact]
    public async Task BatchScope_flushes_operations()
    {
        var fake2 = new FakeUniverJsInterop();
        var scope = new BatchScope(fake2, "inst3", "sheet-Z");

        scope.SetValue(new URange { startRow = 0, startColumn = 0, endRow = 0, endColumn = 0 }, 42);
        await scope.FlushAsync();

        var action = fake2.Actions.Single();
        Assert.Equal(RecordedActionKind.ResolveAction, action.Kind);
        Assert.Contains("batchSheetOperations", action.ActionName, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task BatchScope_dispose_flushes()
    {
        var fake2 = new FakeUniverJsInterop();

        await using (var scope = new BatchScope(fake2, "inst4", "sheet-W"))
        {
            scope.SetValue(new URange { startRow = 0, startColumn = 0, endRow = 0, endColumn = 0 }, 99);
        }

        Assert.Single(fake2.Actions);
    }

    [Fact]
    public async Task OperationContext_includes_IdempotencyKey()
    {
        var ctx = new SpreadsheetOperationContext("inst1", "sheet-A", OperationKind.Sheet,
            OperationId: 42, IdempotencyKey: "key-abc");

        Assert.Equal(42, ctx.OperationId);
        Assert.Equal("key-abc", ctx.IdempotencyKey);
    }

    private SafeUniverJsInterop CreateSafeInterop()
        => new(fake, scheduler, TestLogger<SafeUniverJsInterop>.Instance);
}

// NullLogger<T> is available from Microsoft.Extensions.Logging namespace
