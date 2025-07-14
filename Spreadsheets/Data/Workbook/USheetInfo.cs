using System.Data;
using System.Text.Json.Nodes;
using UniverBlazored.Spreadsheets.Data.Images;

namespace UniverBlazored.Spreadsheets.Data.Workbook
{
    /// <summary>
    /// Info for the sheet
    /// </summary>
    public struct USheetInfo
    {
        /// <summary>
        /// Univer's Id to the sheet
        /// </summary>
        public string id { get; set; } = "";

        /// <summary>
        /// Sheet's name
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Range used in the sheet
        /// </summary>
        public URange maxUsed { get; set; } = new();

        /// <summary>
        /// True if the sheet is hidden
        /// </summary>
        public bool isHidden { get; set; } = false;

        /// <summary>
        /// List of rows (in range format) hidden in the sheet 
        /// </summary>
        public List<URange> rowsHidden { get; set; } = new();

        /// <summary>
        /// List of columns (in range format) hidden in the sheet 
        /// </summary>
        public List<URange> columnsHidden { get; set; } = new();

        /// <summary>
        /// Max amount of rows used
        /// </summary>
        public string tabColor { get; set; } = "";

        /// <summary>
        /// Info for the sheet
        /// </summary>
        public USheetInfo() { }
    }
}
