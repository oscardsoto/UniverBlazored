using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;

namespace UniverBlazored.Spreadsheets.Services;

/// <summary>
/// Interface that provides interoperability with Univer's spreadsheets and Blazor.
/// </summary>
public class UniverSpreadsheetJsInterop : IUniverJsInterop
{
    private readonly IJSRuntime runtime;

    private readonly UniverConfig config;

    /// <summary>
    /// Module to initialize Univer in the component
    /// </summary>
    public Lazy<Task<IJSObjectReference>> moduleTask { get; private set; }

    /// <summary>
    /// Action Queue for execute in FacadeAPI for Univer
    /// </summary>
    /// <value></value>
    public Queue<UniverQueueValue> actionQueue { get; private set; }

    /// <summary>
    /// Interface that provides interoperability with Univer's spreadsheets and Blazor.
    /// </summary>
    /// <param name="runtime">Js Runtime</param>
    /// <param name="options">Configuration options</param>
    public UniverSpreadsheetJsInterop(IJSRuntime runtime, IOptions<UniverConfig> options)
    {
        this.runtime = runtime;
        config = options.Value;
        moduleTask = new (() => runtime.InvokeAsync<IJSObjectReference>("import", "./_content/UniverBlazored/univer/xlsx/initUniver.min.js?v=1.1").AsTask());
        actionQueue = new();
    }

    /// <summary>
    /// Sets all instances of all scripts from Univer acording to the version
    /// </summary>
    /// <param name="newIdDiv">The new Id for the div to execute Univer</param>
    /// <returns></returns>
    public async Task InitializeAsync(string newIdDiv)
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
        await Task.Delay(1000);         // 1 sec delay for waiting to all presets to charge
        var module = await moduleTask.Value;
        config.InitialConfig.SetNewIdDiv(newIdDiv);
        await module.InvokeVoidAsync("initUniver", config.InitialConfig);
    }

    /// <summary>
    /// Sets an action for the FacadeAPI 
    /// </summary>
    /// <param name="action">Method's name to execute on the Facade's API</param>
    /// <param name="args">Arguments for the method (must be serializables for Json to pass into JS)</param>
    /// <returns>This, for linking</returns>   
    public IUniverJsInterop SetAction(string action, params object[] args)
    {
        actionQueue.Enqueue(new(action, args));
        return this;
    }

    /// <summary>
    /// Resolve the queue
    /// </summary>
    /// <returns></returns>
    public async Task ResolveAsync()
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<bool>>("getAndExecuteMethod", actionQueue.ToArray(), false);
        if (!result.res)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
    }

    /// <summary>
    /// Resolve the queue
    /// </summary>
    /// <typeparam name="T">Type of value to be expected</typeparam>
    /// <returns></returns>
    public async Task<T> ResolveAsync<T>()
    {
        var module = await moduleTask.Value;
        var result = await module.InvokeAsync<UniverResponse<T>>("getAndExecuteMethod", actionQueue.ToArray(), true);
        if (result == null)
            throw new UniverException($"Method in queue cannot be found in pointer.");
        actionQueue.Clear();
        return result.res;
    }

    /// <summary>
    /// Gets all scripts components for Univer
    /// </summary>
    /// <returns>An array of each Univer library to import</returns>
    public string[] GetUniverLinks()
    {
        // Must-have Links: React, UniverCore Presets & UniverFindAndReplace Presets
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

        // Links depending the Univer's config object
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
                //$"https://unpkg.com/@univerjs/watermark@{config.Version}/lib/umd/locales/{config.Language}.js",
                //$"https://unpkg.com/@univerjs/watermark@{config.Version}/lib/index.css"
            ]);

        if (univerConfig.hasCrosshair)
            links.AddRange([
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/umd/index.js",
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/umd/facade.js",
                //$"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/umd/locales/{config.Language}.js",
                $"https://unpkg.com/@univerjs/sheets-crosshair-highlight@{config.Version}/lib/index.css"
            ]);
        
        return links.ToArray();
    }
    
    /// <summary>
    /// Resolve a function in the javascript module
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="args">Arguments for that method</param>
    /// <typeparam name="T">Type of value to return from that method</typeparam>
    /// <returns>The type desired for the function</returns>
    public async Task<T> ResolveActionAsync<T>(string name, params object[] args)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<T>(name, args);
    }

    /// <summary>
    /// Resolve a function in the javascript module
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="args">Arguments for that method</param>
    /// <returns></returns>
    public async Task ResolveActionAsync(string name, params object[] args)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync(name, args);
    }
}