namespace UniverBlazored.Spreadsheets.Data;

public readonly record struct UniverSpreadsheetListenerData
{
    public int row { get; init; }
    public int col { get; init; }
    public string unitId { get; init; }
    public string subUnitId { get; init; }
    public string instanceId { get; init; }
}
