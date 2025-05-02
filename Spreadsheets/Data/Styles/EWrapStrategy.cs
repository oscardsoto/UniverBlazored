namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// WrapStrategy, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/enumerations/WrapStrategy)
    /// </summary>
    public enum EWrapStrategy
    {
        /// <summary>
        /// Context dependent
        /// </summary>
        UNSPECIFIED,

        /// <summary>
        /// Not cut text at the end of the cell
        /// </summary>
        OVERFLOW,

        /// <summary>
        /// Cut the text at the end of the cell
        /// </summary>
        CLIP,

        /// <summary>
        /// Just wrap
        /// </summary>
        WRAP
    }
}