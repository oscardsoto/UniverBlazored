namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// CFSubRuleType, from Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L55)
/// </summary>
public enum ECFSubRuleType
{
    ///<summary>
    /// Matches cells with unique values in the column or row.
    ///</summary>
    uniqueValues,
    
    ///<summary>
    /// Matches cells with duplicate values in the column or row.
    ///</summary>
    duplicateValues,
    
    ///<summary>
    /// Matches cells where the value is ranked based on other rules applied.
    ///</summary>
    rank,
    
    ///<summary>
    /// Matches cells where the text content matches a specified pattern or value.
    ///</summary>
    text,
    
    ///<summary>
    /// Matches cells based on time period (e.g., today, yesterday, tomorrow).
    ///</summary>
    timePeriod,
    
    ///<summary>
    /// Matches cells where the value is a number and meets specific conditions (greater than, less than, etc.).
    ///</summary>
    number,
    
    ///<summary>
    /// Matches cells with an average calculation that matches specified conditions.
    ///</summary>
    average,
    
    ///<summary>
    /// Matches cells where the value is a result of a formula and meets specific conditions (e.g., cell values are used in calculations).
    ///</summary>
    formula
}