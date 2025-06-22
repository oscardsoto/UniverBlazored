using Microsoft.JSInterop;

namespace UniverBlazored.Generic;

/// <summary>
/// Module to extract scripts and import them on a page
/// </summary>
public class UniverJsImports : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    /// <summary>
    /// Module to extract scripts and import them on a page
    /// </summary>
    /// <param name="jsRuntime">Javascript Runtime</param>
    public UniverJsImports(IJSRuntime jsRuntime)
    {
        moduleTask = new (() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/UniverBlazored/univer/imports.min.js").AsTask());
    }

    /// <summary>
    /// Import the script for use in the page that has Univer's component
    /// </summary>
    /// <param name="uris">List of scripts that will be added to the "head" tag.</param>
    /// <returns></returns>
    public async Task ImportLibrary(params string[] uris)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("chargeScript", uris);
    }

    /// <summary>
    /// Dispose the component
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}