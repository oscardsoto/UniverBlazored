using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Styles;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Range operations (sort, merge, filters)
/// </summary>
public class RangeCommands : USpreadsheetCommandBase<RangeCommands>
{
    /// <summary>
    /// Commands focused on Sheet Range operations (sort, merge, filters)
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <returns></returns>
    public RangeCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Sort selected range by ascending
    /// </summary>
    /// <param name="columns">Columns to be sorted</param>
    /// <returns>This, for chaining</returns>
    public async Task SortAscending(params int[] columns)
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction("sort", columns).ResolveQueueAsync();
    }

    /// <summary>
    /// Sort selected range by ascending or descending
    /// </summary>
    /// <param name="sorts">(column: Column to be sorted, ascending: True if the col will be sorted by ascending)</param>
    /// <returns>This, for chaining</returns>
    public async Task SortAscending(params (int column, bool ascending)[] sorts)
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        List<object> sort = [];
        foreach (var s in sorts)
            sort.Add(new
            {
                column = s.column,
                ascending = s.ascending
            });
        await queue.SetAction("sort", sort.ToArray()).ResolveQueueAsync();
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
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction(action, defaultMerge).ResolveQueueAsync();
    }

    /// <summary>
    /// Unmerge all cells in range
    /// </summary>
    /// <returns></returns>
    public async Task BreakMerge()
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction("breakApart").ResolveQueueAsync();
    }

    /// <summary>
    /// Get all merges in active page
    /// </summary>
    /// <returns>All ranges that correspond to each merge</returns>
    public async Task<URange[]> GetAllMerges()
    {
        return await UniverJS.ResolveActionAsync<URange[]>("getAllMerges", this.Snapshot);
    }

    /// <summary>
    /// True if the range has cells that overlap a merged cell 
    /// </summary>
    public async Task<bool> RangeIsPartOfMerge()
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        return await queue.SetAction("isPartOfMerge").ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Create a filter for the selected range (on the active page)
    /// </summary>
    /// <returns></returns>
    public async Task CreateFilter()
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        await queue.SetAction("createFilter").ResolveQueueAsync();
    }

    /// <summary>
    /// Returns true if the active page has a filter on it
    /// </summary>
    /// <returns></returns>
    public async Task<bool> HasFilter()
    {
        return await UniverJS.ResolveActionAsync<bool>("hasFilter", this.Snapshot);
    }

    /// <summary>
    /// Gets the range's filter for the active page
    /// </summary>
    /// <returns>URange object with the range of the filter</returns>
    public async Task<URange?> GetFilter()
    {
        var queue = new UniverQueue(UniverJS);
        UseSheet(queue);
        return await queue.SetAction("getFilter").SetAction("getRange").SetAction("getRange").ResolveQueueAsync<URange?>();
    }

    /// <summary>
    /// Removes the filter for the active page
    /// </summary>
    /// <returns></returns>
    public async Task RemoveFilter()
    {
        var queue = new UniverQueue(UniverJS);
        UseSheet(queue);
        await queue.SetAction("getFilter").SetAction("remove").ResolveQueueAsync();
    }

    /// <inheritdoc/>
    protected override RangeCommands Clone(USpreadsheetSnapshot context)
        => new RangeCommands(context, UniverJS);
}