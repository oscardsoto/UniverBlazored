namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// Conditional format configuration for Rank type, from Univer
/// </summary>
public struct URankConfig
{
    /// <summary>
    /// True if the condition will begin at the bottom
    /// </summary>
    public bool isBottom { get; set; }

    /// <summary>
    /// True if the value is a percentage
    /// </summary>
    public bool isPercent { get; set; }

    /// <summary>
    /// Value to evaluate
    /// </summary>
    /// <value></value>
    public double value { get; set; }

    /// <summary>
    /// Conditional format configuration for Rank type, from Univer
    /// </summary>
    public URankConfig() { }

    /// <summary>
    /// Conditional format configuration for Rank type, from Univer
    /// </summary>
    /// <param name="isBottom">True if the condition will begin at the bottom</param>
    /// <param name="isPercent">True if the value is a percentage</param>
    /// <param name="value">Value to evaluate</param>
    public URankConfig(bool isBottom, bool isPercent, double value)
    {
        this.isBottom = isBottom;
        this.isPercent = isPercent;
        this.value = value;
    }
}