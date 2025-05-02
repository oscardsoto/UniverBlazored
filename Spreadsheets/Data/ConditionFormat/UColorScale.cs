namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IColorScale, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L91)
/// </summary>
public struct UColorScale
{
    /// <summary>
    /// ECFRuleType value
    /// </summary>
    /// <value></value>
    public string type { get; set; } = "colorScale";

    /// <summary>
    /// Configuration objects for each situation
    /// </summary>
    public UColorScaleConfig[] config { get; set; }

    /// <summary>
    /// IColorScale, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L91)
    /// </summary>
    public UColorScale() { config = []; }
}

/// <summary>
/// Color Scale configuration Object for the conditional format
/// </summary>
public struct UColorScaleConfig
{
    /// <summary>
    /// Color index
    /// </summary>
    public int index { get; set; }

    /// <summary>
    /// Color (in hexadecimal)
    /// </summary>
    public string color { get; set; }

    /// <summary>
    /// Value for the Color scale to apply
    /// </summary>
    public UValueConfig value { get; set; }
}
