namespace UniverBlazored.Spreadsheets.Data.Styles;

/// <summary>
/// IStyleBase, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-style-data.ts#L134)
/// </summary>
public interface IStyleBase
{
    /// <summary>
    /// fontFamily
    /// </summary>
    string ff { get; set; }

    /// <summary>
    /// fontSize (pt)
    /// </summary>
    int fs { get; set; }

    /// <summary>
    /// italic 
    /// 0: false 
    /// 1: true
    /// </summary>
    int it { get; set; }

    /// <summary>
    /// bold 
    /// 0: false 
    /// 1: true
    /// </summary>
    int bl { get; set; }

    /// <summary>
    /// underline
    /// </summary>
    UTextDecoration ul { get; set; }

    /// <summary>
    /// bottomBorderLine
    /// </summary>
    UTextDecoration? bbl { get; set; }

    /// <summary>
    /// strikethrough
    /// </summary>
    UTextDecoration st { get; set; }

    /// <summary>
    /// overline
    /// </summary>
    UTextDecoration ol { get; set; }

    /// <summary>
    /// background
    /// </summary>
    UColorStyle? bg { get; set; }

    /// <summary>
    /// border
    /// </summary>
    UBorderData? bd { get; set; }

    /// <summary>
    /// foreground
    /// </summary>
    UColorStyle? cl { get; set; }

    /// <summary>
    /// (Subscript 下标 / Superscript 上标 Text)
    /// </summary>
    object? va { get; set; }

    /// <summary>
    /// Number format pattern
    /// </summary>
    UFormatStyle? n { get; set; }
}