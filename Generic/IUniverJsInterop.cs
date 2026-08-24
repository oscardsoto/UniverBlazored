using System.Text.Json.Nodes;
using Microsoft.JSInterop;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Generic;

public interface IUniverJsInterop
{
    Lazy<Task<IJSObjectReference>> moduleTask { get; }
    Queue<UniverQueueValue> actionQueue { get; }

    Task InitializeAsync(string newIdDiv);
    Task InitializeAsync(string instanceId, string newIdDiv);
    Task InitializeAsync(string instanceId, string newIdDiv, UniverInit initConfig);
    IUniverJsInterop SetAction(string action, params object[] args);
    Task ResolveAsync();
    Task<T> ResolveAsync<T>();
    Task ResolveAsync(Queue<UniverQueueValue> actionQueue);
    Task<T> ResolveAsync<T>(Queue<UniverQueueValue> actionQueue);

    Task ResolveAsync(SpreadsheetOperationContext context);
    Task<T> ResolveAsync<T>(SpreadsheetOperationContext context);
    Task ResolveAsync(SpreadsheetOperationContext context, Queue<UniverQueueValue> actionQueue);
    Task<T> ResolveAsync<T>(SpreadsheetOperationContext context, Queue<UniverQueueValue> actionQueue);

    Task ExecuteAtomicAsync(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task> operation);
    Task<T> ExecuteAtomicAsync<T>(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task<T>> operation);

    Task<T> ResolveActionAsync<T>(string name, params object[] args);
    Task ResolveActionAsync(string name, params object[] args);
}
