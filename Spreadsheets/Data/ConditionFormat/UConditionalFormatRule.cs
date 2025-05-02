using System.Text.Json;
using System.Text.Json.Nodes;
using UniverBlazored.Spreadsheets.Data.Styles;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IConditionlFormattingRule, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts)
/// </summary>
public struct UConditionalFormatRule
{
    /// <summary>
    /// Ranges to aply the conditional format
    /// </summary>
    /// <value></value>
    public List<URange> ranges { get; set; }

    /// <summary>
    /// Id for the conditional format
    /// </summary>
    /// <value></value>
    public string cfId { get; set; }

    /// <summary>
    /// True if only apply to 1 element to stop
    /// </summary>
    /// <value></value>
    public bool stopIfTrue { get; set; }

    /// <summary>
    /// Rule object for the conditional format. See doc for every single combination (This is serialized as a JsonObject to reduce code)
    /// </summary>
    /// <value></value>
    public JsonObject rule { get; set; }

    // Ignore only this
    bool RuleNotDeclared() => (rule == null) || rule["type"] == null;

    /// <summary>
    /// Returns the type of conditional format inside "rule" JsonObject. Returns null if *rule* or the "type" key doesnt exist
    /// </summary>
    /// <returns></returns>
    public ECFRuleType? GetTypeConditionalFormat()
    {
        if (RuleNotDeclared())
            return null;
            
        var type = rule["type"]?.ToString();
        return type == null ? null : Enum.Parse<ECFRuleType>(type);
    }

    /// <summary>
    /// Returns the SubType of the conditional format inside "rule" JsonObject. Only applicable for "highlightCell" type
    /// </summary>
    /// <returns></returns>
    public ECFSubRuleType? GetSubType()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type is not ECFRuleType.highlightCell)
            return null;

        var subType = rule["subType"]?.ToString();
        return subType == null ? null : Enum.Parse<ECFSubRuleType>(subType);
    }

    /// <summary>
    /// Returns the operator at the root node in "rule" JsonObject. Only applicable for "highlightCell" type and: "text", "timePeriod", "number" and "average" SubTypes
    /// </summary>
    /// <returns></returns>
    public ECFOperators? GetOperator()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type.Value is not ECFRuleType.highlightCell)
            return null;

        var subType = GetSubType();
        if ((subType == null) || (subType.Value is not ECFSubRuleType.text && subType.Value is not ECFSubRuleType.timePeriod && subType.Value is not ECFSubRuleType.number && subType.Value is not ECFSubRuleType.average))
            return null;

        var _operator = rule["operator"]?.ToString();
        return _operator == null ? null : Enum.Parse<ECFOperators>(_operator);
    }

    /// <summary>
    /// Returns the style object at the root node in "rule" JsonObject. Only applicable for "highlightCell" type
    /// </summary>
    /// <returns></returns>
    public IStyleBase? GetStyleUsed()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type.Value is not ECFRuleType.highlightCell)
            return null;

        return rule["style"].Deserialize<UGenericStyle>();
    }

    /// <summary>
    /// Returns the configuration object for the Data Bar Conditional Format. Only applicable for "dataBar" type
    /// </summary>
    /// <returns></returns>
    public UDataBarConfig? GetDataBarConfig()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type.Value is not ECFRuleType.dataBar)
            return null;

        return rule["config"].Deserialize<UDataBarConfig>();
    }

    /// <summary>
    /// Returns the configuration object for the Color Scale Conditional Format. Only applicable for "colorScale" type
    /// </summary>
    /// <returns></returns>
    public UColorScaleConfig[] GetColorScaleConfigs()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type.Value is not ECFRuleType.colorScale)
            return null;

        return rule["config"].Deserialize<UColorScaleConfig[]>() ?? [];
    }

    /// <summary>
    /// Returns the ocnfiguration object for the Icon Set Conditional Format. Only applicable for "iconSet" type
    /// </summary>
    /// <returns></returns>
    public UIconSetConfig[] GetIconSetConfigs()
    {
        var type = GetTypeConditionalFormat();
        if ((type == null) || type.Value is not ECFRuleType.iconSet)
            return null;

        return rule["config"].Deserialize<UIconSetConfig[]>() ?? [];
    }
}