using System.Text.Json.Nodes;
using Microsoft.JSInterop;
using UniverBlazored.Generic.Data;

namespace UniverBlazored.Generic;

/// <summary>
/// Interface that provides interoperability with Univer and Blazor.
/// </summary>
public interface IUniverJsInterop
{
    /// <summary>
    /// Module to initialize Univer in the component
    /// </summary>
    Lazy<Task<IJSObjectReference>> moduleTask { get; }

    /// <summary>
    /// Action Queue for execute in FacadeAPI for Univer
    /// </summary>
    /// <value></value>
    Queue<UniverQueueValue> actionQueue { get; }

    /// <summary>
    /// Sets all instances of all scripts from Univer acording to the version
    /// </summary>
    /// <param name="newIdDiv">The new Id for the div to execute Univer</param>
    /// <returns></returns>
    Task<bool> InitializeAsync(string newIdDiv);

    /// <summary>
    /// Sets an action for the FacadeAPI 
    /// </summary>
    /// <param name="action">Method's name to execute on the Facade's API</param>
    /// <param name="args">Arguments for the method (must be serializables for Json to pass into JS)</param>
    /// <returns>This, for linking</returns>
    IUniverJsInterop SetAction(string action, params object[] args);

    /// <summary>
    /// Resolve the queue
    /// </summary>
    /// <returns></returns>
    Task ResolveAsync();

    /// <summary>
    /// Resolve the queue
    /// </summary>
    /// <typeparam name="T">Type of value to be expected</typeparam>
    /// <returns></returns>
    Task<T> ResolveAsync<T>();

    /// <summary>
    /// Resolve a function in the javascript module
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="args">Arguments for that method</param>
    /// <typeparam name="T">Type of value to return from that method</typeparam>
    /// <returns>The type desired for the function</returns>
    Task<T> ResolveActionAsync<T>(string name, params object[] args);

    /// <summary>
    /// Resolve a function in the javascript module
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="args">Arguments for that method</param>
    /// <returns></returns>
    Task ResolveActionAsync(string name, params object[] args);

    /// <summary>
    /// Gets all scripts components for Univer
    /// </summary>
    /// <returns>An array of each Univer library to import</returns>
    string[] GetUniverLinks();
}