namespace UniverBlazored.Generic;

/// <summary>
/// Configuration for univer to initialize
/// </summary>
public class UniverConfig
{
    private Version versionLimit = new Version("0.15.1");

    private string _version = "";

    /// <summary>
    /// Univer's version
    /// </summary>
    public string Version
    {
        get
        {
            return _version;
        }
        set
        {
            var versionInput = new Version(value);
            if (versionInput < versionLimit)
                throw new ArgumentException($"Version must be greater than or equal to {versionLimit}. Your version: {value}");

            _version = value;
        }
    }

    /// <summary>
    /// Univer's language
    /// </summary>
    public UniversLanguage Language { get; set; } = UniversLanguage.ENGLISH;

    /// <summary>
    /// Config object that's sended to Univer to initialize the component
    /// </summary>
    public UniverInit InitialConfig { get; set; } = new();
}