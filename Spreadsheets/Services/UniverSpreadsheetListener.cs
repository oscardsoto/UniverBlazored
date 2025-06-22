using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Spreadsheets.Data;

namespace UniverBlazored.Spreadsheets.Services;

/// <summary>
/// Univer Listener. A listener gets automatically the value of one cell every time its value has changed
/// </summary>
/// <param name="univerJS">Univer's Interop to access Univer</param>
public class UniverSpreadsheetListener(IUniverJsInterop univerJS) : IUniverSpreadsheetListener
{
    /// <summary>
    /// All active Listeners
    /// </summary>
    /// 
    public Dictionary<UniverSpreadsheetListenerData, Action<object>> Listeners { get; private set; } = new();

    /// <summary>
    /// Init the dictionary and the relation in the JSInterop
    /// </summary>
    public async Task InitializeListenersAsync()
    {
        Listeners = new();
        var reference = DotNetObjectReference.Create(this);
        await univerJS.ResolveActionAsync("initListenerObject", reference);
    }

    /// <summary>
    /// Adds a new listener to the array
    /// </summary>
    /// <param name="data">Data to locate the listener in the workbook</param>
    /// <param name="_event">Triggers when the cell in the listener change its value</param>
    public async Task AddListenerAsync(UniverSpreadsheetListenerData data, Action<object> _event)
    {
        await univerJS.ResolveActionAsync("addListenerRange", data);
        Listeners.Add(data, _event);
    }

    /// <summary>
    /// Removes the specified listener in the array
    /// </summary>
    /// <param name="data">Item to delete</param>
    public async Task RemoveListenerAsync(UniverSpreadsheetListenerData data)
    {
        await univerJS.ResolveActionAsync("removeListener", data);
        Listeners.Remove(data);
    } 

    /// <summary>
    /// Returns all active listeners
    /// </summary>
    /// <returns></returns>
    public UniverSpreadsheetListenerData[] GetListeners() => Listeners.Keys.ToArray();
    
    /// <summary>
    /// JSInvokable: Event for the Manager to trigger the listener, depending the cell that was value changed
    /// </summary>
    /// <param name="data">Data Listener</param>
    /// <param name="value">Value obtained</param>
    [JSInvokable]
    public void OnDataChanged(UniverSpreadsheetListenerData data, object value)
    {
        Listeners[data].Invoke(value);
    }
}