namespace UniverBlazored.Generic;

/// <summary>
/// Univer avaiable language
/// </summary>
public class UniversLanguage
{
    private UniversLanguage(string value) => Value = value;

    /// <summary>
    /// Language's value
    /// </summary>
    /// <returns></returns>
    public string Value { get; set; }

    /// <summary>
    /// English
    /// </summary>
    public static UniversLanguage ENGLISH => new UniversLanguage("en-US");

    /// <summary>
    /// Russian
    /// </summary>
    public static UniversLanguage RUSSIAN => new UniversLanguage("ru-RU");

    /// <summary>
    /// Simple Chinese
    /// </summary>
    public static UniversLanguage SIMPLE_CHINESE => new UniversLanguage("zh-CN");

    /// <summary>
    /// Vietnamese
    /// </summary>
    public static UniversLanguage VIETNAMESE => new UniversLanguage("vi-VN");

    /// <summary>
    /// Persian
    /// </summary>
    public static UniversLanguage FARSI => new UniversLanguage("fa-IR");

    /// <summary>
    /// Japanese
    /// </summary>
    public static UniversLanguage JAPANESE => new UniversLanguage("ja-JP");

    /// <summary>
    /// Korean
    /// </summary>
    public static UniversLanguage KOREAN => new UniversLanguage("ko-KR");

    /// <summary>
    /// Spanish
    /// </summary>
    public static UniversLanguage SPANISH_SPAIN => new UniversLanguage("es-ES");

    /// <summary>
    /// Catalan
    /// </summary>
    public static UniversLanguage CATALAN => new UniversLanguage("ca-ES");

    /// <summary>
    /// Return the value from the language
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Return the value from the language
    /// </summary>
    /// <returns></returns>
    public static implicit operator string(UniversLanguage language) => language.Value;
}
