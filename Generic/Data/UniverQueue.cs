using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Generic.Data;

public class UniverQueue
{
    protected IUniverJsInterop UniverJS { get; set; }
    protected Queue<UniverQueueValue> ActionQueue { get; set; }
    protected SpreadsheetOperationContext? OperationContext { get; set; }

    public UniverQueue(IUniverJsInterop univerJs, SpreadsheetOperationContext? context = null)
    {
        UniverJS = univerJs;
        ActionQueue = new();
        OperationContext = context;
    }

    public UniverQueue SetAction(string action, params object[] args)
    {
        ActionQueue.Enqueue(new UniverQueueValue(action, args));
        return this;
    }

    public UniverQueueValue[] ToArray() => ActionQueue.ToArray();

    public async Task ResolveQueueAsync()
    {
        if (OperationContext.HasValue)
            await UniverJS.ResolveAsync(OperationContext.Value, ActionQueue);
        else
            await UniverJS.ResolveAsync(ActionQueue);
        ActionQueue.Clear();
    }

    public async Task<T> ResolveQueueAsync<T>()
    {
        T result;
        if (OperationContext.HasValue)
            result = await UniverJS.ResolveAsync<T>(OperationContext.Value, ActionQueue);
        else
            result = await UniverJS.ResolveAsync<T>(ActionQueue);
        ActionQueue.Clear();
        return result;
    }
}
