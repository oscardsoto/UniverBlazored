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
    /// Manifest of presets that UniverSpreadsheetResources preloads from CDN.
    /// Individual components receive their own initialization config via the InitConfig parameter;
    /// presets enabled there must be preloaded here.
    /// </summary>
    public UniverInit InitialConfig { get; set; } = new();

    /// <summary>
    /// Maximum time (in seconds) to wait for the Univer scripts to become ready before initializing
    /// </summary>
    public double ScriptLoadTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Interval (in milliseconds) between readiness polls while waiting for the Univer scripts
    /// </summary>
    public int ScriptLoadPollIntervalMs { get; set; } = 200;
}