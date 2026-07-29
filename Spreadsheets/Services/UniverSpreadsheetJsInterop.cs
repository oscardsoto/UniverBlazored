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
        moduleTask = new (() => runtime.InvokeAsync<IJSObjectReference>("import", "./_content/UniverBlazored/univer/xlsx/initUniver.js?v=1.5").AsTask());
        actionQueue = new();
    }

    public async Task InitializeAsync(string newIdDiv)
        => await InitializeAsync("default", newIdDiv);

    public async Task InitializeAsync(string instanceId, string newIdDiv)
    {
        var imports = new UniverJsImports(runtime);
        var univers = GetUniverLinks();
        foreach (var item in univers)
        {
            var ok = await imports.ImportLibrary(item);
            if (ok)
                continue;
        }
            
        await imports.DisposeAsync();
        await Task.Delay(1000);
        var module = await moduleTask.Value;
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

    public string[] GetUniverLinks()
    {
        List<string> links = [
            "https://unpkg.com/react@18.3.1/umd/react.production.min.js",
            "https://unpkg.com/react-dom@18.3.1/umd/react-dom.production.min.js",
            "https://unpkg.com/rxjs/dist/bundles/rxjs.umd.min.js",
            "https://cdnjs.cloudflare.com/ajax/libs/lodash.js/4.17.21/lodash.min.js",

            $"https://unpkg.com/@univerjs/presets@{config.Version}/lib/umd/index.js",
            $"https://unpkg.com/@univerjs/preset-sheets-core@{config.Version}/lib/umd/index.js",
            $"https://unpkg.com/@univerjs/preset-sheets-core@{config.Version}/lib/umd/locales/{config.Language}.js",
            $"https://unpkg.com/@univerjs/preset-sheets-core@{config.Version}/lib/index.css",

            $"https://unpkg.com/@univerjs/preset-sheets-find-replace@{config.Version}/lib/umd/index.js",
            $"https://unpkg.com/@univerjs/preset-sheets-find-replace@{config.Version}/lib/umd/locales/{config.Language}.js",
            $"https://unpkg.com/@univerjs/preset-sheets-find-replace@{config.Version}/lib/index.css"
        ];

        var univerConfig = config.InitialConfig;
        if (univerConfig.hasShort)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-sort@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-sort@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-sort@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasDataValidation)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-data-validation@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-data-validation@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-data-validation@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasFilter)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-filter@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-filter@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-filter@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasConditionalFormatting)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-conditional-formatting@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-conditional-formatting@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-conditional-formatting@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasHyperLink)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-hyper-link@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-hyper-link@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-hyper-link@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasDrawing)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-drawing@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-drawing@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-drawing@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasThreadComment)
            links.AddRange([
                $"https://unpkg.com/@univerjs/preset-sheets-thread-comment@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/preset-sheets-thread-comment@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/preset-sheets-thread-comment@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasWatermark)
            links.AddRange([
                $"https://unpkg.com/@univerjs/watermark@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/watermark@{config.Version}/lib/umd/facade.js",
            ]);

        if (univerConfig.hasCrosshair)
            links.AddRange([
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/umd/facade.js",
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/index.css"
            ]);
        
        return links.ToArray();
    }
    
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
