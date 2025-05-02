namespace UniverBlazored.Spreadsheets.Data.Images
{
    /// <summary>
    /// ICellPosition, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-drawing/src/services/sheet-drawing.service.ts#L64)
    /// </summary>
    public struct UCellPosition
    {
        /// <summary>
        /// Column number
        /// </summary>
        public int column { get; set; }

        /// <summary>
        /// Column offset, unit is EMUs
        /// </summary>
        public double columnOffset { get; set; }

        /// <summary>
        /// Row number
        /// </summary>
        public int row { get; set; }

        /// <summary>
        /// Row offset, unit is EMUs
        /// </summary>
        public double rowOffset { get; set; }
    }
}