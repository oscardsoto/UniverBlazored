namespace UniverBlazored.Generic.Data;

/// <summary>
/// Action Queue for execute in FacadeAPI for Univer
/// </summary>
public class UniverQueue
{
    /// <summary>
    /// Js Interop to Univer
    /// </summary>
    protected IUniverJsInterop UniverJS { get; set; }

    /// <summary>
    /// Queue container
    /// </summary>
    protected Queue<UniverQueueValue> ActionQueue { get; set; }

    /// <summary>
    /// Action Queue for execute in FacadeAPI for Univer
    /// </summary>
    /// <param name="univerJs"></param>
    public UniverQueue(IUniverJsInterop univerJs)
    {
        UniverJS = univerJs;
        ActionQueue = new();
    }

    /// <summary>
    /// Sets an action for the FacadeAPI 
    /// </summary>
    /// <param name="action">Method's name to execute on the Facade's API</param>
    /// <param name="args">Arguments for the method (must be serializables for Json to pass into JS)</param>
    /// <returns>This, for linking</returns>   
    public UniverQueue SetAction(string action, params object[] args)
    {
        ActionQueue.Enqueue(new UniverQueueValue(action, args));
        return this;
    }

    /// <summary>
    /// Return the queue value from this to an array
    /// </summary>
    /// <returns></returns>
    public UniverQueueValue[] ToArray() => ActionQueue.ToArray();

    /// <summary>
    /// Resolves all actions from the queue in hte JsInterop
    /// </summary>
    /// <returns></returns>
    public async Task ResolveQueueAsync()
    {
        await UniverJS.ResolveAsync(ActionQueue);
        ActionQueue.Clear();
    }

    /// <summary>
    /// Resolves all actions from the queue in hte JsInterop
    /// </summary>
    /// <typeparam name="T">Type of return</typeparam>
    /// <returns></returns>
    public async Task<T> ResolveQueueAsync<T>()
    {
        var result = await UniverJS.ResolveAsync<T>(ActionQueue);
        ActionQueue.Clear();
        return result;
    }
}