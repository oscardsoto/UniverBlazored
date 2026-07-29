using System.Text.Json;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on Sheet Data
/// </summary>
public class DataCommands : USpreadsheetCommandBase<DataCommands>
{
    /// <summary>
    /// Commands focused on Sheet Data
    /// </summary>
    /// <param name="univerJs">Js Interop service</param>
    /// <param name="snapshot"></param>
    public DataCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Set a value on the active Row/Col
    /// </summary>
    /// <param name="value">The value to put on the cell</param>
    public async Task SetValue(object value)
    {
        var queue = CreateQueue();
        UseRange(queue);
        await queue.SetAction("setValue", value).ResolveQueueAsync();
    }

    /// <summary>
    /// Gets the value in the specified Row/Col. If not, return null. CellSelected has to be null to consider the active range
    /// </summary>
    /// <returns>The value from the cell</returns>
    public async Task<TValue> GetValue<TValue>()
    {
        var queue = CreateQueue();
        UseRange(queue);
        var result = await queue.SetAction("getValue").ResolveQueueAsync<object>();
        JsonElement jsonValue = (JsonElement)result;
        return jsonValue.Deserialize<TValue>() ?? default;
    }

    /// <summary>
    /// Set values on the active range 
    /// </summary>
    /// <param name="values">Values of each row for the range. The size of each array must be the same number as the number of columns used</param>
    public async Task SetValue(params object[][] values)
    {
        var queue = CreateQueue();
        UseRange(queue);
        await queue.SetAction("setValues", [values]).ResolveQueueAsync();
    }

    /// <summary>
    /// Gets all values in the range. If a cell in the range is empty, returns null in that cell's position. All non-null values are "JsonElement". Convert to desire.
    /// </summary>
    /// <returns>2 Dimensional array for each row/col values</returns>
    public async Task<object[][]> GetValues()
    {
        var queue = CreateQueue();
        UseRange(queue);
        return await queue.SetAction("getRawValues").ResolveQueueAsync<object[][]>();
    }

    /// <summary>
    /// Set a formula in the cell
    /// </summary>
    /// <param name="formula">All formulas begin with "="</param>
    /// <returns></returns>
    public async Task SetFormula(string formula)
    {
        var queue = CreateQueue();
        UseRange(queue);
        await queue.SetAction("setFormula", formula).ResolveQueueAsync();
    }

    /// <summary>
    /// Set all formulas in the active range
    /// </summary>
    /// <param name="formulas">All formulas begin with "=". The size of each array must be the same number as the number of columns used</param>
    /// <returns></returns>
    public async Task SetFormula(params string[][] formulas)
    {
        var queue = CreateQueue();
        UseRange(queue);
        await queue.SetAction("setFormulas", [formulas]).ResolveQueueAsync();
    }

    /// <summary>
    /// Returns the formula in the cell. If there's not, return an empty string
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetFormula()
    {
        var queue = CreateQueue();
        UseRange(queue);
        var result = await queue.SetAction("getFormula").ResolveQueueAsync<object>();
        JsonElement jsonRes = (JsonElement)result;
        return jsonRes.Deserialize<string>() ?? "";
    }

    /// <summary>
    /// Returns all formulas in the range. If a cell in the range doesn´t have a formula, returns an empty string in that cell's position
    /// </summary>
    /// <returns></returns>
    public async Task<string[][]> GetFormulas()
    {
        var queue = CreateQueue();
        UseRange(queue);
        return await queue.SetAction("getFormulas").ResolveQueueAsync<string[][]>();
    }

    /// <inheritdoc/>
    protected override DataCommands Clone(USpreadsheetSnapshot context)
        => new DataCommands(context, UniverJS);
}