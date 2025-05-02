namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IValueConfig, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L30)
/// </summary>
public struct UValueConfig
{
    /// <summary>
    /// ECFValueType value
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// Value to evaluate (in JsonElement format, if extracted directly from Univer)
    /// </summary>
    public object value { get; set; }

    /// <summary>
    /// IValueConfig, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L30)
    /// </summary>
    public UValueConfig()
    {
        type = ECFValueType.num.ToString();
        value = 0;
    }

    /// <summary>
    /// IValueConfig, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L30)
    /// </summary>
    /// <param name="type">Value Type</param>
    /// <param name="value">Content for the value (string, or double only)</param>
    public UValueConfig(ECFValueType type, string value)
    {
        this.type = type.ToString();
        this.value = value;
    }

    /// <summary>
    /// IValueConfig, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L30)
    /// </summary>
    /// <param name="type">Value Type</param>
    /// <param name="value">Content for the value (string, or double only)</param>
    public UValueConfig(ECFValueType type, double value)
    {
        this.type   = type.ToString();
        this.value = value;
    }

    /// <summary>
    /// Set "type" value from enum
    /// </summary>
    /// <param name="type">Value Type</param>
    public void SetType(ECFValueType type) => this.type = type.ToString();

    /// <summary>
    /// Returns the type value (in Enum)
    /// </summary>
    /// <returns></returns>
    public ECFValueType? GetValueType()
    {
        if (string.IsNullOrEmpty(type))
            return null;

        return Enum.Parse<ECFValueType>(type);
    }
}
