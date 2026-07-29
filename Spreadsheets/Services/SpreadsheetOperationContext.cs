namespace UniverBlazored.Spreadsheets.Services;

public enum OperationKind
{
    Sheet,
    Workbook,
    Ui,
    Structural
}

public readonly record struct SpreadsheetOperationContext(
    string InstanceId,
    string? SheetId,
    OperationKind Kind,
    CancellationToken CancellationToken = default,
    long OperationId = 0,
    string? IdempotencyKey = null);