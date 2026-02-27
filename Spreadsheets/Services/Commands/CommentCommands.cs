using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Comments
/// </summary>
public class CommentCommands : USpreadsheetCommandBase<CommentCommands>
{
    /// <summary>
    /// Commands focused on Sheet Comments
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <param name="snapshot"></param>
    /// <returns></returns>
    public CommentCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Insert a comment in the first cell on the active range
    /// </summary>
    /// <param name="comment">Comment to insert</param>
    /// <returns></returns>
    public async Task InsertComment(UniverComment comment) => await UniverJS.ResolveActionAsync("insertComment", Snapshot, comment);

    /// <summary>
    /// Returns the first (root) comment at the first cell in the active  range
    /// </summary>
    /// <param name="delete">True for deleting the selected comment</param>
    /// <returns></returns>
    public async Task<UniverComment> GetComment(URange? cell = null, bool delete = false)
    {
        var queue = new UniverQueue(UniverJS);
        UseRange(queue);
        var position = await queue.SetAction("getComment").SetAction("getCommentData").ResolveQueueAsync<UniverComment>();
        if (delete)
        {
            UseRange(queue);
            await queue.SetAction("getComment").SetAction("delete").ResolveQueueAsync<UniverComment>();
        }
        return position;
    }

    /// <summary>
    /// Return all comments in the sheet
    /// </summary>
    /// <returns></returns>
    public async Task<UniverComment[]> GetComments() => await UniverJS.ResolveActionAsync<UniverComment[]>("getAllComments", Snapshot);

    /// <summary>
    /// Delete all comments in the page
    /// </summary>
    /// <returns></returns>
    public async Task ClearComments()
    {
        var queue = new UniverQueue(UniverJS);
        UseSheet(queue);
        await queue.SetAction("clearComments").ResolveQueueAsync();
    }

    /// <inheritdoc/>
    protected override CommentCommands Clone(USpreadsheetSnapshot context)
        => new CommentCommands(context, UniverJS);
}