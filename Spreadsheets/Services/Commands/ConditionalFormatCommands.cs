using UniverBlazored;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.ConditionFormat;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Conditional Formats
/// </summary>
public class ConditionalFormatCommands : USpreadsheetCommandBase<ConditionalFormatCommands>
{
    /// <summary>
    /// Commands focused on Sheet Conditional Formats
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <param name="snapshot"></param>
    public ConditionalFormatCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    public async Task AddConditionalFormat(UConditionType type, UConditionFormatStyle style)
    {
        await ExecuteAtomically(async inner =>
        {
            var rangeQueue = new UniverQueue(inner, Snapshot.ToContext());
            UseRange(rangeQueue);
            URange range = await rangeQueue.SetAction("getRange").ResolveQueueAsync<URange>();

            var ruleQueue = new UniverQueue(inner, Snapshot.ToContext());
            UseSheet(ruleQueue);
            SetConditionalFormatTypeToQueue(ruleQueue, type);
            SetConditionalFormatStyleToQueue(ruleQueue, style);
            ruleQueue.SetAction("setRanges", new URange[] { range }).SetAction("build");

            await inner.ResolveActionAsync("addConditionalFormat", Snapshot, ruleQueue.ToArray());
        });
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
        var queue = CreateQueue();
        UseSheet(queue);
        SetConditionalFormatTypeToQueue(queue, type);
        SetConditionalFormatStyleToQueue(queue, style);
        queue.SetAction("setRanges", ranges).SetAction("build");

        await UniverJS.ResolveActionAsync("addConditionalFormat", queue.ToArray());
    }

    /// <summary>
    /// Deletes the specified conditional format by its Id
    /// </summary>
    /// <param name="idRule">Conditional format id</param>
    /// <returns></returns>
    public async Task DeleteConditionalFormat(string idRule)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("deleteConditionalFormattingRule", idRule).ResolveQueueAsync();
    }

    /// <summary>
    /// Clears all conditional formats in the page
    /// </summary>
    /// <returns></returns>
    public async Task ClearConditionalFormats()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("clearConditionalFormatRules").ResolveQueueAsync();
    }

    /// <summary>
    /// Gets all conditional formats in the page
    /// </summary>
    /// <returns>An array of each conditional format in the page</returns>
    public async Task<UConditionalFormatRule[]> GetAllConditionalFormats()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getConditionalFormattingRules").ResolveQueueAsync<UConditionalFormatRule[]>();
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
        UConditionalFormatRule rule = rules.FirstOrDefault(r => r.cfId.Equals(idRule));
        if (string.IsNullOrEmpty(rule.cfId))
            throw new UniverException($"cfId does not exist: {idRule}");

        rule.ranges = [range];
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("setConditionalFormattingRule", idRule, rule).ResolveQueueAsync();
    }

    void SetConditionalFormatTypeToQueue(UniverQueue queue, UConditionType type)
    {
        queue.SetAction("newConditionalFormattingRule");

        if (type.IsIconSet || type.IsColorScale || type.IsDataBar)
            return;

        if (type.WhenCellEmpty)
        {
            queue.SetAction("whenCellEmpty");
            return;
        }

        if (type.WhenCellNotEmpty)
        {
            queue.SetAction("whenCellNotEmpty");
            return;
        }

        if (type.WhenDate != null)
        {
            queue.SetAction("whenDate", type.WhenDate.Value.ToString());
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenFormulaSatisfied))
        {
            queue.SetAction("whenFormulaSatisfied", type.WhenFormulaSatisfied);
            return;
        }

        if (type.WhenNumberEqualTo != null)
        {
            double value = type.WhenNumberEqualTo.Value;
            queue.SetAction("whenNumberEqualTo", value);
            return;
        }

        if (type.WhenNumberGreaterThan != null)
        {
            double value = type.WhenNumberGreaterThan.Value;
            queue.SetAction("whenNumberGreaterThan", value);
            return;
        }

        if (type.WhenNumberGreaterThanOrEqual != null)
        {
            double value = type.WhenNumberGreaterThanOrEqual.Value;
            queue.SetAction("whenNumberGreaterThanOrEqual", value);
            return;
        }

        if (type.WhenNumberInBetween != null)
        {
            (double starts, double ends) value = type.WhenNumberInBetween.Value;
            queue.SetAction("whenNumberBetween", value.starts, value.ends);
            return;
        }

        if (type.WhenNumberLessThan != null)
        {
            double value = type.WhenNumberLessThan.Value;
            queue.SetAction("whenNumberLessThan", value);
            return;
        }

        if (type.WhenNumberLessThanOrEqual != null)
        {
            double value = type.WhenNumberLessThanOrEqual.Value;
            queue.SetAction("whenNumberLessThanOrEqual", value);
            return;
        }

        if (type.WhenNumberNotBetween != null)
        {
            (double starts, double ends) value = type.WhenNumberNotBetween.Value;
            queue.SetAction("whenNumberNotBetween", value.starts, value.ends);
            return;
        }

        if (type.WhenNumberNotEqual != null)
        {
            double value = type.WhenNumberNotEqual.Value;
            queue.SetAction("whenNumberNotEqual", value);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextContains))
        {
            queue.SetAction("whenTextContains", type.WhenTextContains);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextDoesNotContain))
        {
            queue.SetAction("whenTextDoesNotContain", type.WhenTextDoesNotContain);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextEndsWith))
        {
            queue.SetAction("whenTextEndsWith", type.WhenTextEndsWith);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextEqualTo))
        {
            queue.SetAction("whenTextEqualTo", type.WhenTextEqualTo);
            return;
        }

        if (!string.IsNullOrEmpty(type.WhenTextStartsWith))
        {
            queue.SetAction("whenTextStartsWith", type.WhenTextStartsWith);
            return;
        }
    }

    void SetConditionalFormatStyleToQueue(UniverQueue queue, UConditionFormatStyle style)
    {
        if (style.Average != null)
            queue.SetAction("setAverage", style.Average.Value);

        if (!string.IsNullOrEmpty(style.Background))
            queue.SetAction("setBackground", style.Background);

        if (style.ColorScale != null)
            queue.SetAction("setColorScale", style.ColorScale.Value.config);

        if (style.DataBar != null)
            queue.SetAction("setDataBar", style.DataBar.Value);

        if (style.DuplicateValues)
            queue.SetAction("setDuplicateValues");

        if (!string.IsNullOrEmpty(style.FontColor))
            queue.SetAction("setFontColor", style.FontColor);

        if (style.IconSet != null)
            queue.SetAction("setIconSet", new UIconSetCond(style.IconSet.Value.isShowValue, style.IconSet.Value.config));

        if (style.IsBold != null)
            queue.SetAction("setBold", style.IsBold.Value);

        if (style.Italic != null)
            queue.SetAction("setItalic", style.Italic.Value);

        if (style.Rank != null)
            queue.SetAction("setRank", new URankConfig(style.Rank.Value.isBottom, style.Rank.Value.isPercent, style.Rank.Value.value));

        if (style.Strikethrough != null)
            queue.SetAction("setStrikethrough", style.Strikethrough.Value);

        if (style.Underline != null)
            queue.SetAction("setUnderline", style.Underline.Value);

        if (style.UniqueValues)
            queue.SetAction("setUniqueValues");
    }

    /// <inheritdoc/>
    protected override ConditionalFormatCommands Clone(USpreadsheetSnapshot context)
        => new ConditionalFormatCommands(context, UniverJS);
}
