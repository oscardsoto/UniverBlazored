using System.Text.Json.Serialization;

namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IIconSet "config" property, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L96)
/// </summary>
public struct UIconSetConfig
{
    /// <summary>
    /// Icon Id (int in string value)
    /// </summary>
    /// <value></value>
    public string iconId { get; set; } = "";

    /// <summary>
    /// Operator value for the icon set ()
    /// </summary>
    /// <value></value>
    [JsonPropertyName("operator")]
    public string Operator { get; set; } = "";

    /// <summary>
    /// Value to evaluate
    /// </summary>
    public UValueConfig value { get; set; } = new();

    /// <summary>
    /// Icon Type to show (Use SetIconType to change it correctly)
    /// </summary>
    /// <value></value>
    public string iconType { get; set; } = "";

    /// <summary>
    /// IIconSet "config" property, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L96)
    /// </summary>
    public UIconSetConfig() {  }

    /// <summary>
    /// Set "operator" value via enum
    /// </summary>
    /// <param name="op"></param>
    public void SetOperator(ECFOperators op)
    {
        if (!ECFOperatorGropus.NumberOperators.HasFlag(op))
            throw new UniverException($"{op} is not a Number Operator.");

        Operator = op.ToString();
    }

    /// <summary>
    /// Return the operator for the icon set (Enumerator)
    /// </summary>
    /// <returns></returns>
    public ECFOperators GetOperator() => Enum.Parse<ECFOperators>(Operator);

    /// <summary>
    /// Set "iconType" value via enum
    /// </summary>
    /// <param name="icon"></param>
    public void SetIconType(EIconType icon) => iconType = icon.ToString().Replace("I_", "");

    /// <summary>
    /// Return the Icon Type (Enumerator)
    /// </summary>
    /// <returns></returns>
    public EIconType? GetIconType() => Enum.Parse<EIconType>("I_" + iconType);
}