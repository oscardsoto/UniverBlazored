namespace UniverBlazored.Generic;

/// <summary>
/// Configuration for univer to initialize
/// </summary>
public class UniverConfig
{
    /// <summary>
    /// Univer's version
    /// </summary>
    public string Version { get; set; } = "0.5.5";

    /// <summary>
    /// Univer's language
    /// </summary>
    public UniversLanguage Language { get; set; } = UniversLanguage.ENGLISH;

    /// <summary>
    /// Config object that's sended to Univer to initialize the component
    /// </summary>
    public UniverInit InitialConfig { get; set; } = new();
}