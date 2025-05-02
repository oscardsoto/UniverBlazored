using System.Diagnostics.CodeAnalysis;

namespace UniverBlazored.Generic.Data;

/// <summary>
/// Response for the result of the queue execution
/// </summary>
public class UniverResponse<TValue>
{
    /// <summary>
    /// Result for that process, in the desired value
    /// </summary>
    public TValue res { get; set; }
}