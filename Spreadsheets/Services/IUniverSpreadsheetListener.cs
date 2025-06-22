using UniverBlazored.Spreadsheets.Data;

namespace UniverBlazored.Spreadsheets.Services;

/// <summary>
/// Univer Listener. A listener gets automatically the value of one cell every time its value has changed
/// </summary>
public interface IUniverSpreadsheetListener
{
    /// <summary>
    /// All active Listeners
    /// </summary>
    Dictionary<UniverSpreadsheetListenerData, Action<object>> Listeners { get; }

    /// <summary>
    /// Init the dictionary and the relation in the JSInterop
    /// </summary>
    Task InitializeListenersAsync();

    /// <summary>
    /// Adds a new listener to the array
    /// </summary>
    /// <param name="data">Data to locate the listener in the workbook</param>
    /// <param name="_event">Triggers when the cell in the listener change its value</param>
    Task AddListenerAsync(UniverSpreadsheetListenerData data, Action<object> _event);

    /// <summary>
    /// Removes the specified listener in the array
    /// </summary>
    /// <param name="data">Item to delete</param>
    Task RemoveListenerAsync(UniverSpreadsheetListenerData data);

    /// <summary>
    /// Returns all active listeners
    /// </summary>
    /// <returns></returns>
    UniverSpreadsheetListenerData[] GetListeners();

    /// <summary>
    /// JSInvokable: Event for the Manager to trigger the listener, depending the cell that was value changed
    /// </summary>
    /// <param name="data">Data Listener</param>
    /// <param name="value">Value obtained</param>
    void OnDataChanged(UniverSpreadsheetListenerData data, object value);
}