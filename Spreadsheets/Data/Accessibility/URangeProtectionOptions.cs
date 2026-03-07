namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Options for protected ranges.
/// </summary>
public class URangeProtectionOptions
{
    /// <summary>
    /// Allows editing in protected range when true.
    /// </summary>
    public bool? AllowEdit { get; set; }

    /// <summary>
    /// Allows viewing protected range by other users when true.
    /// </summary>
    public bool? AllowViewByOthers { get; set; }

    /// <summary>
    /// Display name for the protection rule.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Allowed user ids.
    /// </summary>
    public string[]? AllowedUsers { get; set; }
}
