namespace UniverBlazored.Spreadsheets.Data.Styles;

/// <summary>
/// Cell value format config
/// </summary>
public struct UFormatStyle
{
    /// <summary>
    /// Format Pattern Value
    /// </summary>
    public string pattern { get; set; }

    /// <summary>
    /// Return true if the pattern is numeric
    /// </summary>
    /// <returns></returns>
    public bool IsForNumber() => pattern.Contains("#") || pattern.Contains("0") || pattern.Contains("?");

    /// <summary>
    /// Return true if the pattern is for dates
    /// </summary>
    /// <returns></returns>
    public bool IsForDate() => pattern.Contains("M") || pattern.Contains("D") || pattern.Contains("A") || pattern.Contains("H") || pattern.Contains("M") || pattern.Contains("S");
}