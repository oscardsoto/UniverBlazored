namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// Sets where the conditional format needs to trigger
/// </summary>
public struct UConditionType
{
    /// <summary>
    /// Sets the conditional formatting rule to fire when the cell is empty. (False to ignore)
    /// </summary>
    public bool WhenCellEmpty { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the cell is not empty. (False to ignore)
    /// </summary>
    public bool WhenCellNotEmpty { get; set; }

    // Ignore this
    private ECFOperators? whenDate;

    /// <summary>
    /// Highlight when the date is in a time period. (null to ignore)
    /// </summary>
    public ECFOperators? WhenDate
    {
        get { return whenDate; }
        set
        {
            if (value == null)
            {
                whenDate = null;
                return;
            }

            if (!ECFOperatorGropus.TimePeriodOperators.HasFlag(value))
            {
                whenDate = value;
                return;
            }

            throw new UniverException($"{value} is not a Time Period Operator");
        }
    }

    /// <summary>
    /// Sets a conditional formatting rule to fire when a given formula evaluates to true. (null to ignore)
    /// </summary>
    public string WhenFormulaSatisfied { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when a number is between two specified values or equal to one of them. (null to ignore)
    /// </summary>
    public (double start, double ends)? WhenNumberInBetween { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the number equals the given value (null to ignore)
    /// </summary>
    public double? WhenNumberEqualTo { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the number is greater than the given value (null to ignore)
    /// </summary>
    public double? WhenNumberGreaterThan { get; set; }

    /// <summary>
    /// Sets a conditional formatting rule to fire when a number is greater than or equal to a given value. (null to ignore)
    /// </summary>
    public double? WhenNumberGreaterThanOrEqual { get; set; }

    /// <summary>
    /// Sets a conditional formatting rule to fire when the number is less than the given value. (null to ignore)
    /// </summary>
    public double? WhenNumberLessThan { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the value is less than or equal to the given value. (null to ignore)
    /// </summary>
    public double? WhenNumberLessThanOrEqual { get; set; }

    /// <summary>
    /// Sets a conditional formatting rule to fire when a number is not between two specified values and is not equal to them. (null to ignore)
    /// </summary>
    public (double start, double ends)? WhenNumberNotBetween { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when a number does not equal a given value. (null to ignore)
    /// </summary>
    public double? WhenNumberNotEqual { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the input contains the given value. (null to ignore)
    /// </summary>
    public string WhenTextContains { get; set; }

    /// <summary>
    /// Sets a conditional formatting rule to fire when the input does not contain the given value. (null to ignore)
    /// </summary>
    public string WhenTextDoesNotContain { get; set; }

    /// <summary>
    /// Sets a conditional formatting rule to fire when input ends with a specified value. (null to ignore)
    /// </summary>
    public string WhenTextEndsWith { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the input equals the given value. (null to ignore)
    /// </summary>
    public string WhenTextEqualTo { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule to fire when the input value begins with the given value. (null to ignore)
    /// </summary>
    public string WhenTextStartsWith { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule for Icon Set (false to ignore)
    /// </summary>
    public bool IsIconSet { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule for Color Scale (false to ignore)
    /// </summary>
    public bool IsColorScale { get; set; }

    /// <summary>
    /// Sets the conditional formatting rule for Data Bara (false to ignore)
    /// </summary>
    public bool IsDataBar { get; set; }
}