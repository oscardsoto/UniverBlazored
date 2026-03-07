namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Batch range-protection item used by worksheet permission applyConfig.
/// </summary>
public class UWorksheetPermissionRangeProtectionConfig
{
    /// <summary>
    /// A1 notation references (for example: Sheet1!A1:B10).
    /// </summary>
    public string[] RangeRefs { get; set; } = [];

    /// <summary>
    /// Optional range protection options.
    /// </summary>
    public URangeProtectionOptions? Options { get; set; }
}
