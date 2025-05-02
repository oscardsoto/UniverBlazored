namespace UniverBlazored.Generic;

/// <summary>
/// Config object for UniverJS
/// </summary>
public struct UniverInit
{
    /// <summary>
    /// Enable Short Preset (True by default)
    /// </summary>
    public bool hasShort { get; set; } = true;

    /// <summary>
    /// Enable DataValidation Preset (True by default)
    /// </summary>
    public bool hasDataValidation { get; set; } = true;

    /// <summary>
    /// Enable Filter Preset (True by default)
    /// </summary>
    public bool hasFilter { get; set; } = true;

    /// <summary>
    /// Enable Conditional Formats Preset (True by default)
    /// </summary>
    public bool hasConditionalFormatting { get; set; } = true;

    /// <summary>
    /// Enable HyperLink Preset (True by default)
    /// </summary>
    public bool hasHyperLink { get; set; } = true;

    /// <summary>
    /// Enable drawing Preset (True by default)
    /// </summary>
    public bool hasDrawing { get; set; } = true;

    /// <summary>
    /// Enable Thread Comment Preset (True by default)
    /// </summary>
    public bool hasThreadComment { get; set; } = true;

    /// <summary>
    /// Enable Crosshair Plugin (False by default)
    /// </summary>
    public bool hasCrosshair { get; set; } = false;

    /// <summary>
    /// Enable Watermark Plugin (False by default)
    /// </summary>
    public bool hasWatermark { get; set; } = false;

    /// <summary>
    /// Text to show in the watermark of each sheet
    /// </summary>
    public string watermarkLabel { get; set; } = "";

    /// <summary>
    /// Name of the first sheet created after initialized
    /// </summary>
    public string newSheetName { get; set; } = "UniverSheet";

    /// <summary>
    /// Id of the div that will execute Univer (default is "uXlsxComp")
    /// </summary>
    public string idDiv { get; set; } = "uXlsxComp";

    /// <summary>
    /// Config object for UniverJS
    /// </summary>
    public UniverInit() { }

    /// <summary>
    /// Set the value for the div's Id to execute Univer
    /// </summary>
    /// <param name="newId"></param>
    public void SetNewIdDiv(string newId) => idDiv = newId;
}