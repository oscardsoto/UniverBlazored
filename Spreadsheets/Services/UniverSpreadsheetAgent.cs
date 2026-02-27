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

    /// <summary>
    /// All data operations
    /// </summary>
    public DataCommands Data { get; private set; }

    /// <summary>
    /// All styles operations
    /// </summary>
    public StyleCommands Styles { get; private set; }

    /// <summary>
    /// All condiitonal format operations
    /// </summary>
    public ConditionalFormatCommands ConditionalFormats { get; private set; }

    /// <summary>
    /// All image operations
    /// </summary>
    public ImageCommands Images { get; private set; }

    /// <summary>
    /// All rows & columns operations
    /// </summary>
    public RowColumnsCommands RowColumns { get; private set; }

    /// <summary>
    /// All comment operations
    /// </summary>
    public CommentCommands Comments { get; private set; }

    /// <summary>
    /// All range operations
    /// </summary>
    public RangeCommands Ranges { get; private set; }

    /// <summary>
    /// Univer's agent. Enables operations inside Blazor
    /// </summary>
    /// <param name="univerJS">Univer's interop to get acces to Univer</param>
    public UniverSpreadsheetAgent(IUniverJsInterop univerJS)
    {
        this.univerJS       = univerJS;
        Data                = new(new USpreadsheetSnapshot(), univerJS);
        Styles              = new(new USpreadsheetSnapshot(), univerJS);
        ConditionalFormats  = new(new USpreadsheetSnapshot(), univerJS);
        Images              = new(new USpreadsheetSnapshot(), univerJS);
        RowColumns          = new(new USpreadsheetSnapshot(), univerJS);
        Comments            = new(new USpreadsheetSnapshot(), univerJS);
        Ranges              = new(new USpreadsheetSnapshot(), univerJS);
    }

    /// <summary>
    /// Sets the active sheet to the component
    /// </summary>
    /// <param name="idSheet">Id of the sheet to put the changes.</param>
    public async Task SetActiveSheet(string idSheet)
    {
        var queue = new UniverQueue(univerJS);
        await queue.SetAction("getActiveWorkbook").SetAction("setActiveSheet", idSheet).ResolveQueueAsync();
    }

    /// <summary>
    /// Sets the range to work in the active sheet on the component
    /// </summary>
    /// <param name="range"></param>
    public async Task SetActiveRange(URange range)
    {
        var queue = new UniverQueue(univerJS);
        await queue.SetAction("getActiveWorkbook").SetAction("getActiveSheet").SetAction("getRange", range).SetAction("activate").ResolveQueueAsync();
    }

    /// <summary>
    /// Return all sheets information
    /// </summary>
    /// <returns></returns>
    public async Task<USheetInfo[]> GetSheetsInfo() => await univerJS.ResolveActionAsync<USheetInfo[]>("getSheetsInfo");

    /// <summary>
    /// Adds a new Worksheet in the active workbook
    /// </summary>
    /// <param name="sheetName">Name of the new worksheet</param>
    /// <param name="cols">Amount of columns that the worksheet will have</param>
    /// <param name="rows">Amount of rows that the worksheet will have</param>
    /// <param name="hexColorTab">Color of its tab (in hexadecimal)</param>
    /// <returns></returns>
    public async Task<USheetInfo> AddNewSheet(string sheetName, int rows, int cols, string hexColorTab = null)
    {
        var queue = new UniverQueue(univerJS);
        var id = await queue.SetAction("getActiveWorkbook").SetAction("create", sheetName, rows + 1, cols + 1).SetAction("setTabColor", hexColorTab).SetAction("getSheetId").ResolveQueueAsync<string>();
        await SetActiveSheet(id);
        return new()
        {
            id = id,
            name = sheetName,
            maxUsed = new(0, rows - 1, 0, cols - 1),
            tabColor = hexColorTab
        };
    }

    /// <summary>
    /// Removes the specified sheet
    /// </summary>
    /// <param name="idSheet">Id for the sheet to delete. If null or empty, deletes the active sheet</param>
    /// <returns></returns>
    public async Task DeleteSheet(string idSheet = null)
    {
        var queue = new UniverQueue(univerJS);
        if (string.IsNullOrEmpty(idSheet))
            idSheet = await queue.SetAction("getActiveWorkbook")
                                 .SetAction("getActiveSheet")
                                 .SetAction("getId").ResolveQueueAsync<string>();

        await queue.SetAction("getActiveWorkbook").SetAction("deleteSheet", idSheet).ResolveQueueAsync();
    }

    /// <summary>
    /// Hide the active sheet
    /// </summary>
    /// <returns></returns>
    public async Task HideSheet(string idSheet = null)
    {
        var queue = new UniverQueue(univerJS);
        if (string.IsNullOrEmpty(idSheet))
            idSheet = await queue.SetAction("getActiveWorkbook")
                                 .SetAction("getActiveSheet")
                                 .SetAction("getId").ResolveQueueAsync<string>();

        await queue.SetAction("getActiveWorkbook").SetAction("getSheetBySheetId", idSheet).SetAction("hideSheet").ResolveQueueAsync();
    }

    /// <summary>
    /// Unhide the active sheet
    /// </summary>
    /// <returns></returns>
    public async Task ShowSheet(string idSheet = null)
    {
        var queue = new UniverQueue(univerJS);
        if (string.IsNullOrEmpty(idSheet))
            idSheet = await queue.SetAction("getActiveWorkbook")
                                 .SetAction("getActiveSheet")
                                 .SetAction("getId").ResolveQueueAsync<string>();

        await queue.SetAction("getActiveWorkbook").SetAction("getSheetBySheetId", idSheet).SetAction("showSheet").ResolveQueueAsync();
    }
}