namespace UniverBlazored.Spreadsheets.Data.Images
{
    /// <summary>
    /// IFOverGridImage, from Univer (see Doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-drawing-ui/src/facade/f-over-grid-image.ts#L27)
    /// </summary>
    public struct UImage
    {
        /// <summary>
        /// Workbook Id
        /// </summary>
        public string unitId { get; set; } = "";

        /// <summary>
        /// Worksheet Id
        /// </summary>
        public string subUnitId { get; set; } = "";

        /// <summary>
        /// Image id on Univer
        /// </summary>
        public string drawingId { get; set; } = "";

        /// <summary>
        /// Image type
        /// </summary>
        public EDrawingType drawingType { get; set; }

        private EImageSourceType sourceT;

        /// <summary>
        /// Type of source for the image 
        /// </summary>
        public string imageSourceType
        {
            get
            {
                return sourceT.ToString();
            }
            set
            {
                bool ok = Enum.TryParse(value, out sourceT);
                if (!ok)
                    sourceT = EImageSourceType.URL;
            }
        }

        /// <summary>
        /// Data Uri for the image (originaly string)
        /// </summary>
        public string source { get; set; } = "";

        /// <summary>
        /// Position in Sheet
        /// </summary>
        public USheetTransform sheetTransform { get; set; }

        /// <summary>
        /// Transform state
        /// </summary>
        public UTransform transform { get; set; }

        /// <summary>
        /// Source Rectangle (from cropping the image)
        /// </summary>
        public USourceRectangle? srcRect { get; set; }

        // prstGeometry is ignored
        
        /// <summary>
        /// IFOverGridImage, from Univer (see Doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-drawing-ui/src/facade/f-over-grid-image.ts#L27)
        /// </summary>
        public UImage() { }

        /// <summary>
        /// Return the base64 data from the Data Uri source
        /// </summary>
        /// <returns></returns>
        public string GetBase64()
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return source.Split(',')[1];
        }

        /// <summary>
        /// Returns the data type from the Data Uri source
        /// </summary>
        /// <returns></returns>
        public string GetImageType()
        {
            if (string.IsNullOrEmpty(source))
                return source;
            return source.Split(':')[1].Split(';')[0];
        }
    }
}