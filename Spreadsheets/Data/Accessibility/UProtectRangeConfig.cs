using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Configuration for creating one range protection rule.
/// </summary>
public class UProtectRangeConfig
{
    /// <summary>
    /// Ranges to protect.
    /// </summary>
    public URange[] Ranges { get; set; } = [];

    /// <summary>
    /// Optional protection options.
    /// </summary>
    public URangeProtectionOptions? Options { get; set; }
}
