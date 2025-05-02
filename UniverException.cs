using System.Runtime.Serialization;

namespace UniverBlazored;

/// <summary>
/// Univer's exception error at execution
/// </summary>
public class UniverException : Exception
{
    /// <summary>
    /// Univer's exception error at execution
    /// </summary>
    public UniverException() {  }

    /// <summary>
    /// Univer's exception error at execution
    /// </summary>
    /// <param name="message"></param>
    public UniverException(string? message) : base(message) { }
}