namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// "config" object used in the FacadeAPI to construct an IconSet Conditional Format
/// </summary>
public struct UIconSetCond
{
    /// <summary>
    /// True to show the value in the cell
    /// </summary>
    public bool isShowValue { get; set; } = true;

    /// <summary>
    /// Configuration objects for each icon
    /// </summary>
    public UIconSetConfig[] iconConfigs { get; set; } = [];

    /// <summary>
    /// "config" object used in the FacadeAPI to construct an IconSet Conditional Format
    /// </summary>
    public UIconSetCond() { }

    /// <summary>
    /// "config" object used in the FacadeAPI to construct an IconSet Conditional Format
    /// </summary>
    /// <param name="isShowValue">True to show the value in the cell</param>
    /// <param name="iconConfigs">Configuration objects for each icon</param>
    public UIconSetCond(bool isShowValue, UIconSetConfig[] iconConfigs)
    {
        this.iconConfigs = iconConfigs;
        this.isShowValue = isShowValue;
    }
}