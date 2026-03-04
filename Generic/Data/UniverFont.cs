namespace UniverBlazored.Generic.Data;

/// <summary>
/// Font definition for Univer
/// </summary>
public struct UniverFont
{
    /// <summary>
    /// The unique identifier of the font, usually the preferred value for CSS font-family.
    /// </summary>
    /// <value></value>
    public string value { get; set; }

    /// <summary>
    /// The translation key for internationalization. If this key is not specified when the Univer instance is created, the passed value is displayed directly.
    /// </summary>
    /// <value></value>
    public string label { get; set; }

    /// <summary>
    /// The category of the font, used for UI grouping (optional). Use "UFontCategory" for predefined categories or create custom ones as needed.
    /// </summary>
    /// <value></value>
    public string category { get; set; }

    /// <summary>
    /// Font definition for Univer (sets category to SANS_SERIF by default)
    /// </summary>
    /// <param name="value">The unique identifier of the font</param>
    /// <param name="label">The translation key for internationalization</param>
    public UniverFont(string value, string label)
    {
        this.value = value;
        this.label = label;
        this.category = UFontCategory.SANS_SERIF;
    }

    /// <summary>
    /// Font definition for Univer
    /// </summary>
    /// <param name="value">The unique identifier of the font</param>
    /// <param name="label">The translation key for internationalization</param>
    /// <param name="category"></param>
    public UniverFont(string value, string label, UFontCategory category)
    {
        this.value = value;
        this.label = label;
        this.category = category;
    }
}

/// <summary>
/// Font category, used for UI grouping (optional).
/// </summary>
public class UFontCategory
{
    private UFontCategory(string value) => Value = value;

    /// <summary>
    /// Font category's value
    /// </summary>
    /// <returns></returns>
    public string Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UFontCategory SANS_SERIF { get; } = new UFontCategory("sans-serif");

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UFontCategory SERIF { get; } = new UFontCategory("serif");

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UFontCategory MONOSPACE { get; } = new UFontCategory("monospace");

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UFontCategory DISPLAY { get; } = new UFontCategory("display");

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static UFontCategory HANDWRITING { get; } = new UFontCategory("handwriting");

    /// <summary>
    /// Return the string representation of the font category
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value;

    /// <summary>
    /// Return the value from the font category
    /// </summary>
    /// <returns></returns>
    public static implicit operator string(UFontCategory category) => category.Value;
}