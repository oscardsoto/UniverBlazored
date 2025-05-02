using UniverBlazored.Spreadsheets.Data.Styles;

namespace UniverBlazored.Spreadsheets.Data.RichText;

/// <summary>
/// ITextStyle, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts#L704)
/// </summary>
public struct UTextStyle : IStyleBase
{
    /// <summary>
    /// Spacing
    /// </summary>
    public int? sc { get; set; }

    /// <summary>
    /// Position
    /// </summary>
    public int? pos { get; set; }

    /// <summary>
    /// Scale
    /// </summary>
    public int? sa { get; set; }

    /// <summary>
    /// fontFamily
    /// </summary>
    public string ff { get; set; }

    /// <summary>
    /// fontSize (pt)
    /// </summary>
    public int fs { get; set; }

    /// <summary>
    /// italic 
    /// 0: false 
    /// 1: true
    /// </summary>
    public int it { get; set; }

    /// <summary>
    /// bold 
    /// 0: false 
    /// 1: true
    /// </summary>
    public int bl { get; set; }

    /// <summary>
    /// underline
    /// </summary>
    public UTextDecoration ul { get; set; }

    /// <summary>
    /// bottomBorderLine
    /// </summary>
    public UTextDecoration? bbl { get; set; }

    /// <summary>
    /// strikethrough
    /// </summary>
    public UTextDecoration st { get; set; }

    /// <summary>
    /// overline
    /// </summary>
    public UTextDecoration ol { get; set; }

    /// <summary>
    /// background
    /// </summary>
    public UColorStyle? bg { get; set; }

    /// <summary>
    /// border
    /// </summary>

    public UBorderData? bd { get; set; }

    /// <summary>
    /// foreground
    /// </summary>
    public UColorStyle? cl { get; set; }

    /// <summary>
    /// (Subscript 下标 / Superscript 上标 Text)
    /// </summary>
    public object? va { get; set; }

    /// <summary>
    /// Number format pattern
    /// </summary>
    public UFormatStyle? n { get; set; }
}