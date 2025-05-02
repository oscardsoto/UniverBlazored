namespace UniverBlazored.Spreadsheets.Data.Styles;

/// <summary>
/// Combines all Fonts characteristics to apply in range
/// </summary>
public struct UFontProperties
{
    /// <summary>
    /// Combines all Fonts characteristics to apply in range
    /// </summary>
    public UFontProperties() { }

    /// <summary>
    /// Font Color
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Background Cell's color
    /// </summary>
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Font family
    /// </summary>
    public string? Family { get; set; }

    /// <summary>
    /// Font Line (line-through)
    /// </summary>
    public bool? Strikethrough { get; set; }

    /// <summary>
    /// Font Line (line-through)
    /// </summary>
    public bool? Underline { get; set; }

    /// <summary>
    /// Font size
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// Text Rotation (angle in degrees)
    /// </summary>
    /// <value></value>
    public int? TextRotation { get; set; }

    /// <summary>
    /// Font Style
    /// </summary>
    public bool? Italic { get; set; }

    /// <summary>
    /// Font Weight
    /// </summary>
    public bool? Bold { get; set; }

    /// <summary>
    /// Font Horizontal Align
    /// </summary>
    public FHorizontalAligment? HorizontalAlign { get; set; }

    /// <summary>
    /// Font Vertical Align
    /// </summary>
    public FVerticalAligment? VerticalAlign { get; set; }

    /// <summary>
    /// Font format for number/Date
    /// </summary>
    public string? NumberFormat { get; set; }

    /// <summary>
    /// Font Wrap for the cells
    /// </summary>
    /// <value></value>
    public bool? IsWrap { get; set; }

    /// <summary>
    /// Font Wrap Strategy
    /// </summary>
    /// <value></value>
    public EWrapStrategy? WrapStrategy { get; set; }
}

/// <summary>
/// FVerticalAligment, from Univer (See doc: https://reference.univer.ai/en-US/globals#fverticalalignment)
/// </summary>
public enum FVerticalAligment
{
    /// <summary>
    /// Top align
    /// </summary>
    TOP,

    /// <summary>
    /// Middle align
    /// </summary>
    MIDDLE,

    /// <summary>
    /// Bottom align
    /// </summary>
    BOTTOM
}

/// <summary>
/// FHorizontalAligment, from Univer (See doc: https://reference.univer.ai/en-US/globals#fhorizontalalignment)
/// </summary>
public enum FHorizontalAligment
{
    /// <summary>
    /// Left align
    /// </summary>
    LEFT,

    /// <summary>
    /// Center align
    /// </summary>
    CENTER,

    /// <summary>
    /// Normal align
    /// </summary>
    NORMAL
}

/// <summary>
/// Merge startegy option for the style
/// </summary>
public enum MergeStrategy
{
    /// <summary>
    /// Just merge
    /// </summary>
    ALL,

    /// <summary>
    /// Horizontal Strategy
    /// </summary>
    HORIZONTAL,

    /// <summary>
    /// Vertical Strategy
    /// </summary>
    VERTICAL
}