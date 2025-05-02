namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// ITextDecoration, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/ITextDecoration)
    /// </summary>
    public struct UTextDecoration
    {
        /// <summary>
        /// Follow the font color. If true, "cl" does not take effect
        /// </summary>
        public int? c { get; set; }

        /// <summary>
        /// Line Color
        /// </summary>
        public UColorStyle? cl { get; set; } = null;

        /// <summary>
        /// Show the line (0: false, 1: true) (0 by default)
        /// </summary>
        public int s { get; set; } = 0; 

        /// <summary>
        /// Line type
        /// </summary>
        public ETextDecoration? t { get; set; } = null;

        /// <summary>
        /// ITextDecoration, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/ITextDecoration)
        /// </summary>
        public UTextDecoration() { }
    }
}