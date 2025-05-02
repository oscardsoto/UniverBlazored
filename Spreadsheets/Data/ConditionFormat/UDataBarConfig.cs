namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// DataBar configuration Object for the conditional format
/// </summary>
public struct UDataBarConfig
{
    /// <summary>
    /// True for a gradient data bar
    /// </summary>
    public bool isGradient { get; set; }

    /// <summary>
    /// True for show value
    /// </summary>
    public bool isShowValue { get; set; }

    /// <summary>
    /// Min value for the data bar
    /// </summary>
    /// <value></value>
    public UValueConfig min { get; set; }

    /// <summary>
    /// Max value for the data bar
    /// </summary>
    /// <value></value>
    public UValueConfig max { get; set; }

    /// <summary>
    /// Negative color (Hexadecimal)
    /// </summary>
    public string nativeColor { get; set; }

    /// <summary>
    /// Positive color (Hexadecimal)
    /// </summary>
    public string positiveColor { get; set; }
}

