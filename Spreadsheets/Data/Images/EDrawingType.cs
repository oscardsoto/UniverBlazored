namespace UniverBlazored.Spreadsheets.Data.Images
{
    /// <summary>
    /// DrawingTypeEnum, from Univer (see doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts)
    /// </summary>
    public enum EDrawingType
    {
        ///<summary>
        /// The drawing type is unrecognized or undefined.
        ///</summary>
        UNRECOGNIZED = -1,
        
        ///<summary>
        /// Represents an image. This could be a picture, logo, etc.
        ///</summary>
        DRAWING_IMAGE = 0,
        
        ///<summary>
        /// Represents a shape like rectangle, circle, triangle, etc.
        ///</summary>
        DRAWING_SHAPE = 1,
        
        ///<summary>
        /// Represents a chart type object (like bar graph, line graph, pie chart, etc).
        ///</summary>
        DRAWING_CHART = 2,
        
        ///<summary>
        /// Represents a table. This could be a grid of data points.
        ///</summary>
        DRAWING_TABLE = 3,
        
        ///<summary>
        /// Represents a smart art object (like a sparkline).
        ///</summary>
        DRAWING_SMART_ART = 4,
        
        ///<summary>
        /// Represents a video file. This could be an embedded youtube video link or similar.
        ///</summary>
        DRAWING_VIDEO = 5,

        ///<summary>
        /// Represents a group of other drawings, could be a layer or a grouped element.
        ///</summary>
        DRAWING_GROUP = 6,
        
        ///<summary>
        /// Represents a unit of measure in the drawing. Could be millimeter, centimeter, inch etc.
        ///</summary>
        DRAWING_UNIT = 7,

        /// <summary>
        /// 
        /// </summary>
        DRAWING_DOM = 8
    }
}