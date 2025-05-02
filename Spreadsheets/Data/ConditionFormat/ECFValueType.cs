namespace UniverBlazored.Spreadsheets.Data.ConditionFormat;

/// <summary>
/// CFValueType, form Univer (See doc: https://github.com/dream-num/univer/blob/dev/packages/sheets-conditional-formatting/src/base/const.ts#L73)
/// </summary>
public enum ECFValueType
{
    ///<summary>
     /// Represents a numerical value (i.e., 1,234).
     ///</summary>
    num,
     
     ///<summary>
     /// Represents the minimum possible value for the type of data being evaluated.
     ///</summary>
    min,
      
     ///<summary>
     /// Represents the maximum possible value for the type of data being evaluated.
     ///</summary>
    max,
     
     ///<summary>
     /// Represents a percentage (i.e., 25%, 75%).
     ///</summary>
    percent,
      
     ///<summary>
     /// Represents the value at a particular percentile within the dataset.
     /// For example, 90th percentile represents the highest 10% of data values.
     ///</summary>
    percentile,
     
     ///<summary>
     /// Represents a formula result (i.e., SUM(A:B), AVG(C:D)).
     ///</summary>
    formula
}
