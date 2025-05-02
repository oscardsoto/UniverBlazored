namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// IBorderStyleData, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IBorderStyleData)
    /// </summary>
    public struct UBorderStyleData
    {
        /// <summary>
        /// Border Color
        /// </summary>
        public UColorStyle? cl { get; set; }

        /// <summary>
        /// Border Type
        /// </summary>
        public EBorderStyleType? s { get; set; }
    }
}