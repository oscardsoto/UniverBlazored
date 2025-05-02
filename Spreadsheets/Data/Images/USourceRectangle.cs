namespace UniverBlazored.Spreadsheets.Data.Images
{
    /// <summary>
    /// ISrcRect, from Univer (see Doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/shared/shape.ts#L37)
    /// </summary>
    public struct USourceRectangle
    {
        /// <summary>
        /// Left crop
        /// </summary>
        public int left { get; set; }

        /// <summary>
        /// Top crop
        /// </summary>
        public int top { get; set; }

        /// <summary>
        /// Bottom crop
        /// </summary>
        public int bottom { get; set; }

        /// <summary>
        /// Right crop
        /// </summary>
        public int right { get; set; }
    }
}