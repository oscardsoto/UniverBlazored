using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Represents an immutable execution context for a spreadsheet command operation.
/// </summary>
public sealed class USpreadsheetSnapshot
{
    /// <summary>
    /// Target sheet for the operation (if any)
    /// </summary>
    public USheetInfo? SheetSelected { get; }

    /// <summary>
    /// Target range for the operation (if any)
    /// </summary>
    public URange? RangeSelected { get; }

    /// <summary>
    /// Represents an immutable execution context for a spreadsheet command operation.
    /// </summary>
    /// <param name="sheetSelected">Target sheet for the operation (if any)</param>
    /// <param name="rangeSelected">Target range for the operation (if any)</param>
    public USpreadsheetSnapshot(USheetInfo? sheetSelected = null, URange? rangeSelected = null)
    {
        SheetSelected = sheetSelected;
        RangeSelected = rangeSelected;
    }

    /// <summary>
    /// Creates a new <see cref="USpreadsheetSnapshot"/> with the specified sheet, preserving the current range.
    /// </summary>
    /// <param name="sheet">Target sheet</param>
    /// <returns></returns>
    public USpreadsheetSnapshot WithSheet(USheetInfo? sheet) => new(sheet, RangeSelected);

    /// <summary>
    /// Creates a new <see cref="USpreadsheetSnapshot"/> with the specified range, preserving the current sheet.
    /// </summary>
    /// <param name="range">Target range</param>
    /// <returns></returns>
    public USpreadsheetSnapshot WithRange(URange? range) => new(SheetSelected, range);
}