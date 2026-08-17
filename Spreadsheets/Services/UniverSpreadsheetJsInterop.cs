using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Spreadsheets.Services;

public class UniverSpreadsheetJsInterop : IUniverJsInterop
{
    private readonly IJSRuntime runtime;

    private readonly UniverConfig config;

    public Lazy<Task<IJSObjectReference>> moduleTask { get; private set; }

    public Queue<UniverQueueValue> actionQueue { get; private set; }

    public UniverSpreadsheetJsInterop(IJSRuntime runtime, IOptions<UniverConfig> options)
    {
        this.runtime = runtime;
        config = options.Value;
        moduleTask = new (() => runtime.InvokeAsync<IJSObjectReference>("import", "./_content/UniverBlazored/univer/xlsx/initUniver.js?v=1.6").AsTask());
        actionQueue = new();
    }

    public async Task InitializeAsync(string newIdDiv)
        => await InitializeAsync("default", newIdDiv);

    public async Task InitializeAsync(string instanceId, string newIdDiv)
    {
        var module = await moduleTask.Value;

        var timeout = TimeSpan.FromSeconds(config.ScriptLoadTimeoutSeconds);
        var sw = Stopwatch.StartNew();
        while (!await module.InvokeAsync<bool>("areScriptsReady"))
        {
            if (sw.Elapsed >= timeout)
                throw new TimeoutException($"Univer scripts did not become ready within {timeout.TotalSeconds} seconds. Please ensure that the UniverSpreadsheetResources component is included in your page.");
            await Task.Delay(config.ScriptLoadPollIntervalMs);
        }

        config.InitialConfig.SetNewIdDiv(newIdDiv);
        await module.InvokeVoidAsync("initUniver", instanceId, config.InitialConfig, config.Language);
    }

    public IUniverJsInterop SetAction(string action, params object[] args)
    {
        actionQueue.Enqueue(new(action, args));
        return this;
    }

    public async Task ResolveAsync()
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<bool>>("getAndExecuteMethod", "default", actionQueue.ToArray(), false);
        if (!result.res)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
    }

    public async Task ResolveAsync(Queue<UniverQueueValue> actionQueue)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<bool>>("getAndExecuteMethod", "default", actionQueue.ToArray(), false);
        if (!result.res)
            throw new UniverException($"Method in queue cannot be found in pointer.");
    }

    public async Task<T> ResolveAsync<T>()
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<T>>("getAndExecuteMethod", "default", actionQueue.ToArray(), true);
        if (result == null)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
        return result.res;
    }

    public async Task<T> ResolveAsync<T>(Queue<UniverQueueValue> actionQueue)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<T>>("getAndExecuteMethod", "default", actionQueue.ToArray(), true);
        if (result == null)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        return result.res;
    }

    public async Task ResolveAsync(SpreadsheetOperationContext context)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<bool>>("getAndExecuteMethod", context.InstanceId, actionQueue.ToArray(), false);
        if (!result.res)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
    }

    public async Task<T> ResolveAsync<T>(SpreadsheetOperationContext context)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<T>>("getAndExecuteMethod", context.InstanceId, actionQueue.ToArray(), true);
        if (result == null)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
        return result.res;
    }

    public async Task ResolveAsync(SpreadsheetOperationContext context, Queue<UniverQueueValue> actionQueue)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<bool>>("getAndExecuteMethod", context.InstanceId, actionQueue.ToArray(), false);
        if (!result.res)
            throw new UniverException($"Method in queue cannot be found in pointer.");
    }

    public async Task<T> ResolveAsync<T>(SpreadsheetOperationContext context, Queue<UniverQueueValue> actionQueue)
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<T>>("getAndExecuteMethod", context.InstanceId, actionQueue.ToArray(), true);
        if (result == null)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        return result.res;
    }

    public Task ExecuteAtomicAsync(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task> operation)
        => operation(this);

    public Task<T> ExecuteAtomicAsync<T>(SpreadsheetOperationContext context, Func<IUniverJsInterop, Task<T>> operation)
        => operation(this);
    
    public async Task<T> ResolveActionAsync<T>(string name, params object[] args)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<T>(name, args);
    }

    public async Task ResolveActionAsync(string name, params object[] args)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync(name, args);
    }
}
