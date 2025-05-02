namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// IColorStyle, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IColorStyle)
    /// </summary>
    public struct UColorStyle
    {
        /// <summary>
        /// Rgb color with the hexadecimal digit
        /// </summary>
        public string rgb { get; set; }

        /// <summary>
        /// Theme color selected (to check if it will take effect)
        /// </summary>
        public int? th { get; set; }
    }
}