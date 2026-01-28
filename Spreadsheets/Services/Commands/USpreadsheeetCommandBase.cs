using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Base class for Spreadsheet commands for FacadeAPI in Univer
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public abstract class USpreadsheetCommandBase<TCommand> where TCommand : USpreadsheetCommandBase<TCommand>
{
    /// <summary>
    /// Context operation
    /// </summary>
    protected readonly USpreadsheetSnapshot Snapshot;

    /// <summary>
    /// Js interoperability with Univer
    /// </summary>
    protected readonly IUniverJsInterop UniverJS;

    /// <summary>
    /// Base class for Spreadsheet commands for FacadeAPI in Univer
    /// </summary>
    /// <param name="snapshot"></param>
    /// <param name="univerJs"></param>
    protected USpreadsheetCommandBase(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs)
    {
        Snapshot = snapshot ?? new USpreadsheetSnapshot();
        UniverJS = univerJs;
    }

    /// <summary>
    /// Clone this command object with the specified context
    /// </summary>
    /// <param name="context">Snapshot for the command</param>
    /// <returns></returns>
    protected abstract TCommand Clone(USpreadsheetSnapshot context);

    /// <summary>
    /// Select the sheet to execute the command
    /// </summary>
    /// <param name="sheet">Sheet object</param>
    /// <returns></returns>
    public TCommand OnSheet(USheetInfo? sheet = null)
        => Clone(Snapshot.WithSheet(sheet));

    /// <summary>
    /// Select the range to execute the command
    /// </summary>
    /// <param name="range">Range object</param>
    /// <returns></returns>
    public TCommand OnRange(URange? range = null)
        => Clone(Snapshot.WithRange(range));

    /// <summary>
    /// Init the queue in the active/selected sheet
    /// </summary>
    /// <param name="queue">The command queue</param>
    protected void UseSheet(UniverQueue queue)
    {
        queue.SetAction("getActiveWorkbook");
        if (Snapshot.SheetSelected == null)
        {
            queue.SetAction("getActiveSheet");
            return;
        }
        queue.SetAction("getSheetBySheetId", Snapshot.SheetSelected.Value.id);
    }

    /// <summary>
    /// Init the queue in the active/selected range in the active/selected sheet 
    /// </summary>
    /// <param name="queue">The command queue</param>
    protected void UseRange(UniverQueue queue)
    {
        UseSheet(queue);
        if (Snapshot.RangeSelected == null)
        {
            queue.SetAction("getActiveRange");
            return;
        }
        queue.SetAction("getRange", Snapshot.RangeSelected.Value);
    }
}