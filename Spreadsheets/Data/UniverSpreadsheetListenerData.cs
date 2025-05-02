namespace UniverBlazored.Spreadsheets.Data;

/// <summary>
/// Data where the listener gets the information
/// </summary>
public struct UniverSpreadsheetListenerData
{
    /// <summary>
    /// Selected Row
    /// </summary>
    public int row { get; set; }

    /// <summary>
    /// Selected Column
    /// </summary>
    public int col { get; set; }

    /// <summary>
    /// Workbook where the listener is stored
    /// </summary>
    public string unitId { get; set; }

    /// <summary>
    /// Worksheet where the listener is located
    /// </summary>
    public string subUnitId { get; set; }
}