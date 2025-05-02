namespace UniverBlazored.Spreadsheets.Data.Workbook;

/// <summary>
/// IFreeze, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IFreeze)
/// </summary>
public struct UFreeze
{
    /// <summary>
    /// scrollable start column(viewMain start column)
    /// </summary>
    public int startColumn { get; set; }

    /// <summary>
    /// scrollable start row(viewMain start row)
    /// </summary>
    public int startRow { get; set; }

    /// <summary>
    /// count of fixed cols
    /// </summary>
    public int xSplit { get; set; }

    /// <summary>
    /// count of fixed rows
    /// </summary>
    public int ySplit { get; set; }
}