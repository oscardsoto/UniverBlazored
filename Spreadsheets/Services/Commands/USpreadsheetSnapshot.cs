using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

public sealed class USpreadsheetSnapshot
{
    public string? InstanceId { get; }
    public USheetInfo? SheetSelected { get; }
    public URange? RangeSelected { get; }

    public USpreadsheetSnapshot(
        string? instanceId = null,
        USheetInfo? sheetSelected = null,
        URange? rangeSelected = null)
    {
        InstanceId = instanceId;
        SheetSelected = sheetSelected;
        RangeSelected = rangeSelected;
    }

    public USpreadsheetSnapshot WithSheet(USheetInfo? sheet) => new(InstanceId, sheet, RangeSelected);

    public USpreadsheetSnapshot WithRange(URange? range) => new(InstanceId, SheetSelected, range);

    public USpreadsheetSnapshot WithInstanceId(string instanceId) => new(instanceId, SheetSelected, RangeSelected);

    public SpreadsheetOperationContext ToContext(OperationKind kind = OperationKind.Sheet) => new(
        InstanceId ?? throw new InvalidOperationException("InstanceId is required to build operation context."),
        SheetSelected?.id,
        kind);
}
