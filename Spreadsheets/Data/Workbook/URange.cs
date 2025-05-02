using System.Text.RegularExpressions;

namespace UniverBlazored.Spreadsheets.Data.Workbook
{
    /// <summary>
    /// IRange, from Univer. (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IRange)
    /// </summary>
    public struct URange
    {
        /// <summary>
        /// The start row (inclusive) of the range startRow
        /// </summary>
        public int startRow { get; set; }

        /// <summary>
        /// The end row (exclusive) of the range endRow
        /// </summary>
        public int endRow { get; set; }

        /// <summary>
        /// The start column (inclusive) of the range startColumn
        /// </summary>
        public int startColumn { get; set; }

        /// <summary>
        /// The end column (exclusive) of the range endColumn
        /// </summary>
        public int endColumn { get; set; }

        /// <summary>
        /// IRange, from Univer. (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IRange)
        /// </summary>
        public URange() {}

        /// <summary>
        /// IRange, from Univer. (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IRange)
        /// </summary>
        /// <param name="col">Cell's column</param>
        /// <param name="row">Cell's row</param>
        public URange(int row, int col)
        {
            startRow = endRow = row;
            startColumn = endColumn = col;
        }

        /// <summary>
        /// IRange, from Univer. (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IRange)
        /// </summary>
        /// <param name="endColumn">Last column of the range</param>
        /// <param name="endRow">Last row of the range</param>
        /// <param name="startColumn">First column of the range</param>
        /// <param name="startRow">First row of the range</param>
        public URange(int startRow, int endRow, int startColumn, int endColumn)
        {
            this.startRow = startRow;
            this.endRow = endRow;
            this.startColumn = startColumn;
            this.endColumn = endColumn;
        }

        /// <summary>
        /// IRange, from Univer. (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IRange)
        /// </summary>
        /// <param name="a1Notation">A1 Notattion for the range/cell</param>
        public URange(string a1Notation)
        {
            // Expresión regular para coincidir con rangos y celdas individuales
            var regex = new Regex(@"([A-Z]+)(\d+)(:([A-Z]+)(\d+))?");
            var match = regex.Match(a1Notation);

            if (!match.Success)
                throw new ArgumentException("La notación no es válida.");

            // Extraemos las partes de la notación
            string startColumnLetter = match.Groups[1].Value;
            int startRow = int.Parse(match.Groups[2].Value);
            
            // Determinamos la columna final
            string endColumnLetter = match.Groups[4].Value;
            int endColumn = string.IsNullOrEmpty(endColumnLetter) ? ColumnLetterToNumber(startColumnLetter) : ColumnLetterToNumber(endColumnLetter);
            
            // Determinamos la fila final
            int endRow = match.Groups[5].Success ? int.Parse(match.Groups[5].Value) : startRow;

            // Si no hay rango (es una sola celda)
            this.startRow = startRow - 1;
            this.startColumn = ColumnLetterToNumber(startColumnLetter) - 1;
            this.endColumn = endColumn - 1;

            // Si es solo una celda
            if (string.IsNullOrEmpty(endColumnLetter))
            {
                this.endRow = this.startRow;
                return;
            }

            // Rango general
            this.endRow = endRow - 1;
        }

        /// <summary>
        /// Returns the first cell of this range
        /// </summary>
        /// <returns></returns>
        public URange GetFirstCellOfRange() => new URange(this.startRow, this.startColumn);

        /// <summary>
        /// Returns the last cell of this range
        /// </summary>
        /// <returns></returns>
        public URange GetLastCellOfRange() => new URange(this.endRow, this.endColumn);

        /// <summary>
        /// Returns this range as a A1 notation
        /// </summary>
        /// <returns></returns>
        public string ToA1Notation()
        {
            string startCell    = $"{ColumnToLetter(startColumn + 1)}{startRow + 1}";
            string endCell      = $"{ColumnToLetter(endColumn + 1)}{endRow + 1}";
            
            if (startRow == endRow && startColumn == endColumn)
                return startCell;
            else
                return startCell + ":" + endCell;
        }

        // Converts the column to letter
        string ColumnToLetter(int column)
        {
            string result = "";
            while (column > 0)
            {
                column--; // Ajustamos para que sea base 0
                result = (char)(column % 26 + 'A') + result;
                column /= 26;
            }
            return result;
        }

        // Convierte una columna en formato letra (A, B, ..., Z, AA, AB, ...) a un índice numérico
        int ColumnLetterToNumber(string columnLetter)
        {
            int result = 0;
            foreach (char c in columnLetter)
                result = result * 26 + (c - 'A' + 1); // Fórmula para convertir letras en número
            return result;
        }
    }
}
