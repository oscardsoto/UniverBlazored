using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Services;
using UniverBlazored.Spreadsheets.Services.Commands;

namespace UniverBlazored.Generic.Services;

public sealed class SafeUniverJsInterop : IUniverJsInterop
{
    private readonly IUniverJsInterop inner;
    private readonly ISpreadsheetCommandScheduler scheduler;
    private readonly ILogger<SafeUniverJsInterop> logger;

    public SafeUniverJsInterop(
        IUniverJsInterop inner,
        ISpreadsheetCommandScheduler scheduler,
        ILogger<SafeUniverJsInterop> logger)
    {
        this.inner = inner;
        this.scheduler = scheduler;
        this.logger = logger;
    }

    public Lazy<Task<IJSObjectReference>> moduleTask => inner.moduleTask;
    public Queue<UniverQueueValue> actionQueue => inner.actionQueue;

    public Task InitializeAsync(string newIdDiv)
        => ExecuteGlobal("InitializeAsync", () => inner.InitializeAsync(newIdDiv), newIdDiv);

    public Task InitializeAsync(string instanceId, string newIdDiv)
        => ExecuteGlobal("InitializeAsync", () => inner.InitializeAsync(instanceId, newIdDiv), instanceId, newIdDiv);

    public IUniverJsInterop SetAction(string action, params object[] args) => inner.SetAction(action, args);

    public Task ResolveAsync(SpreadsheetOperationContext context)
        => RouteByContext(context, () => inner.ResolveAsync(context));

    public Task<T> ResolveAsync<T>(SpreadsheetOperationContext context)
        => RouteByContext(context, () => inner.ResolveAsync<T>(context));

    public Task ResolveAsync(SpreadsheetOperationContext context, Queue<UniverQueueValue> queue)
        => RouteByContext(context, () => inner.ResolveAsync(context, queue));

    public Task<T> ResolveAsync<T>(SpreadsheetOperationContext context, Queue<UniverQueueValue> queue)
        => RouteByContext(context, () => inner.ResolveAsync<T>(context, queue));

    public Task ResolveAsync() => ExecuteQueue("ResolveAsync", inner.ResolveAsync, actionQueue.ToArray());
    public Task<T> ResolveAsync<T>() => ExecuteQueue("ResolveAsync<T>", inner.ResolveAsync<T>, actionQueue.ToArray());
    public Task ResolveAsync(Queue<UniverQueueValue> queue) => ExecuteQueue("ResolveAsync(queue)", () => inner.ResolveAsync(queue), queue.ToArray());
    public Task<T> ResolveAsync<T>(Queue<UniverQueueValue> queue) => ExecuteQueue("ResolveAsync<T>(queue)", () => inner.ResolveAsync<T>(queue), queue.ToArray());

    public Task<T> ResolveActionAsync<T>(string name, params object[] args)
        => ExecuteForArguments(name, () => inner.ResolveActionAsync<T>(name, args), args);

    public Task ResolveActionAsync(string name, params object[] args)
        => ExecuteForArguments(name, () => inner.ResolveActionAsync(name, args), args);

    public Task ExecuteAtomicAsync(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task> operation)
        => scheduler.ExecuteSheetAsync(context, () => operation(inner));

    public Task<T> ExecuteAtomicAsync<T>(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task<T>> operation)
        => scheduler.ExecuteSheetAsync(context, () => operation(inner));

    public string[] GetUniverLinks() => inner.GetUniverLinks();

    private Task RouteByContext(SpreadsheetOperationContext context, Func<Task> operation)
        => context.SheetId is not null && context.Kind is OperationKind.Sheet
            ? scheduler.EnqueueSheet(context, () => Trace(GetOperationName(), context.SheetId, operation))
            : scheduler.EnqueueGlobal(context, () => Trace(GetOperationName(), "workbook", operation));

    private Task<T> RouteByContext<T>(SpreadsheetOperationContext context, Func<Task<T>> operation)
        => context.SheetId is not null && context.Kind is OperationKind.Sheet
            ? scheduler.EnqueueSheet(context, () => Trace(GetOperationName(), context.SheetId, operation))
            : scheduler.EnqueueGlobal(context, () => Trace(GetOperationName(), "workbook", operation));

    private Task ExecuteQueue(string operationName, Func<Task> operation, UniverQueueValue[] queue)
        => !IsWorkbookOperation(queue) && FindSheetId(queue) is { } sheetId
            ? ExecuteSheet(operationName, sheetId, operation)
            : ExecuteGlobal(operationName, operation);

    private Task<T> ExecuteQueue<T>(string operationName, Func<Task<T>> operation, UniverQueueValue[] queue)
        => !IsWorkbookOperation(queue) && FindSheetId(queue) is { } sheetId
            ? ExecuteSheet(operationName, sheetId, operation)
            : ExecuteGlobal(operationName, operation);

    private Task ExecuteForArguments(string operationName, Func<Task> operation, object[] args)
    {
        var snapshot = args.OfType<USpreadsheetSnapshot>().FirstOrDefault();
        var sheetId = FindSheetId(args);
        if (snapshot is not null && sheetId is null)
            return Task.FromException(new InvalidOperationException("Sheet context is required for spreadsheet commands."));
        if (sheetId is not null)
        {
            var instanceId = snapshot?.InstanceId ?? "default";
            var context = new SpreadsheetOperationContext(instanceId, sheetId, OperationKind.Sheet);
            return scheduler.EnqueueSheet(context, () => Trace(operationName, sheetId, operation));
        }
        return ExecuteGlobal(operationName, operation);
    }

    private Task<T> ExecuteForArguments<T>(string operationName, Func<Task<T>> operation, object[] args)
    {
        var snapshot = args.OfType<USpreadsheetSnapshot>().FirstOrDefault();
        var sheetId = FindSheetId(args);
        if (snapshot is not null && sheetId is null)
            return Task.FromException<T>(new InvalidOperationException("Sheet context is required for spreadsheet commands."));
        if (sheetId is not null)
        {
            var instanceId = snapshot?.InstanceId ?? "default";
            var context = new SpreadsheetOperationContext(instanceId, sheetId, OperationKind.Sheet);
            return scheduler.EnqueueSheet(context, () => Trace(operationName, sheetId, operation));
        }
        return ExecuteGlobal<T>(operationName, operation);
    }

    private Task ExecuteSheet(string name, string sheetId, Func<Task> operation)
    {
        var context = new SpreadsheetOperationContext("default", sheetId, OperationKind.Sheet);
        return scheduler.EnqueueSheet(context, () => Trace(name, sheetId, operation));
    }

    private Task<T> ExecuteSheet<T>(string name, string sheetId, Func<Task<T>> operation)
    {
        var context = new SpreadsheetOperationContext("default", sheetId, OperationKind.Sheet);
        return scheduler.EnqueueSheet(context, () => Trace(name, sheetId, operation));
    }

    private Task ExecuteGlobal(string name, Func<Task> operation, params object[] _)
    {
        var context = new SpreadsheetOperationContext("default", null, OperationKind.Structural);
        return scheduler.EnqueueGlobal(context, () => Trace(name, "workbook", operation));
    }

    private Task<T> ExecuteGlobal<T>(string name, Func<Task<T>> operation)
    {
        var context = new SpreadsheetOperationContext("default", null, OperationKind.Structural);
        return scheduler.EnqueueGlobal(context, () => Trace(name, "workbook", operation));
    }

    private async Task Trace(string name, string scope, Func<Task> operation)
    {
        var timer = Stopwatch.StartNew();
        try { await operation(); }
        catch (JSDisconnectedException ex)
        {
            logger.LogWarning(ex, "JS disconnected during {Operation} for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Univer operation {Operation} failed for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds);
            throw;
        }
        finally { logger.LogDebug("Univer operation {Operation} ended for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds); }
    }

    private async Task<T> Trace<T>(string name, string scope, Func<Task<T>> operation)
    {
        var timer = Stopwatch.StartNew();
        try { return await operation(); }
        catch (JSDisconnectedException ex)
        {
            logger.LogWarning(ex, "JS disconnected during {Operation} for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds);
            return default!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Univer operation {Operation} failed for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds);
            throw;
        }
        finally { logger.LogDebug("Univer operation {Operation} ended for {Scope} after {ElapsedMs} ms", name, scope, timer.ElapsedMilliseconds); }
    }

    private static string GetOperationName([System.Runtime.CompilerServices.CallerMemberName] string name = "") => name;

    private static string? FindSheetId(IEnumerable<UniverQueueValue> queue)
        => queue.FirstOrDefault(item => item.methodName == "getSheetBySheetId").args?.FirstOrDefault() as string;

    private static string? FindSheetId(IEnumerable<object> args)
    {
        var snapshotId = args.OfType<USpreadsheetSnapshot>()
            .Select(snapshot => snapshot.SheetSelected?.id)
            .FirstOrDefault(id => !string.IsNullOrWhiteSpace(id));
        return snapshotId ?? args.OfType<IEnumerable<UniverQueueValue>>()
            .Select(FindSheetId)
            .FirstOrDefault(id => !string.IsNullOrWhiteSpace(id));
    }

    private static bool IsWorkbookOperation(IEnumerable<UniverQueueValue> queue)
        => queue.Any(item => item.methodName is "create" or "deleteSheet" or "setActiveSheet" or "hideSheet" or "showSheet" or "toggleDarkMode" or "addFonts");
}
