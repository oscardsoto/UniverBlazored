namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// CFRuleType, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L55)
/// </summary>
public enum ECFRuleType
{
    /// <summary>
    /// To apply style that match the conditional format
    /// </summary>
    highlightCell,

    /// <summary>
    /// To apply dataBar
    /// </summary>
    dataBar,

    /// <summary>
    /// To apply colorScale
    /// </summary>
    colorScale,

    /// <summary>
    /// To apply iconSet
    /// </summary>
    iconSet
}