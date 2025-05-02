namespace UniverBlazored.Spreadsheets.Data.RichText;

/// <summary>
/// ITextRun, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts#L333)
/// </summary>
public struct UTextRun
{
    /// <summary>
    /// Start for styling
    /// </summary>
    public int st { get; set; }

    /// <summary>
    /// End styling
    /// </summary>
    public int ed { get; set; }

    /// <summary>
    /// Style Id
    /// </summary>
    public string sId { get; set; }

    /// <summary>
    /// Text Style
    /// </summary>
    /// <value></value>
    public UTextStyle ts { get; set; }    
}