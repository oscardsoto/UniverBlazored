using UniverBlazored.Spreadsheets.Data;

namespace UniverBlazored.Spreadsheets.Services;

public interface IUniverSpreadsheetListener
{
    Dictionary<UniverSpreadsheetListenerData, Func<object, Task>> Listeners { get; }

    Task InitializeListenersAsync();
    Task InitializeListenersAsync(string instanceId);

    Task AddListenerAsync(UniverSpreadsheetListenerData data, Func<object, Task> _event);
    Task RemoveListenerAsync(UniverSpreadsheetListenerData data);
    Task RemoveAllListenersAsync(string instanceId);

    UniverSpreadsheetListenerData[] GetListeners();

    Task OnDataChanged(UniverSpreadsheetListenerData data, object value);
}
