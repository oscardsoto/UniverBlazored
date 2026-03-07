using System.Text.Json.Serialization;
using UniverBlazored.Generic.Data;

/// <summary>
/// Univer configuration object for fonts
/// </summary>
public struct UniverFontsConfig
{
    /// <summary>
    /// List to fonts to add
    /// </summary>
    /// <returns></returns>
    public List<UniverFont> list { get; set; } = new();

    /// <summary>
    /// Whether to override the default font list, defaults to false
    /// </summary>
    /// <value></value>
    [JsonPropertyName("override")]
    public bool Override { get; set; } = false;

    public UniverFontsConfig()
    {
        list = new();
        Override = false;
    }

    /// <summary>
    /// Univer configuration object for fonts
    /// </summary>
    /// <param name="list">List to fonts to add</param>
    /// <param name="override">Whether to override the default font list</param>
    public UniverFontsConfig(List<UniverFont> list, bool @override = false)
    {
        this.list = list;
        Override = @override;
    }
}