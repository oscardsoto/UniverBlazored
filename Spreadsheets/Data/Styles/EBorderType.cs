namespace UniverBlazored.Spreadsheets.Data.Styles;

/// <summary>
/// BorderType enum, from Univer's object BorderData, to specifý in wich border will apply the changes (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IBorderData)
/// </summary>
public enum EBorderType
{
    /// <summary>
    /// All borders
    /// </summary>
    ALL,

    /// <summary>
    /// Bottom-Left, Top-Right 
    /// </summary>
    BLTR,

    /// <summary>
    /// Bottom border
    /// </summary>
    BOTTOM,

    /// <summary>
    /// All horizontal borders for the range
    /// </summary>
    HORIZONTAL,

    /// <summary>
    /// All borders inside the range
    /// </summary>
    INSIDE,

    /// <summary>
    /// All borders from the left for the range
    /// </summary>
    LEFT,

    /// <summary>
    /// Middle-Left Top-Right, Bottom-Center Top-Right
    /// </summary>
    MLTR_BCTR,

    /// <summary>
    /// No borders
    /// </summary>
    NONE,
    
    /// <summary>
    /// All borders outside the range
    /// </summary>
    OUTSIDE,

    /// <summary>
    /// All borders from the right for the page
    /// </summary>
    RIGHT,

    /// <summary>
    /// Top-Left Bottom-Center, Top-Left Middle-Right
    /// </summary>
    TLBC_TLMR,

    /// <summary>
    /// Top-Left Bottom-Right
    /// </summary>
    TLBR,

    /// <summary>
    /// Top-Left Bottom-Right, Top-Left Bottom-Center, Top-Left Middle-Right
    /// </summary>
    TLBR_TLBC_TLMR,

    /// <summary>
    /// All top borders from the range
    /// </summary>
    TOP,

    /// <summary>
    /// All vertical borders for the range
    /// </summary>
    VERTICAL
}