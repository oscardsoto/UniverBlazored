using UniverBlazored.Generic;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

public sealed class BatchScope : IAsyncDisposable
{
    private readonly IUniverJsInterop univerJS;
    private readonly string instanceId;
    private readonly string sheetId;
    private readonly List<object> operations = new();
    private bool flushed;

    internal BatchScope(IUniverJsInterop univerJS, string instanceId, string sheetId)
    {
        this.univerJS = univerJS;
        this.instanceId = instanceId;
        this.sheetId = sheetId;
    }

    public BatchScope SetValue(URange range, object value)
    {
        operations.Add(new { range, method = "setValue", args = new object[] { value } });
        return this;
    }

    public BatchScope SetValues(URange range, object[,] values)
    {
        operations.Add(new { range, method = "setValues", args = new object[] { values } });
        return this;
    }

    public BatchScope SetFormula(URange range, string formula)
    {
        operations.Add(new { range, method = "setFormula", args = new object[] { formula } });
        return this;
    }

    public BatchScope SetFormulas(URange range, string[,] formulas)
    {
        operations.Add(new { range, method = "setFormulas", args = new object[] { formulas } });
        return this;
    }

    public BatchScope SetColumnWidth(int colPos, double width)
    {
        operations.Add(new { range = (URange?)null, method = "setColumnWidth", args = new object[] { colPos, width } });
        return this;
    }

    public BatchScope SetRowHeight(int rowPos, double height)
    {
        operations.Add(new { range = (URange?)null, method = "setRowHeight", args = new object[] { rowPos, height } });
        return this;
    }

    public async Task FlushAsync()
    {
        if (flushed || operations.Count == 0) return;
        flushed = true;
        await univerJS.ResolveActionAsync("batchSheetOperations", instanceId, sheetId, operations.ToArray());
    }

    public async ValueTask DisposeAsync() => await FlushAsync();
}
