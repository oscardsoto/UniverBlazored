using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.ConditionFormat;
using UniverBlazored.Spreadsheets.Data.Images;
using UniverBlazored.Spreadsheets.Data.Styles;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services;

/// <summary>
/// Univer's agent. Enables operations inside Blazor
/// </summary>
public class UniverSpreadsheetAgent
{
    private readonly IUniverJsInterop univerJS;

    /// <summary>
    /// Univer's agent. Enables operations inside Blazor
    /// </summary>
    /// <param name="univerJS">Univer's interop to get acces to Univer</param>
    public UniverSpreadsheetAgent(IUniverJsInterop univerJS)
    {
        this.univerJS = univerJS;
    }

    void SetQueueToPosition() => univerJS.SetAction("getActiveWorkbook").SetAction("getActiveSheet").SetAction("getActiveRange");

    void SetQueueToPage() => univerJS.SetAction("getActiveWorkbook").SetAction("getActiveSheet");

    /// <summary>
    /// Sets the active sheet to the component
    /// </summary>
    /// <param name="idSheet">Id of the sheet to put the changes.</param>
    public async Task SetActiveSheet(string idSheet) => await univerJS.SetAction("getActiveWorkbook").SetAction("setActiveSheet", idSheet).ResolveAsync();

    /// <summary>
    /// Sets the range to work in the active sheet on the component
    /// </summary>
    /// <param name="range"></param>
    public async Task SetActiveRange(URange range) => await univerJS.SetAction("getActiveWorkbook").SetAction("getActiveSheet").SetAction("getRange", range).SetAction("activate").ResolveAsync();

    /// <summary>
    /// Return all sheets information
    /// </summary>  
    /// <returns></returns>
    public async Task<USheetInfo[]> GetSheetsInfo() => await univerJS.ResolveActionAsync<USheetInfo[]>("getSheetsInfo");

    /// <summary>
    /// Adds a new Worksheet in the active workbook
    /// </summary>
    /// <param name="sheetName">Name of the new worksheet</param>
    /// <param name="cols">Amount of columns that the worksheet will have</param>
    /// <param name="rows">Amount of rows that the worksheet will have</param>
    /// <param name="hexColorTab">Color of its tab (in hexadecimal)</param>
    /// <returns></returns>
    public async Task<USheetInfo> AddNewSheet(string sheetName, int rows, int cols, string hexColorTab = null)
    {
        var id = await univerJS.SetAction("getActiveWorkbook").SetAction("create", sheetName, rows, cols).SetAction("setTabColor", hexColorTab).SetAction("getSheetId").ResolveAsync<string>();
        await SetActiveSheet(id);
        return new ()
        {
            id = id,
            name = sheetName,
            maxUsed = new(0, rows - 1, 0, cols - 1),
            tabColor = hexColorTab
        };
    }

    /// <summary>
    /// Removes the specified sheet
    /// </summary>
    /// <param name="idSheet">Id for the sheet to delete. If null or empty, deletes the active sheet</param>
    /// <returns></returns>
    public async Task DeleteSheet(string idSheet = null)
    {
        if (string.IsNullOrEmpty(idSheet))
        {
            SetQueueToPage();
            var id = await univerJS.SetAction("getId").ResolveAsync<string>();
            await univerJS.SetAction("getActiveWorkbook").SetAction("deleteSheet", id).ResolveAsync();
            return;
        }

        await univerJS.SetAction("getActiveWorkbook").SetAction("deleteSheet", idSheet).ResolveAsync();
    }

    /// <summary>
    /// Set a value on a specific Row/Col
    /// </summary>
    /// <param name="value">The value to put on the cell</param>
    public async Task SetValue(object value)
    {
        SetQueueToPosition();
        await univerJS.SetAction("setValue", value).ResolveAsync();
    }

    /// <summary>
    /// Gets the value in the specific Row/col. If not, return null
    /// </summary>
    /// <returns>The value from the cell</returns>
    public async Task<TValue> GetValue<TValue>()
    {
        SetQueueToPosition();
        var result = await univerJS.SetAction("getValue").ResolveAsync<object>();
        JsonElement jsonValue = (JsonElement) result;
        return jsonValue.Deserialize<TValue>() ?? default;
    }

    /// <summary>
    /// Set values on a range 
    /// </summary>
    /// <param name="values">Values of each row for the range. The size of each array must be the same number as the number of columns used</param>
    public async Task SetValue(params object[][] values)
    {
        SetQueueToPosition();
        await univerJS.SetAction("setValues", [values]).ResolveAsync();
    }

    /// <summary>
    /// Gets all values in the range. If a cell in the range is empty, returns null in that cell's position. All non-null values are "JsonElement". Convert to desire.
    /// </summary>
    /// <returns>2 Dimensional array for each row/col values</returns>
    public async Task<object[][]> GetValues()
    {
        SetQueueToPosition();
        return await univerJS.SetAction("getValues").ResolveAsync<object[][]>();
    }

    /// <summary>
    /// Set a formula in the cell
    /// </summary>
    /// <param name="formula">All formulas begin with "="</param>
    /// <returns></returns>
    public async Task SetFormula(string formula)
    {
        SetQueueToPosition();
        await univerJS.SetAction("setFormula", formula).ResolveAsync();
    }

    /// <summary>
    /// Set all formulas in the active range
    /// </summary>
    /// <param name="formulas">All formulas begin with "=". The size of each array must be the same number as the number of columns used</param>
    /// <returns></returns>
    public async Task SetFormula(params string[][] formulas)
    {
        SetQueueToPosition();
        await univerJS.SetAction("setFormulas", [formulas]).ResolveAsync();
    }

    /// <summary>
    /// Returns the formula in the cell. If there's not, return an empty string
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetFormula()
    {
        SetQueueToPosition();
        var result = await univerJS.SetAction("getFormula").ResolveAsync<object>();
        JsonElement jsonRes = (JsonElement) result;
        return jsonRes.Deserialize<string>() ?? default;
    }

    /// <summary>
    /// Returns all formulas in the range. If a cell in the range doesn´t have a formula, returns an empty string in that cell's position
    /// </summary>
    /// <returns></returns>
    public async Task<string[][]> GetFormulas()
    {
        SetQueueToPosition();
        return await univerJS.SetAction("getFormulas").ResolveAsync<string[][]>();
    }

    /// <summary>
    /// Set font properties to the specified range
    /// </summary>
    /// <param name="properties">Properties with null will not be applied (Don't use empty strings)</param>
    /// <returns>This, for chaining</returns>
    public async Task SetFontProperties(UFontProperties properties)
    {
        SetQueueToPosition();
        if (properties.Color != null)
            univerJS.SetAction("setFontColor", properties.Color);

        if (properties.Family != null)
            univerJS.SetAction("setFontFamily", properties.Family);

        if (properties.Strikethrough != null)
            univerJS.SetAction("setFontLine", properties.Strikethrough.Value ? "line-through" : "none");

        if (properties.Underline != null)
            univerJS.SetAction("setFontLine", properties.Underline.Value ? "underline" : "none");

        if (properties.Italic != null)
            univerJS.SetAction("setFontStyle", properties.Italic.Value ? "italic" : "normal");

        if (properties.Size != null)
            univerJS.SetAction("setFontSize", properties.Size);

        if (properties.Bold != null)
            univerJS.SetAction("setFontWeight", properties.Bold.Value ? "bold" : "normal");

        if (properties.HorizontalAlign != null)
            univerJS.SetAction("setHorizontalAlignment", properties.HorizontalAlign.ToString().ToLower());

        if (properties.VerticalAlign != null)
            univerJS.SetAction("setVerticalAlignment", properties.VerticalAlign.ToString().ToLower());

        if (properties.NumberFormat != null)
            univerJS.SetAction("setNumberFormat", properties.NumberFormat);

        if (properties.TextRotation != null)
            univerJS.SetAction("setTextRotation", properties.TextRotation);

        if (properties.BackgroundColor != null)
            univerJS.SetAction("setBackgroundColor", properties.BackgroundColor);

        if (properties.IsWrap != null)
        {
            if ((bool)properties.IsWrap)
            {
                univerJS.SetAction("setWrap", properties.IsWrap);
                if (properties.WrapStrategy != null)
                    univerJS.SetAction("setWrapStrategy", (EWrapStrategy)properties.WrapStrategy);
            }
        }

        await univerJS.ResolveAsync();
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
        var enumValueStyle = (int) borderStyle;
        SetQueueToPosition();
        await univerJS.SetAction("setBorder", enumValueType, enumValueStyle, color).ResolveAsync();
    }

    /// <summary>
    /// Sets all styles in the active range to default
    /// </summary>
    /// <returns></returns>
    public async Task ResetStyle()
    {
        SetQueueToPosition();
        await univerJS.SetAction("useThemeStyle", "default").ResolveAsync();
        
    }

    /// <summary>
    /// Return all styles used in the active range. Be careful with getting a lot of styles. It may cause a StackOverflowException!
    /// </summary>
    /// <returns></returns>
    public async Task<UStyleData[][]> GetStyles() => await univerJS.ResolveActionAsync<UStyleData[][]>("getCellsStylesInfo");

    /// <summary>
    /// Sets a hyper link in the first cell on the active range
    /// </summary>
    /// <param name="text">Text for the Hyperlink</param>
    /// <param name="link">Link to redirect</param>
    /// <returns></returns>
    public async Task SetHyperLink(string text, string link)
    {
        await univerJS.ResolveActionAsync("insertHyperLink", text, link);
    }

    /// <summary>
    /// Sort selected range by ascending
    /// </summary>
    /// <param name="columns">Columns to be sorted</param>
    /// <returns>This, for chaining</returns>
    public async Task SortAscending(params int[] columns)
    {
        SetQueueToPosition();
        await univerJS.SetAction("sort", columns).ResolveAsync();
    }

    /// <summary>
    /// Sort selected range by ascending or descending
    /// </summary>
    /// <param name="sorts">(column: Column to be sorted, ascending: True if the col will be sorted by ascending)</param>
    /// <returns>This, for chaining</returns>
    public async Task SortAscending(params (int column, bool ascending)[] sorts)
    {
        SetQueueToPosition();
        List<object> sort = [];
        foreach(var s in sorts)
            sort.Add(new{ 
                column = s.column, 
                ascending = s.ascending 
            });
        await univerJS.SetAction("sort", sort.ToArray()).ResolveAsync();
    }

    /// <summary>
    /// Merge all cells in range
    /// </summary>
    /// <param name="strategy">Strategy to merge all cells in range</param>
    /// <param name="defaultMerge">True if the value in the upper left cell will be retained</param>
    /// <returns></returns>
    public async Task Merge(MergeStrategy strategy, bool defaultMerge)
    {
        string action = "";
        switch (strategy)
        {
            case MergeStrategy.ALL:
                action = "merge";
                break;

            case MergeStrategy.HORIZONTAL:
                action = "mergeAcross";
                break;

            case MergeStrategy.VERTICAL:
                action = "mergeVertically";
                break;
        }
        SetQueueToPosition();
        await univerJS.SetAction(action, defaultMerge).ResolveAsync();
    }

    /// <summary>
    /// Unmerge all cells in range
    /// </summary>
    /// <returns></returns>
    public async Task BreakMerge()
    {
        SetQueueToPosition();
        await univerJS.SetAction("breakApart").ResolveAsync();
        
    }

    /// <summary>
    /// Get all merges in active page
    /// </summary>
    /// <returns>All ranges that correspond to each merge</returns>
    public async Task<URange[]> GetAllMerges()
    {
        return await univerJS.ResolveActionAsync<URange[]>("getAllMerges");
    }

    /// <summary>
    /// True if the range has cells that overlap a merged cell 
    /// </summary>
    public async Task<bool> RangeIsPartOfMerge()
    {
        SetQueueToPosition();
        return await univerJS.SetAction("isPartOfMerge").ResolveAsync<bool>();
    }

    /// <summary>
    /// Create a filter for the selected range (on the active page)
    /// </summary>
    /// <returns></returns>
    public async Task CreateFilter()
    {
        SetQueueToPosition();
        await univerJS.SetAction("createFilter").ResolveAsync();
    }

    /// <summary>
    /// Returns true if the active page has a filter on it
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasFilter()
    {
        return await univerJS.ResolveActionAsync<bool>("hasFilter");
    }

    /// <summary>
    /// Gets the range's filter for the active page
    /// </summary>
    /// <returns>URange object with the range of the filter</returns>
    public async Task<URange?> GetFilter()
    {
        SetQueueToPage();
        return await univerJS.SetAction("getFilter").SetAction("getRange").SetAction("getRange").ResolveAsync<URange?>();
    }

    /// <summary>
    /// Removes the filter for the active page
    /// </summary>
    /// <returns></returns>
    public async Task RemoveFilter()
    {
        SetQueueToPage();
        await univerJS.SetAction("getFilter").SetAction("remove").ResolveAsync();
    }

    /// <summary>
    /// Adds a conditional format to the active page in the active range
    /// </summary>
    /// <param name="type">Where the conditional format will trigger</param>
    /// <param name="style">Style that the cell will have</param>
    /// <returns></returns>
    public async Task AddConditionalFormat(UConditionType type, UConditionFormatStyle style)
    {
        SetQueueToPosition();
        URange range = await univerJS.SetAction("getRange").ResolveAsync<URange>();

        SetQueueToPage();
        SetConditionalFormatTypeToQueue(type);
        SetConditionalFormatStyleToQueue(style);
        univerJS.SetAction("setRanges", new URange[]{ range }).SetAction("build");

        await univerJS.ResolveActionAsync("addConditionalFormat", univerJS.actionQueue.ToArray());
        univerJS.actionQueue.Clear();
    }

    /// <summary>
    /// Adds a conditional format to the active page in the specified ranges
    /// </summary>
    /// <param name="type">Where the conditional format will trigger</param>
    /// <param name="style">Style that the cell will have</param>
    /// <param name="ranges">Ranges where the conditional format will apply</param>
    /// <returns></returns>
    public async Task AddConditionalFormat(UConditionType type, UConditionFormatStyle style, params URange[] ranges)
    {
        SetQueueToPage();
        SetConditionalFormatTypeToQueue(type);
        SetConditionalFormatStyleToQueue(style);
        univerJS.SetAction("setRanges", ranges).SetAction("build");

        await univerJS.ResolveActionAsync("addConditionalFormat", univerJS.actionQueue.ToArray());
        univerJS.actionQueue.Clear();
    }

    /// <summary>
    /// Deletes the specified conditional format by its Id
    /// </summary>
    /// <param name="idRule">Conditional format id</param>
    /// <returns></returns>
    public async Task DeleteConditionalFormat(string idRule)
    {
        SetQueueToPage();
        await univerJS.SetAction("deleteConditionalFormattingRule", idRule).ResolveAsync();
    }

    /// <summary>
    /// Clears all conditional formats in the page
    /// </summary>
    /// <returns></returns>
    public async Task ClearConditionalFormats()
    {
        SetQueueToPage();
        await univerJS.SetAction("clearConditionalFormatRules").ResolveAsync();
    }

    /// <summary>
    /// Gets all conditional formats in the page
    /// </summary>
    /// <returns>An array of each conditional format in the page</returns>
    public async Task<UConditionalFormatRule[]> GetAllConditionalFormats()
    {
        SetQueueToPage();
        return await univerJS.SetAction("getConditionalFormattingRules").ResolveAsync<UConditionalFormatRule[]>();
    }

    /// <summary>
    /// Apply the rule in the specified range with its Id
    /// </summary>
    /// <param name="idRule">Conditional format id</param>
    /// <param name="range">Range to be applied</param>
    /// <returns></returns>
    public async Task ApplyConditionalFormat(string idRule, URange range)
    {
        var rules = await GetAllConditionalFormats();
        UConditionalFormatRule rule = rules.Where(r => r.cfId.Equals(idRule)).FirstOrDefault();
        if (string.IsNullOrEmpty(rule.cfId))
            throw new UniverException($"cfId does not exist: {idRule}");

        rule.ranges = [range];
        await univerJS.SetAction("setConditionalFormattingRule", idRule, rule).ResolveAsync();
    }

    /// <summary>
    /// Set the condition to the conditional format
    /// </summary>
    /// <param name="type"></param>
    void SetConditionalFormatTypeToQueue(UConditionType type)
    {
        univerJS.SetAction("newConditionalFormattingRule");

        if (type.IsIconSet || type.IsColorScale || type.IsDataBar)
            return;

        if (type.WhenCellEmpty)
        {
            univerJS.SetAction("whenCellEmpty");
            return;
        }

        if (type.WhenCellNotEmpty)
        {
            univerJS.SetAction("whenCellNotEmpty");
            return;
        }

        if (type.WhenDate != null)
        {
            univerJS.SetAction("whenDate", type.WhenDate.Value.ToString());
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenFormulaSatisfied))
        {
            univerJS.SetAction("whenFormulaSatisfied", type.WhenFormulaSatisfied);
            return;
        }

        if (type.WhenNumberEqualTo != null)
        {
            double value = type.WhenNumberEqualTo.Value;
            univerJS.SetAction("whenNumberEqualTo", value);
            return;
        }

        if (type.WhenNumberGreaterThan != null)
        {
            double value = type.WhenNumberGreaterThan.Value;
            univerJS.SetAction("whenNumberGreaterThan", value);
            return;
        }

        if (type.WhenNumberGreaterThanOrEqual != null)
        {
            double value = type.WhenNumberGreaterThanOrEqual.Value;
            univerJS.SetAction("whenNumberGreaterThanOrEqual", value);
            return;
        }

        if (type.WhenNumberInBetween != null)
        {
            (double starts, double ends) value = type.WhenNumberInBetween.Value;
            univerJS.SetAction("whenNumberBetween", value.starts, value.ends);
            return;
        }

        if (type.WhenNumberLessThan != null)
        {
            double value = type.WhenNumberLessThan.Value;
            univerJS.SetAction("whenNumberLessThan", value);
            return;
        }

        if (type.WhenNumberLessThanOrEqual != null)
        {
            double value = type.WhenNumberLessThanOrEqual.Value;
            univerJS.SetAction("whenNumberLessThanOrEqual", value);
            return;
        }

        if (type.WhenNumberNotBetween != null)
        {
            (double starts, double ends) value = type.WhenNumberNotBetween.Value;
            univerJS.SetAction("whenNumberNotBetween", value.starts, value.ends);
            return;
        }

        if (type.WhenNumberNotEqual != null)
        {
            double value = type.WhenNumberNotEqual.Value;
            univerJS.SetAction("whenNumberNotEqual", value);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextContains))
        {
            univerJS.SetAction("whenTextContains", type.WhenTextContains);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextDoesNotContain))
        {
            univerJS.SetAction("whenTextDoesNotContain", type.WhenTextDoesNotContain);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextEndsWith))
        {
            univerJS.SetAction("whenTextEndsWith", type.WhenTextEndsWith);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextEqualTo))
        {
            univerJS.SetAction("whenTextEqualTo", type.WhenTextEqualTo);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextStartsWith))
        {
            univerJS.SetAction("whenTextStartsWith", type.WhenTextStartsWith);
            return;
        }
    }

    /// <summary>
    /// Set all styles for the conditional format 
    /// </summary>
    /// <param name="style"></param>
    void SetConditionalFormatStyleToQueue(UConditionFormatStyle style)
    {
        if (style.Average != null)
            univerJS.SetAction("setAverage", style.Average.Value);

        if (!string.IsNullOrEmpty(style.Background))
            univerJS.SetAction("setBackground", style.Background);

        if (style.ColorScale != null)
            univerJS.SetAction("setColorScale", style.ColorScale.Value.config);

        if (style.DataBar != null)
            univerJS.SetAction("setDataBar", style.DataBar.Value);

        if (style.DuplicateValues)
            univerJS.SetAction("setDuplicateValues");

        if (!string.IsNullOrEmpty(style.FontColor))
            univerJS.SetAction("setFontColor", style.FontColor);

        if (style.IconSet != null)
            univerJS.SetAction("setIconSet", new UIconSetCond(style.IconSet.Value.isShowValue, style.IconSet.Value.config));

        if (style.IsBold != null)
            univerJS.SetAction("setBold", style.IsBold.Value);

        if (style.Italic != null)
            univerJS.SetAction("setItalic", style.Italic.Value);

        if (style.Rank != null)
            univerJS.SetAction("setRank", new URankConfig(style.Rank.Value.isBottom, style.Rank.Value.isPercent, style.Rank.Value.value));

        if (style.Strikethrough != null)
            univerJS.SetAction("setStrikethrough", style.Strikethrough.Value);

        if (style.Underline != null)
            univerJS.SetAction("setUnderline", style.Underline.Value);

        if (style.UniqueValues)
            univerJS.SetAction("setUniqueValues");
    }

    /// <summary>
    /// Set an image to the active page on the active range
    /// </summary>
    /// <param name="urlImage">URL (or data Uri) for the image (Only accepts JPEG, PNG, TIFF, GIF (no animated), ICO and SVG)</param>
    /// <returns></returns>
    public async Task AddImage(string urlImage)
    {
        SetQueueToPosition();
        URange activeRange = await univerJS.ResolveAsync<URange>();
        SetQueueToPage();
        await univerJS.SetAction("insertImage", urlImage, activeRange.startColumn, activeRange.startRow).ResolveAsync();
    }

    /// <summary>
    /// Set an image to the active page on the specified row and column
    /// </summary>
    /// <param name="urlImage">URL (or Data Uri) for the image (Only accepts JPEG, PNG, TIFF, GIF (no animated), ICO and SVG)</param>
    /// <param name="row">Row's position</param>
    /// <param name="col">Col's position</param>
    /// <param name="rowOffset">Row's offset for the image (null to ignore)</param>
    /// <param name="colOffset">Column's offset for the image (null to ignore)</param>
    /// <returns></returns>
    public async Task AddImage(string urlImage, int row, int col, double? rowOffset = null, double? colOffset = null)
    {
        SetQueueToPage();
        await univerJS.SetAction("insertImage", urlImage, col, row, rowOffset, colOffset).ResolveAsync();
    }

    /// <summary>
    /// Set images on the active page
    /// </summary>
    /// <param name="images">Full properties of each image that will be added</param>
    /// <returns></returns>
    public async Task AddImage(params UImage[] images)
    {
        SetQueueToPage();
        await univerJS.SetAction("insertImages", images).ResolveAsync();
    }

    /// <summary>
    /// Return all images on the active page (Use this if all images combined are less than 33KB, otherwise will fail in Exception)
    /// </summary>
    /// <returns></returns>
    public async Task<UImage[]> GetImages()
    {
        SetQueueToPage();
        return await univerJS.SetAction("getImages").ResolveAsync<UImage[]>();
    }

    /// <summary>
    /// Return all images ids in the active sheet
    /// </summary>
    /// <returns></returns>
    public async Task<string[]> GetImagesId() => await univerJS.ResolveActionAsync<string[]>("getImagesId");

    /// <summary>
    /// Return the image by id on the active page (Use this if the image is less than 33KB, otherwise will fail in Exception.)
    /// </summary>
    /// <param name="id">Image Id</param>
    /// <param name="withSource">False if you dont want to get the image source (if the image is more than 33KB)</param>
    /// <returns></returns>
    public async Task<UImage> GetImage(string id, bool withSource = true) => await univerJS.ResolveActionAsync<UImage>("getImageById", id, withSource);

    /// <summary>
    /// Return the data Uri of the image in the active sheet
    /// </summary>
    /// <param name="id">Image Id</param>
    /// <returns></returns>
    public async Task<string> GetImageSource(string id)
    {
        SetQueueToPage();
        int counterLength = 0;
        int maxChunk = 29000;   // 29 KB, to keep the limit
        string source = "", chunk = "";
        do
        {
            object val = await univerJS.SetAction("getImageById", id)
                                    .SetAction("toBuilder")
                                    .SetAction("getSource")
                                    .SetAction("slice", counterLength,  counterLength + maxChunk)
                                    .ResolveAsync<object>();
            JsonElement jsonVal = (JsonElement) val;
            chunk = (jsonVal.ValueKind is JsonValueKind.String) ? jsonVal.GetString() ?? "" : "";
            counterLength += maxChunk + 1;
            source += chunk;
        }
        while (!string.IsNullOrEmpty(chunk));
        return source;
    }

    /// <summary>
    /// Remove all selected images in the active sheet
    /// </summary>
    /// <param name="ids">Images ids</param>
    public async Task DeleteImagesById(params string[] ids)
    {
        foreach (string id in ids)
        {
            SetQueueToPage();
            await univerJS.SetAction("getImageById", id).SetAction("remove").ResolveAsync();
        }
    }

    /// <summary>
    /// Insert a comment in the first cell on the active range
    /// </summary>
    /// <param name="comment">Comment to insert</param>
    /// <returns></returns>
    public async Task InsertComment(UniverComment comment)=> await univerJS.ResolveActionAsync("insertComment", comment);

    /// <summary>
    /// Returns the first (root) comment at the first cell in the active  range
    /// </summary>
    /// <param name="delete">True for deleting the selected comment</param>
    /// <returns></returns>
    public async Task<UniverComment> GetComment(bool delete = false)
    {
        SetQueueToPosition();
        var position = await univerJS.SetAction("getComment").SetAction("getCommentData").ResolveAsync<UniverComment>();
        if (delete)
        {
            SetQueueToPosition();
            await univerJS.SetAction("getComment").SetAction("delete").ResolveAsync<UniverComment>();
        }
        return position;
    }

    /// <summary>
    /// Return all comments in the sheet
    /// </summary>
    /// <returns></returns>
    public async Task<UniverComment[]> GetComments() => await univerJS.ResolveActionAsync<UniverComment[]>("getAllComments");

    /// <summary>
    /// Delete all comments in the page
    /// </summary>
    /// <returns></returns>
    public async Task ClearComments()
    {
        SetQueueToPage();
        await univerJS.SetAction("clearComments").ResolveAsync();
    }

    /// <summary>
    /// Delete all selected images on the active sheet
    /// </summary>
    /// <param name="images">Selected images to delete (must be full objects)</param>
    /// <returns></returns>
    public async Task DeleteImages(params UImage[] images)
    {
        SetQueueToPage();
        await univerJS.SetAction("deleteImages", images).ResolveAsync();
    }

    /// <summary>
    /// Freeze the amount of rows and columns (beggining in A1)
    /// </summary>
    /// <param name="rows">Number of rows to freeze</param>
    /// <param name="cols">Number of cols to freeze</param>
    /// <returns></returns>
    public async Task SetFreeze(int rows, int cols)
    {
        SetQueueToPage();
        await univerJS.SetAction("setFrozenRows", rows).SetAction("setFrozenColumns", cols).ResolveAsync();
    }

    /// <summary>
    /// Cancels the frozen state of the current sheet.
    /// </summary>
    /// <returns></returns>
    public async Task CancelFreeze()
    {
        SetQueueToPage();
        await univerJS.SetAction("cancelFreeze").ResolveAsync();
    }

    /// <summary>
    /// Returns the frozen state (rows and cols) of the page
    /// </summary>
    /// <returns>Frozen state object</returns>
    public async Task<UFreeze> GetFreeze()
    {
        SetQueueToPage();
        return await univerJS.SetAction("getFreeze").ResolveAsync<UFreeze>();
    }

    // Default:
    //      RowHeight   -> Univer/24 (px), ClosedXML/15 (pt)
    //      ColumnWidth -> Univer/88 (px), ClosedXML/8.43 (NoC)
    // We gotta use "Rule of 3" in order to convert the points

    /// <summary>
    /// Return an array of all heights of each row index indicated
    /// </summary>
    /// <param name="rowPos">Rows positions</param>
    /// <returns></returns>
    public async Task<double[]> GetRowsHeights(params int[] rowPos)
    {
        var results = new List<double>();
        foreach (int pos in rowPos)
        {
            SetQueueToPage();
            results.Add(await univerJS.SetAction("getRowHeight", pos).ResolveAsync<double>());
        }
        return results.ToArray();
    }

    /// <summary>
    /// Return an array of widths of each column index indicated
    /// </summary>
    /// <param name="colPos">Columns positions</param>
    /// <returns></returns>
    public async Task<double[]> GetColumnWidth(params int[] colPos)
    {
        var results = new List<double>();
        foreach (int pos in colPos)
        {
            SetQueueToPage();
            results.Add(await univerJS.SetAction("getColumnWidth", pos).ResolveAsync<double>());
        }
        return results.ToArray();
    }

    /// <summary>
    /// Sets the column's width at the position 
    /// </summary>
    /// <param name="colPos">Column position</param>
    /// <param name="width">Width of that column</param>
    /// <returns></returns>
    public async Task SetColumnWidth(int colPos, double width)
    {
        SetQueueToPage();
        await univerJS.SetAction("setColumnWidth", colPos, width).ResolveAsync();
    }

    /// <summary>
    /// Sets the row's height at the position
    /// </summary>
    /// <param name="rowPos">Row position</param>
    /// <param name="height">height of that row</param>
    /// <returns></returns>
    public async Task SetRowHeight(int rowPos, double height)
    {
        SetQueueToPage();
        await univerJS.SetAction("setRowHeight", rowPos, height).ResolveAsync();
    }

    /// <summary>
    /// Inserts one or more consecutive blank columns in a sheet starting at the specified location.
    /// </summary>
    /// <param name="colPos">The index indicating where to insert a column, starting at 0 for the first column</param>
    /// <param name="colCount">The number of columns to insert</param>
    /// <returns></returns>
    public async Task InsertColumns(int colPos, int colCount = 1)
    {
        SetQueueToPage();
        await univerJS.SetAction("insertColumns", colPos, colCount).ResolveAsync();
    }

    /// <summary>
    /// Deletes a number of columns starting at the given column position.
    /// </summary>
    /// <param name="colPos">The position of the first column to delete, starting at 0 for the first column</param>
    /// <param name="colCount">The number of columns to delete</param>
    /// <returns></returns>
    public async Task DeleteColumns(int colPos, int colCount = 1)
    {
        SetQueueToPage();
        await univerJS.SetAction("deleteColumns", colPos, colCount).ResolveAsync();
    }

    /// <summary>
    /// Inserts one or more consecutive blank rows in a sheet starting at the specified location.
    /// </summary>
    /// <param name="rowPos">The index indicating where to insert a row, starting at 0 for the first row.</param>
    /// <param name="rowCount">The number of rows to insert.</param>
    /// <returns></returns>
    public async Task InsertRows(int rowPos, int rowCount = 1)
    {
        SetQueueToPage();
        await univerJS.SetAction("insertRows", rowPos, rowCount).ResolveAsync();
    }

    /// <summary>
    /// Deletes a number of rows starting at the given row position.
    /// </summary>
    /// <param name="rowPos">The position of the first row to delete, starting at 0 for the first row.</param>
    /// <param name="rowCount">The number of rows to delete.</param>
    /// <returns></returns>
    public async Task DeleteRows(int rowPos, int rowCount = 1)
    {
        SetQueueToPage();
        await univerJS.SetAction("deleteRows", rowPos, rowCount).ResolveAsync();
    }
}