using System.Text.Json.Serialization;
using UniverBlazored.Spreadsheets.Data.Styles;

namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// IAverageHighlightCell, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L74)
/// </summary>
public struct UAverageHighlightCell
{
    /// <summary>
    /// ECFRuleType value
    /// </summary>
    /// <value></value>
    public string type { get; set; } = "highlightCell";

    /// <summary>
    /// ECFSubRuleType value
    /// </summary>
    public string subType { get; set; } = "average";

    /// <summary>
    /// Style applied
    /// </summary>
    public IStyleBase style { get; set; }

    /// <summary>
    /// ECFOperators value
    /// </summary>
    [JsonPropertyName("operator")]
    public string Operator { get; set; } = "";

    /// <summary>
    /// IAverageHighlightCell, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/models/type.ts#L74)
    /// </summary>
    public UAverageHighlightCell() 
    {
        // UStyleData extends IStyleBase
        style = new UStyleData();
    }

    /// <summary>
    /// Set "operator" value from enum
    /// </summary>
    /// <param name="op"></param>
    public void SetOperator(ECFOperators op)
    {
        if (!ECFOperatorGropus.NumberOperators.HasFlag(op))
            throw new UniverException($"{op} is not a Number Operator.");

        Operator = op.ToString();
    }
}

