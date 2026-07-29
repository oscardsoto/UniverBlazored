using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Images;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Images
/// </summary>
public class ImageCommands : USpreadsheetCommandBase<ImageCommands>
{
    /// <summary>
    /// Commands focused on Sheet Images
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <param name="snapshot"></param>
    public ImageCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    public async Task AddImage(string urlImage)
    {
        await ExecuteAtomically(async inner =>
        {
            var rangeQueue = new UniverQueue(inner, Snapshot.ToContext());
            UseRange(rangeQueue);
            URange activeRange = await rangeQueue.SetAction("getRange").ResolveQueueAsync<URange>();
            var sheetQueue = new UniverQueue(inner, Snapshot.ToContext());
            UseSheet(sheetQueue);
            await sheetQueue.SetAction("insertImage", urlImage, activeRange.startColumn, activeRange.startRow).ResolveQueueAsync();
        });
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
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("insertImage", urlImage, col, row, rowOffset, colOffset).ResolveQueueAsync();
    }

    /// <summary>
    /// Set images on the active page
    /// </summary>
    /// <param name="images">Full properties of each image that will be added</param>
    /// <returns></returns>
    public async Task AddImage(params UImage[] images)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("insertImages", images).ResolveQueueAsync();
    }

    /// <summary>
    /// Return all images on the active page (Use this if all images combined are less than 33KB, otherwise will fail in Exception)
    /// </summary>
    /// <returns></returns>
    public async Task<UImage[]> GetImages()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getImages").ResolveQueueAsync<UImage[]>();
    }

    /// <summary>
    /// Return all images ids in the active sheet
    /// </summary>
    /// <returns></returns>
    public async Task<string[]> GetImagesId() => await UniverJS.ResolveActionAsync<string[]>("getImagesId", this.Snapshot);

    /// <summary>
    /// Return the image by id on the active page (Use this if the image is less than 33KB, otherwise will fail in Exception.)
    /// </summary>
    /// <param name="id">Image Id</param>
    /// <param name="withSource">False if you dont want to get the image source (if the image is more than 33KB)</param>
    public async Task<UImage> GetImage(string id, bool withSource = true) => await UniverJS.ResolveActionAsync<UImage>("getImageById", this.Snapshot, id, withSource);

    public async Task<string> GetImageSource(string id)
    {
        return await ExecuteAtomically(async inner =>
        {
            int counterLength = 0;
            int maxChunk = 20000;
            string source = "", chunk = "";
            do
            {
                var q = new UniverQueue(inner, Snapshot.ToContext());
                UseSheet(q);
                chunk = await q.SetAction("getImageById", id)
                             .SetAction("toBuilder")
                             .SetAction("getSource")
                             .SetAction("slice", counterLength, counterLength + maxChunk)
                             .ResolveQueueAsync<string>();
                counterLength += maxChunk;
                source += chunk;
            }
            while (!string.IsNullOrEmpty(chunk));
            return source;
        });
    }

    public async Task DeleteImagesById(params string[] ids)
    {
        await ExecuteAtomically(async inner =>
        {
            foreach (string id in ids)
            {
                var q = new UniverQueue(inner, Snapshot.ToContext());
                UseSheet(q);
                await q.SetAction("getImageById", id).SetAction("remove").ResolveQueueAsync();
            }
        });
    }

    /// <summary>
    /// Delete all selected images on the active sheet
    /// </summary>
    /// <param name="images">Selected images to delete (must be full objects)</param>
    public async Task DeleteImages(params UImage[] images)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("deleteImages", images).ResolveQueueAsync();
    }

    /// <inheritdoc/>
    protected override ImageCommands Clone(USpreadsheetSnapshot context)
        => new ImageCommands(context, UniverJS);
}
