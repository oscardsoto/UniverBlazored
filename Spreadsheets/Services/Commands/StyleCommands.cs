using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Styles;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Styles
/// </summary>
public class StyleCommands : USpreadsheetCommandBase<StyleCommands>
{
    /// <summary>
    /// Commands focused on Sheet Styles
    /// </summary>
    /// <param name="snapshot"></param>
    /// <param name="univerJs">Js Interop service</param>
    /// <returns></returns>
    public StyleCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Set font properties to the specified range
    /// </summary>
    /// <param name="properties">Properties with null will not be applied (Don't use empty strings)</param>
    /// <returns>This, for chaining</returns>
    public async Task SetFontProperties(UFontProperties properties)
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        if (properties.Color != null)
            queue.SetAction("setFontColor", properties.Color);

        if (properties.Family != null)
            queue.SetAction("setFontFamily", properties.Family);

        if (properties.Strikethrough != null)
            queue.SetAction("setFontLine", properties.Strikethrough.Value ? "line-through" : "none");

        if (properties.Underline != null)
            queue.SetAction("setFontLine", properties.Underline.Value ? "underline" : "none");

        if (properties.Italic != null)
            queue.SetAction("setFontStyle", properties.Italic.Value ? "italic" : "normal");

        if (properties.Size != null)
            queue.SetAction("setFontSize", properties.Size);

        if (properties.Bold != null)
            queue.SetAction("setFontWeight", properties.Bold.Value ? "bold" : "normal");

        if (properties.HorizontalAlign != null)
            queue.SetAction("setHorizontalAlignment", properties.HorizontalAlign.ToString().ToLower());

        if (properties.VerticalAlign != null)
            queue.SetAction("setVerticalAlignment", properties.VerticalAlign.ToString().ToLower());

        if (properties.NumberFormat != null)
            queue.SetAction("setNumberFormat", properties.NumberFormat);

        if (properties.TextRotation != null)
            queue.SetAction("setTextRotation", properties.TextRotation);

        if (properties.BackgroundColor != null)
            queue.SetAction("setBackgroundColor", properties.BackgroundColor);

        if (properties.IsWrap != null)
        {
            if ((bool)properties.IsWrap)
            {
                queue.SetAction("setWrap", properties.IsWrap);
                if (properties.WrapStrategy != null)
                    queue.SetAction("setWrapStrategy", (EWrapStrategy)properties.WrapStrategy);
            }
        }

        await queue.ResolveQueueAsync();
    }

    /// <summary>
    /// Set border to the range
    /// </summary>
    /// <param name="borderType">Border Type to apply to the range</param>
    /// <param name="borderStyle">Border Style to apply to the border's range</param>
    /// <param name="color">Color on hexadecimal ()</param>
    /// <returns>This, for chaining</returns>
    public async Task SetBorderStyle(EBorderType borderType, EBorderStyleType borderStyle, string color)
    {
        var enumValueType = borderType.ToString().ToLower();
        var enumValueStyle = (int)borderStyle;
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction("setBorder", enumValueType, enumValueStyle, color).ResolveQueueAsync();
    }

    /// <summary>
    /// Sets all styles in the active range to default
    /// </summary>
    /// <returns></returns>
    public async Task ResetStyle()
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction("useThemeStyle", "default").ResolveQueueAsync();
    }

    /// <summary>
    /// Set to all ranges the same style asynchronously
    /// </summary>
    /// <param name="style">Font properties ti apply</param>
    /// <param name="ranges">Ranges to set the style</param>
    /// <returns></returns>
    public async Task SetStylesAsync(UFontProperties style, params URange[] ranges)
    {
        await UniverJS.ResolveActionAsync("setRangeStyles", this.Snapshot, style, ranges);
    }

    /// <summary>
    /// Set to all ranges the same border style asynchronously
    /// </summary>
    /// <param name="bordersInfo">All info about the borders of each range</param>
    /// <param name="ranges">Ranges to apply changes</param>
    /// <returns></returns>
    public async Task SetBordersAsync(List<(EBorderType type, EBorderStyleType style, string color)> bordersInfo, params URange[] ranges)
    {
        List<object> borders = new();
        foreach (var border in bordersInfo)
        {
            borders.Add(new
            {
                type = border.type.ToString().ToLower(),
                style = (int)border.style,
                color = border.color,
            });
        }
        await UniverJS.ResolveActionAsync("setRangeBorders", this.Snapshot, borders, ranges);
    }

    class StyleReference
    {
        /// <summary>
        /// style
        /// </summary>
        public UStyleData s { get; set; }

        /// <summary>
        /// positions
        /// </summary>
        public int[][] p { get; set; }

        /// <summary>
        /// Convert all positions to ranges
        /// </summary>
        /// <returns></returns>
        public URange[] ToRanges()
        {
            var ranges = new List<URange>();
            foreach (var pos in p)
                ranges.Add(new(pos[0], pos[1]));
            return ranges.ToArray();
        }
    }

    /// <summary>
    /// Return all styles used in the active range. Be careful with getting a lot of styles. It may cause a StackOverflowException!
    /// </summary>
    /// <returns></returns>
    public async Task<Dictionary<UStyleData, URange[]>> GetStyles()
    {
        var references = await UniverJS.ResolveActionAsync<StyleReference[]>("getCellsStylesInfo", this.Snapshot);

        // Each "p" contains [row, col]
        var styleGroups = new Dictionary<UStyleData, URange[]>();

        foreach (var reference in references)
        {
            // p = positions
            var regions = reference.ToRanges();
            var result = new List<URange>();
            var visited = new HashSet<URange>();
            var grid = new HashSet<URange>(regions);

            foreach (var region in regions)
            {
                if (visited.Contains(region))
                    continue;

                int endCol = region.startColumn;
                // Expand to the right
                while (grid.Contains(new(region.startRow, endCol + 1)) && !visited.Contains(new(region.startRow, endCol + 1)))
                    endCol++;

                int endRow = region.startRow;
                bool fullRowMatch;

                // Expand downward while full row match
                do
                {
                    endRow++;
                    fullRowMatch = true;
                    for (int col = region.startColumn; col <= endCol; col++)
                    {
                        if (!grid.Contains(new(endRow, col)) || visited.Contains(new(endRow, col)))
                        {
                            fullRowMatch = false;
                            break;
                        }
                    }
                }
                while (fullRowMatch);

                // Final endRow is the last matching one
                endRow--;

                // Mark all block as visited
                for (int row = region.startRow; row <= endRow; row++)
                    for (int col = region.startColumn; col <= endCol; col++)
                        visited.Add(new(row, col));

                result.Add(new(region.startRow, endRow, region.startColumn, endCol));
            }

            styleGroups.Add(reference.s, result.ToArray());
        }
        return styleGroups;
    }

    /// <summary>
    /// Sets a hyper link in the first cell on the active range
    /// </summary>
    /// <param name="text">Text for the Hyperlink</param>
    /// <param name="link">Link to redirect</param>
    /// <returns></returns>
    public async Task SetHyperLink(string text, string link)
    {
        await UniverJS.ResolveActionAsync("insertHyperLink", this.Snapshot, text, link);
    }

    /// <inheritdoc/>
    protected override StyleCommands Clone(USpreadsheetSnapshot context)
        => new StyleCommands(context, UniverJS);
}