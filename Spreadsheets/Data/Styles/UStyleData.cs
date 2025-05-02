using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// IStyleData, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IStyleData)
    /// </summary>
        // It has to be a class, beacuse I need nullables...
    public class UStyleData : IStyleBase
    {
        /// <summary>
        /// Bottom border line
        /// </summary>
        public UTextDecoration? bbl { get; set; } = null;

        /// <summary>
        /// Border data
        /// </summary>
        public UBorderData? bd { get; set; } = null;

        /// <summary>
        /// Background
        /// </summary>
        public UColorStyle? bg { get; set;} = null;

        /// <summary>
        /// Bold (0: false, 1: true) (0 by default)
        /// </summary>
        public int bl { get; set; } = 0;

        /// <summary>
        /// Foreground (Font color)
        /// </summary>
        public UColorStyle? cl { get; set; } = null;

        /// <summary>
        /// Font Family (Arial by default)
        /// </summary>
        public string ff { get; set; } = "Arial";

        /// <summary>
        /// Font size (10 by default)
        /// </summary>
        public int fs { get; set; } = 10;

        /// <summary>
        /// Horizontal Alignment (N) (UNSPECIFIED by default)
        /// </summary>
        public EHorizontalAlign ht { get; set; } = EHorizontalAlign.UNSPECIFIED;

        /// <summary>
        /// Italic (0: false, 1: true) (0 by default)
        /// </summary>
        public int it { get; set; } = 0;

        /// <summary>
        /// Number format pattern
        /// </summary>
        public UFormatStyle? n { get; set; } = null;

        /// <summary>
        /// Overline
        /// </summary>
        public UTextDecoration ol { get; set; } = new();

        /// <summary>
        /// Padding (N)
        /// </summary>
        public UPaddingData pd { get; set; } = new();

        /// <summary>
        /// Strike through
        /// </summary>
        public UTextDecoration st { get; set; } = new();

        /// <summary>
        /// Wrap strategy (N) (UNSPECIFIED by default)
        /// </summary>
        public EWrapStrategy tb { get; set; } = EWrapStrategy.UNSPECIFIED;

        /// <summary>
        /// Text direction (N) (UNSPECIFIED by default)
        /// </summary>
        public ETextDirection td { get; set; } = ETextDirection.UNSPECIFIED;

        /// <summary>
        /// Text Rotation (N)
        /// </summary>
        public UTextRotation? tr { get; set; }

        /// <summary>
        /// Underline
        /// </summary>
        public UTextDecoration ul {get; set;} = new();

        /// <summary>
        /// Vertical Align (N) (UNSPECIFIED by default)
        /// </summary>
        public EVerticalAlign? vt { get; set; } = null;

        /// <summary>
        /// Subscript
        /// </summary>
        [Obsolete("used just for chinese.")]
        public object? va { get; set; } = null;

        /// <summary>
        /// Return true if the style is in default values
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool IsDefault(UStyleData style)
        {
            return  style.bbl != null || style.bd != null || style.bg != null || style.bl != 0 || style.cl != null || !style.ff.Equals("Arial") || style.fs != 10 || 
                    style.ht != EHorizontalAlign.UNSPECIFIED || style.it != 0 || style.n != null || style.ol.t != null || style.pd.l != 0 || style.st.t != null ||
                    style.tb != EWrapStrategy.UNSPECIFIED || style.td != ETextDirection.UNSPECIFIED || style.tr != null || style.ul.t != null || style.vt != null;
        } 
    }
}
