namespace UniverBlazored.Spreadsheets.Data.Images
{    
    /// <summary>
    /// ITransformState, from Univer (see Doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts)
    /// </summary>
    public struct UTransform
    {
        ///<summary>
        /// True if the image should be flipped along the Y axis (horizontally).
        ///</summary>
        public bool flipY { get; set; }
        
        ///<summary>
        /// True if the image should be flipped along the X axis (vertically).
        ///</summary>
        public bool flipX { get; set; }
        
        ///<summary>
        /// The rotation angle of the image in degrees. A positive value will rotate clockwise and a negative one counter-clockwise.
        ///</summary>
        public int angle { get; set; }
        
        ///<summary>
        /// Specifies the horizontal skew angle for the image, in degrees. 
        ///</summary>
        public double skewX { get; set; }
        
        ///<summary>
        /// Specifies the vertical skew angle for the image, in degrees.
        ///</summary>
        public double skewY { get; set; }

        ///<summary>
        /// The distance from the left edge of the containing cell to the left side of this image, in pixel units.
        ///</summary>
        public double left { get; set; }
    
        ///<summary>
        /// The distance from the top edge of the containing cell to the top side of this image, in pixel units.
        ///</summary>
        public double top { get; set; }
    
        ///<summary>
        /// The width of this image, in pixel units.
        ///</summary>
        public double width { get; set; }
    
        ///<summary>
        /// The height of this image, in pixel units.
        ///</summary>
        public double height { get; set; }
    }
}