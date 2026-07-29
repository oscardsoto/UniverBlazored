namespace UniverBlazored.Spreadsheets.Services;

public interface ISpreadsheetCommandScheduler : IAsyncDisposable
{
    Task EnqueueSheet(SpreadsheetOperationContext context, Func<Task> operation);
    Task<T> EnqueueSheet<T>(SpreadsheetOperationContext context, Func<Task<T>> operation);
    Task ExecuteSheetAsync(SpreadsheetOperationContext context, Func<Task> operation);
    Task<T> ExecuteSheetAsync<T>(SpreadsheetOperationContext context, Func<Task<T>> operation);
    Task EnqueueGlobal(SpreadsheetOperationContext context, Func<Task> operation);
    Task<T> EnqueueGlobal<T>(SpreadsheetOperationContext context, Func<Task<T>> operation);
    int PendingSheetCount(string instanceId, string sheetId);
    bool IsDisposed { get; }
}
