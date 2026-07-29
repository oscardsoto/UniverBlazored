namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Worksheet permission configuration that mirrors Univer applyConfig contract.
/// </summary>
public class UWorksheetPermissionConfig
{
    /// <summary>
    /// Optional worksheet mode.
    /// </summary>
    public EWorksheetMode? Mode { get; set; }

    /// <summary>
    /// Optional permission point patch keyed by worksheet permission id.
    /// Example: WorksheetEdit, WorksheetView, WorksheetInsertRow.
    /// </summary>
    public Dictionary<string, bool>? Points { get; set; }

    /// <summary>
    /// Optional batch range protections by reference strings.
    /// </summary>
    public UWorksheetPermissionRangeProtectionConfig[]? RangeProtections { get; set; }

    /// <summary>
    /// Worksheet permission configuration that mirrors Univer applyConfig contract.
    /// </summary>
    public UWorksheetPermissionConfig()
    {
        // Initialize the Points dictionary to avoid null reference issues
        Points = new Dictionary<string, bool>();
        RangeProtections = Array.Empty<UWorksheetPermissionRangeProtectionConfig>();
    }
}
