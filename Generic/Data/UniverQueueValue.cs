namespace UniverBlazored.Generic.Data;

/// <summary>
/// Queue info to resolve in Univer's core
/// </summary>
public struct UniverQueueValue
{
    /// <summary>
    /// Method name
    /// </summary>
    /// <value></value>
    public string methodName { get; set; } = "";

    /// <summary>
    /// Arguments for that method (make sure that each argument is serializable)
    /// </summary>
    /// <value></value>
    public object[] args { get; set; } = [];

    /// <summary>
    /// Queue info to resolve in Univer's core
    /// </summary>
    public UniverQueueValue() { }

    /// <summary>
    /// Queue info to resolve in Univer's core
    /// </summary>
    /// <param name="methodName">Method name</param>
    /// <param name="args">Arguments for that method (make sure that each argument is serializable)</param>
    public UniverQueueValue(string methodName, params object[] args)
    {
        this.methodName = methodName;
        this.args = args;
    }
}