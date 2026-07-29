using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Rows and Columns
/// </summary>
public class RowColumnsCommands : USpreadsheetCommandBase<RowColumnsCommands>
{
    /// <summary>
    /// Commands focused on Sheet Rows and Columns
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <param name="snapshot"></param>
    public RowColumnsCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Freeze the amount of rows and columns (beggining in A1)
    /// </summary>
    /// <param name="rows">Number of rows to freeze</param>
    /// <param name="cols">Number of cols to freeze</param>
    /// <returns></returns>
    public async Task SetFreeze(int rows, int cols)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("setFrozenRows", rows).SetAction("setFrozenColumns", cols).ResolveQueueAsync();
    }

    /// <summary>
    /// Cancels the frozen state of the current sheet.
    /// </summary>
    /// <returns></returns>
    public async Task CancelFreeze()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("cancelFreeze").ResolveQueueAsync();
    }

    /// <summary>
    /// Returns the frozen state (rows and cols) of the page
    /// </summary>
    /// <returns>Frozen state object</returns>
    public async Task<UFreeze> GetFreeze()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getFreeze").ResolveQueueAsync<UFreeze>();
    }

    // Default:
    //      RowHeight   -> Univer/24 (px), ClosedXML/15 (pt)
    //      ColumnWidth -> Univer/88 (px), ClosedXML/8.43 (NoC)
    // We gotta use "Rule of 3" in order to convert the points

    public async Task<double[]> GetRowsHeights(params int[] rowPos)
    {
        return await ExecuteAtomically(async inner =>
        {
            var results = new List<double>();
            foreach (int pos in rowPos)
            {
                var q = new UniverQueue(inner, Snapshot.ToContext());
                UseSheet(q);
                results.Add(await q.SetAction("getRowHeight", pos).ResolveQueueAsync<double>());
            }
            return results.ToArray();
        });
    }

    public async Task<double[]> GetColumnWidth(params int[] colPos)
    {
        return await ExecuteAtomically(async inner =>
        {
            var results = new List<double>();
            foreach (int pos in colPos)
            {
                var q = new UniverQueue(inner, Snapshot.ToContext());
                UseSheet(q);
                results.Add(await q.SetAction("getColumnWidth", pos).ResolveQueueAsync<double>());
            }
            return results.ToArray();
        });
    }

    /// <summary>
    /// Sets the column's width at the position 
    /// </summary>
    /// <param name="colPos">Column position</param>
    /// <param name="width">Width of that column</param>
    /// <returns></returns>
    public async Task SetColumnWidth(int colPos, double width)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("setColumnWidth", colPos, width).ResolveQueueAsync();
    }

    /// <summary>
    /// Sets the row's height at the position
    /// </summary>
    /// <param name="rowPos">Row position</param>
    /// <param name="height">height of that row</param>
    /// <returns></returns>
    public async Task SetRowHeight(int rowPos, double height)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("setRowHeight", rowPos, height).ResolveQueueAsync();
    }

    /// <summary>
    /// Inserts one or more consecutive blank columns in a sheet starting at the specified location.
    /// </summary>
    /// <param name="colPos">The index indicating where to insert a column, starting at 0 for the first column</param>
    /// <param name="colCount">The number of columns to insert</param>
    /// <returns></returns>
    public async Task InsertColumns(int colPos, int colCount = 1)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("insertColumns", colPos, colCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Deletes a number of columns starting at the given column position.
    /// </summary>
    /// <param name="colPos">The position of the first column to delete, starting at 0 for the first column</param>
    /// <param name="colCount">The number of columns to delete</param>
    /// <returns></returns>
    public async Task DeleteColumns(int colPos, int colCount = 1)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("deleteColumns", colPos, colCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Inserts one or more consecutive blank rows in a sheet starting at the specified location.
    /// </summary>
    /// <param name="rowPos">The index indicating where to insert a row, starting at 0 for the first row.</param>
    /// <param name="rowCount">The number of rows to insert.</param>
    /// <returns></returns>
    public async Task InsertRows(int rowPos, int rowCount = 1)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("insertRows", rowPos, rowCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Deletes a number of rows starting at the given row position.
    /// </summary>
    /// <param name="rowPos">The position of the first row to delete, starting at 0 for the first row.</param>
    /// <param name="rowCount">The number of rows to delete.</param>
    /// <returns></returns>
    public async Task DeleteRows(int rowPos, int rowCount = 1)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("deleteRows", rowPos, rowCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Hide an amount of rows in the active sheet
    /// </summary>
    /// <param name="rowPos">Row's position to count the rows to hide</param>
    /// <param name="rowCount">Number of rows to hide from the position</param>
    /// <returns></returns>
    public async Task HideRows(int rowPos, int rowCount)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("hideRows", rowPos, rowCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Hide an amount of columns in the active sheet
    /// </summary>
    /// <param name="colPos">Columns's position to count the columns to hide</param>
    /// <param name="colCount">Number of columns to hide from the position</param>
    /// <returns></returns>
    public async Task HideColumns(int colPos, int colCount)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("hideColumns", colPos, colCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Unhide an amount of rows in the active sheet
    /// </summary>
    /// <param name="rowPos">Row's position to count the rows to hide</param>
    /// <param name="rowCount">Number of rows to hide from the position</param>
    /// <returns></returns>
    public async Task UnhideRows(int rowPos, int rowCount)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("showRows", rowPos, rowCount).ResolveQueueAsync();
    }

    /// <summary>
    /// Unhide an amount of columns in the active sheet
    /// </summary>
    /// <param name="colPos">Columns's position to count the columns to hide</param>
    /// <param name="colCount">Number of columns to hide from the position</param>
    /// <returns></returns>
    public async Task UnhideColumns(int colPos, int colCount)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("showColumns", colPos, colCount).ResolveQueueAsync();
    }

    /// <inheritdoc/>
    protected override RowColumnsCommands Clone(USpreadsheetSnapshot context)
        => new RowColumnsCommands(context, UniverJS);
}