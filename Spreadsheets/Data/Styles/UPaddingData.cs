namespace UniverBlazored.Spreadsheets.Data.Styles
{
    
    /// <summary>
    /// IPaddingData, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IPaddingData)
    /// </summary>
    public struct UPaddingData
    {
        /// <summary>
        /// Top
        /// </summary>
        public int t { get; set;} = 0;

        /// <summary>
        /// Bottom
        /// </summary>
        public int b { get; set; } = 0;

        /// <summary>
        /// Left
        /// </summary>
        public int l { get; set; } = 0;

        /// <summary>
        /// Right
        /// </summary>
        public int r { get; set; } = 0;

        /// <summary>
        /// IPaddingData, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IPaddingData)
        /// </summary>
        public UPaddingData() { }
    }
}