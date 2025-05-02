using System.Text.Json.Serialization;
using UniverBlazored.Spreadsheets.Data.RichText;

namespace UniverBlazored.Generic.Data;

/// <summary>
/// Univer Thread Comment component's data
/// </summary>
public class UniverComment
{
    /// <summary>
    /// Workbook Id
    /// </summary>
    /// <value></value>
    public string unitId { get; set; } = "";

    /// <summary>
    /// Worksheet Id
    /// </summary>
    /// <value></value>
    public string subUnitId { get; set; } = "";

    /// <summary>
    /// Comment Id
    /// </summary>
    /// <value></value>
    public string id { get; set; } = "";

    /// <summary>
    /// Thread Id
    /// </summary>
    /// <value></value>
    public string threadId { get; set; } = "";

    /// <summary>
    /// DateTime (as string) when the comment was created
    /// </summary>
    /// <value></value>
    public string dT { get; set; } = "";

    /// <summary>
    /// User's Id that create the commment
    /// </summary>
    /// <value></value>
    public string personId { get; set; } = "";

    /// <summary>
    /// Location in cell in A1 notation
    /// </summary>
    [JsonPropertyName("ref")]
    public string reference { get; set; } = "";

    // attachments is ignored

    /// <summary>
    /// Comment's content
    /// </summary>
    /// <returns></returns>
    public URichTextValue text { get; set; } = new();

    /// <summary>
    /// Univer Thread Comment component's data
    /// </summary>
    public UniverComment() { }

    /// <summary>
    /// Sets a plain text in the comment
    /// </summary>
    /// <param name="text">Text in the comment</param>
    public void SetText(string text) => this.text = new(text);

    /// <summary>
    /// Sets the DateTime for the comment
    /// </summary>
    /// <param name="dt">Date for the comment (leave null if the Date is Now)</param>
    public void SetDateTime(DateTime? dt = null)
    {
        if (dt == null)
        {
            dT = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return;
        }

        dT = dt.Value.ToString("dd/MM/yyyy HH:mm");
    }
}