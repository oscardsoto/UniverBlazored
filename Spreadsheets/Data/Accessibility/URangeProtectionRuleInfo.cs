using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Serializable view for a range protection rule in a worksheet.
/// </summary>
public class URangeProtectionRuleInfo
{
    /// <summary>
    /// Rule identifier in Univer.
    /// </summary>
    public string RuleId { get; set; } = string.Empty;

    /// <summary>
    /// Protected ranges in this rule.
    /// </summary>
    public URange[] Ranges { get; set; } = [];

    /// <summary>
    /// Rule options.
    /// </summary>
    public URangeProtectionOptions? Options { get; set; }
}
