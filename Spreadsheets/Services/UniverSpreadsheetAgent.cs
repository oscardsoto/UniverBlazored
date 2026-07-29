using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;
using UniverBlazored.Spreadsheets.Services.Commands;

namespace UniverBlazored.Spreadsheets.Services;

/// <summary>
/// Univer's agent. Enables operations inside Blazor
/// </summary>
public class UniverSpreadsheetAgent
{
    private readonly IUniverJsInterop univerJS;

    public sealed class SheetScope
    {
        private readonly IUniverJsInterop univerJS;
        private readonly USheetInfo sheet;
        private readonly string instanceId;

        internal SheetScope(IUniverJsInterop univerJS, USheetInfo sheet, string instanceId)
        {
            this.univerJS = univerJS;
            this.sheet = sheet;
            this.instanceId = instanceId;
        }

        public DataCommands Data() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public StyleCommands Styles() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public ConditionalFormatCommands ConditionalFormats() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public ImageCommands Images() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public RowColumnsCommands RowColumns() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public CommentCommands Comments() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public RangeCommands Ranges() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public AccessibilityCommands Accessibility() => new(new USpreadsheetSnapshot(instanceId, sheet), univerJS);
        public BatchScope Batch() => new(univerJS, instanceId, sheet.id);
    }

    private readonly string instanceId;

    public DataCommands Data() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public StyleCommands Styles() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public ConditionalFormatCommands ConditionalFormats() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public ImageCommands Images() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public RowColumnsCommands RowColumns() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public CommentCommands Comments() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public RangeCommands Ranges() => new(new USpreadsheetSnapshot(instanceId), univerJS);
    public AccessibilityCommands Accessibility() => new(new USpreadsheetSnapshot(instanceId), univerJS);

    /// <summary>
    /// Creates a scope for operating on a specific sheet
    /// </summary>
    /// <param name="sheet">Sheet object</param>
    /// <returns></returns>
    public SheetScope ForSheet(USheetInfo sheet) => new(univerJS, sheet, instanceId);

    /// <summary>
    /// Creates a scope for operating on a specific sheet
    /// </summary>
    /// <param name="sheetId">Sheet id</param>
    /// <returns></returns>
    public SheetScope ForSheetId(string sheetId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sheetId);
        return new(univerJS, new USheetInfo { id = sheetId }, instanceId);
    }

    /// <summary>
    /// Univer's agent. Enables operations inside Blazor
    /// </summary>
    /// <param name="univerJS">Univer's interop to get acces to Univer</param>
    public UniverSpreadsheetAgent(IUniverJsInterop univerJS, string instanceId)
    {
        this.univerJS = univerJS;
        this.instanceId = instanceId;
    }

    private UniverQueue CreateStructuralQueue() => new(univerJS, new SpreadsheetOperationContext(instanceId, null, OperationKind.Structural));

    private UniverQueue CreateUiQueue() => new(univerJS, new SpreadsheetOperationContext(instanceId, null, OperationKind.Ui));

    private UniverQueue CreateWorkbookQueue() => new(univerJS, new SpreadsheetOperationContext(instanceId, null, OperationKind.Workbook));

    public async Task ToggleDarkMode(bool darkMode)
    {
        var queue = CreateStructuralQueue();
        await queue.SetAction("toggleDarkMode", darkMode).ResolveQueueAsync();
    }

    public async Task SetActiveSheet(string idSheet)
    {
        var queue = CreateUiQueue();
        await queue.SetAction("getActiveWorkbook").SetAction("setActiveSheet", idSheet).ResolveQueueAsync();
    }

    public async Task SetActiveRange(URange range)
    {
        var queue = CreateUiQueue();
        await queue.SetAction("getActiveWorkbook").SetAction("getActiveSheet").SetAction("getRange", range).SetAction("activate").ResolveQueueAsync();
    }

    public async Task<USheetInfo[]> GetSheetsInfo()
    {
        return await univerJS.ResolveActionAsync<USheetInfo[]>("getSheetsInfo", instanceId);
    }

    public async Task<USheetInfo> AddNewSheet(string sheetName, int rows, int cols, string hexColorTab = null)
    {
        var queue = CreateWorkbookQueue();
        var id = await queue.SetAction("getActiveWorkbook").SetAction("create", sheetName, rows + 1, cols + 1).SetAction("setTabColor", hexColorTab).SetAction("getSheetId").ResolveQueueAsync<string>();

        return new() { id = id, name = sheetName, maxUsed = new(0, rows - 1, 0, cols - 1), tabColor = hexColorTab };
    }

    public async Task DeleteSheet(string idSheet)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(idSheet);
        var queue = CreateWorkbookQueue();
        await queue.SetAction("getActiveWorkbook").SetAction("deleteSheet", idSheet).ResolveQueueAsync();
    }

    public async Task HideSheet(string idSheet)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(idSheet);
        var queue = CreateWorkbookQueue();
        await queue.SetAction("getActiveWorkbook").SetAction("getSheetBySheetId", idSheet).SetAction("hideSheet").ResolveQueueAsync();
    }

    public async Task ShowSheet(string idSheet)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(idSheet);
        var queue = CreateWorkbookQueue();
        await queue.SetAction("getActiveWorkbook").SetAction("getSheetBySheetId", idSheet).SetAction("showSheet").ResolveQueueAsync();
    }

    public async Task AddNewFonts(params UniverFont[] fonts)
    {
        var queue = CreateStructuralQueue();
        await queue.SetAction("addFonts", [fonts]).ResolveQueueAsync();
    }
}
