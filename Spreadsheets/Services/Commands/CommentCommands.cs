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

    public async Task<UniverComment> GetComment(bool delete = false)
    {
        return await ExecuteAtomically(async inner =>
        {
            var q = new UniverQueue(inner, Snapshot.ToContext());
            UseRange(q);
            var position = await q.SetAction("getComment").SetAction("getCommentData").ResolveQueueAsync<UniverComment>();
            if (delete)
            {
                var dq = new UniverQueue(inner, Snapshot.ToContext());
                UseRange(dq);
                await dq.SetAction("getComment").SetAction("delete").ResolveQueueAsync<UniverComment>();
            }
            return position;
        });
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
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("clearComments").ResolveQueueAsync();
    }

    /// <inheritdoc/>
    protected override CommentCommands Clone(USpreadsheetSnapshot context)
        => new CommentCommands(context, UniverJS);
}