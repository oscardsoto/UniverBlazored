namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// Conditional Format Style for the builder (See doc from univer for each function in: https://reference.univer.ai/en-US/classes/FConditionalFormattingBuilder#setaverage)
/// </summary>
public struct UConditionFormatStyle
{
    /// <summary>
    /// Set average rule (null to ignore)
    /// </summary>
    public UAverageHighlightCell? Average { get; set; }

    /// <summary>
    /// Sets the background color (null to ignore)
    /// </summary>
    public string Background { get; set; }

    /// <summary>
    /// Set Bold (null to ignore)
    /// </summary>
    public bool? IsBold { get; set; }

    /// <summary>
    /// Set colorScale rule (null to ingore)
    /// </summary>
    public UColorScale? ColorScale { get; set; }

    /// <summary>
    /// Set dataBar rule (null to ignore)
    /// </summary>
    public UDataBarConfig? DataBar { get; set; }

    /// <summary>
    /// Set duplicateValues rule (false to ignore)
    /// </summary>
    public bool DuplicateValues { get; set; }

    /// <summary>
    /// Sets the font color (null to ignore)
    /// </summary>
    public string FontColor { get; set; }

    /// <summary>
    /// Set iconSet rule (null to ignore)
    /// </summary>
    public (bool isShowValue, UIconSetConfig[] config)? IconSet { get; set; }

    /// <summary>
    /// Set the text to italic (null to ignore)
    /// </summary>
    public bool? Italic { get; set; }

    /// <summary>
    /// Set rank rule (null to ignore)
    /// </summary>
    public (bool isBottom, bool isPercent, double value)? Rank { get; set; }

    /// <summary>
    /// Set the strikethrough (null to ignore)
    /// </summary>
    public bool? Strikethrough { get; set; }

    /// <summary>
    /// Set the underscore (null to ignore)
    /// </summary>
    public bool? Underline { get; set; }

    /// <summary>
    /// Set uniqueValues rule (false to ignore)
    /// </summary>
    public bool UniqueValues { get; set; }
}
