namespace UniverBlazored.Spreadsheets.Data.Images
{
    /// <summary>
    /// ISheetDrawingPosition, from Univer (see doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-drawing/src/services/sheet-drawing.service.ts)
    /// </summary>
    public struct USheetTransform
    {
        ///<summary>
        /// True if the image should be flipped along the Y axis (horizontally).
        ///</summary>
        public bool flipY { get; set; }
        
        ///<summary>
        /// True if the image should be flipped along the X axis (vertically).
        ///</summary>
        public bool flipX { get; set; }

        /// <summary>
        /// Rotation angle
        /// </summary>
        public int angle { get; set; }

        ///<summary>
        /// Specifies the horizontal skew angle for the image, in degrees. 
        ///</summary>
        public double skewX { get; set; }
        
        ///<summary>
        /// Specifies the vertical skew angle for the image, in degrees.
        ///</summary>
        public double skewY { get; set; }

        /// <summary>
        /// Cell begin position
        /// </summary>
        /// <value></value>
        public UCellPosition from { get; set; }

        /// <summary>
        /// Cell end position
        /// </summary>
        /// <value></value>
        public UCellPosition to { get; set; }
    }
}