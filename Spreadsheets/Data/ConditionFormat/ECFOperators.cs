namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// Group of all operator types in Univer
/// </summary>
[Flags]
public enum ECFOperators
{
    ///<summary>
    /// Matches if source value starts with specified text.
    ///</summary>
    beginsWith,
    
    ///<summary>
    /// Matches if source value ends with specified text.
    ///</summary>
    endsWith,
    
    ///<summary>
    /// Matches if source value includes the specified text anywhere within it.
    ///</summary>
    containsText,
    
    ///<summary>
    /// Does not match if source value includes the specified text.
    ///</summary>
    notContainsText,
    
    ///<summary>
    /// Matches when both values are exactly the same.
    ///</summary>
    equal,
    
    ///<summary>
    /// Matches if values are not exactly the same.
    ///</summary>
    notEqual,
    
    ///<summary>
    /// Matches when source value includes at least one white space character.
    ///</summary>
    containsBlanks,
    
    ///<summary>
    /// Does not match if the source value includes no white space characters.
    ///</summary>
    notContainsBlanks,
    
    ///<summary>
    /// Matches when error flag is set for any character in the text string.
    ///</summary>
    containsErrors,
    
    ///<summary>
    /// Does not match if no characters have an error flag set.
    ///</summary>
    notContainsErrors,
    
    ///<summary>
    /// Matches the current date exactly.
    ///</summary>
    today,
    
    ///<summary>
    /// Matches one day prior to the current date.
    ///</summary>
    yesterday,
    
    ///<summary>
    /// Matches one day after the current date.
    ///</summary>
    tomorrow,
    
    ///<summary>
    /// Matches any date within the past seven days.
    ///</summary>
    last7Days,

    ///<summary>
    /// Matches any day within the previous month.
    ///</summary>
    lastMonth,
    
    ///<summary>
    /// Matches any day in the current month.
    ///</summary>
    thisMonth,
    
    ///<summary>
    /// Matches any day within the following month.
    ///</summary>
    nextMonth,
    
    ///<summary>
    /// Matches any week within the previous year.
    ///</summary>
    lastWeek,
    
    ///<summary>
    /// Matches any day in the current week.
    ///</summary>
    thisWeek,
    
    ///<summary>
    /// Matches any day within the following week.
    ///</summary>
    nextWeek,
    
    ///<summary>
    /// Matches when source value is larger than specified number.
    ///</summary>
    greaterThan,
    
    ///<summary>
    /// Matches when source value is larger or equal to the specified number.
    ///</summary>
    greaterThanOrEqual,
    
    ///<summary>
    /// Matches when source value is smaller than specified number.
    ///</summary>
    lessThan,
    
    ///<summary>
    /// Matches when source value is smaller or equal to the specified number.
    ///</summary>
    lessThanOrEqual,
    
    ///<summary>
    /// Does not match if source value is within the range of two specified numbers.
    ///</summary>
    notBetween,
    
    ///<summary>
    /// Matches if source value falls in a defined range of minimum and maximum values.
    ///</summary>
    between
}

/// <summary>
/// Groups for operators that correspond to each Text, Dates and Numbers
/// </summary>
public static class ECFOperatorGropus
{
    /// <summary>
    /// CFTextOperator, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L20)
    /// </summary>
    public static ECFOperators TextOperators = ECFOperators.beginsWith | ECFOperators.endsWith | ECFOperators.containsText | ECFOperators.notContainsText | ECFOperators.equal | 
                                               ECFOperators.notEqual | ECFOperators.containsBlanks | ECFOperators.notContainsBlanks | ECFOperators.containsErrors | ECFOperators.notContainsErrors;

    /// <summary>
    /// CFTimePeriodOperator, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L32)
    /// </summary>
    public static ECFOperators TimePeriodOperators = ECFOperators.today | ECFOperators.yesterday | ECFOperators.tomorrow | ECFOperators.last7Days | ECFOperators.thisMonth | 
                                                     ECFOperators.lastMonth | ECFOperators.nextMonth | ECFOperators.thisWeek | ECFOperators.lastWeek | ECFOperators.nextWeek;

    /// <summary>
    /// CFNumberOperator, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L44)
    /// </summary>
    public static ECFOperators NumberOperators = ECFOperators.greaterThan | ECFOperators.lessThan | ECFOperators.greaterThanOrEqual | ECFOperators.lessThanOrEqual | ECFOperators.notBetween | 
                                                 ECFOperators.between | ECFOperators.equal | ECFOperators.notEqual;
}