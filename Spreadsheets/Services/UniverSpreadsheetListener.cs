using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Spreadsheets.Data;

namespace UniverBlazored.Spreadsheets.Services;

public class UniverSpreadsheetListener(IUniverJsInterop univerJS) : IUniverSpreadsheetListener, IAsyncDisposable
{
    private readonly Dictionary<string, InstanceState> instances = new();
    private bool disposed;

    public Dictionary<UniverSpreadsheetListenerData, Func<object, Task>> Listeners { get; private set; } = new();

    public async Task InitializeListenersAsync()
        => await InitializeListenersAsync("default");

    public async Task InitializeListenersAsync(string instanceId)
    {
        if (!instances.TryGetValue(instanceId, out var state))
        {
            state = new InstanceState { Reference = DotNetObjectReference.Create(this) };
            instances[instanceId] = state;
        }
        state.Reference ??= DotNetObjectReference.Create(this);
        await univerJS.ResolveActionAsync("initListenerObject", instanceId, state.Reference);
    }

    public async Task AddListenerAsync(UniverSpreadsheetListenerData data, Func<object, Task> _event)
    {
        var instanceId = data.instanceId;
        if (string.IsNullOrEmpty(instanceId))
        {
            instanceId = instances.Keys.LastOrDefault() ?? "default";
        }
        await univerJS.ResolveActionAsync("addListenerRange", instanceId, data);
        Listeners.Add(data, _event);
    }

    public async Task RemoveListenerAsync(UniverSpreadsheetListenerData data)
    {
        var instanceId = data.instanceId;
        if (string.IsNullOrEmpty(instanceId))
        {
            instanceId = instances.Keys.LastOrDefault() ?? "default";
        }
        await univerJS.ResolveActionAsync("removeListener", instanceId, data);
        Listeners.Remove(data);
    }

    public async Task RemoveAllListenersAsync(string instanceId)
    {
        var keysToRemove = Listeners.Keys.Where(k => k.instanceId == instanceId).ToList();
        foreach (var key in keysToRemove)
            Listeners.Remove(key);
        await univerJS.ResolveActionAsync("removeAllEvents", instanceId);
    }

    public UniverSpreadsheetListenerData[] GetListeners() => Listeners.Keys.ToArray();

    private int reentrancyDepth;

    [JSInvokable]
    public async Task OnDataChanged(UniverSpreadsheetListenerData data, object value)
    {
        if (disposed) return;
        if (Listeners.TryGetValue(data, out var handler))
        {
            reentrancyDepth++;
            try
            {
                await handler(value);
            }
            finally
            {
                reentrancyDepth--;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (disposed) return;
        disposed = true;

        foreach (var instanceId in instances.Keys.ToArray())
        {
            try
            {
                await univerJS.ResolveActionAsync("removeAllEvents", instanceId);
            }
            catch
            {
            }
        }

        Listeners.Clear();
        foreach (var state in instances.Values)
            state.Reference?.Dispose();
        instances.Clear();
    }

    private sealed class InstanceState
    {
        public DotNetObjectReference<UniverSpreadsheetListener>? Reference;
    }
}
