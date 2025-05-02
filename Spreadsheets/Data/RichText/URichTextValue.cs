namespace UniverBlazored.Spreadsheets.Data.RichText;

//RichTextBuilder
//https://github.com/dream-num/univer/blob/4eed277c4a9996e1c81f5c92fb9b58bbea6c0290/packages/core/src/docs/data-model/rich-text-builder.ts#L1689

/// <summary>
/// Fragment of IDocumentBody, from Univer, to create and get all Rich Texts (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts#L121)
/// </summary>
public struct URichTextValue
{
    /// <summary>
    /// Data in the rich text
    /// </summary>
    public string dataStream { get; set; } = "";

    /// <summary>
    /// Text configuration for each style
    /// </summary>
    public List<UTextRun> textRuns { get; set; } = new();

    /// <summary>
    /// Fragment of IDocumentBody, from Univer, to create and get all Rich Texts (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts#L121)
    /// </summary>
    public URichTextValue() { }

    /// <summary>
    /// Fragment of IDocumentBody, from Univer, to create and get all Rich Texts (See doc: https://github.com/dream-num/univer/blob/dev/packages/core/src/types/interfaces/i-document-data.ts#L121)
    /// </summary>
    /// <param name="richText">Text inside the rich text</param>
    public URichTextValue(string richText) => dataStream = richText;

    
}