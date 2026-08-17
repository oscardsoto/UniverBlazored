using Microsoft.JSInterop;

namespace UniverBlazored.Tests;

public sealed class FakeJsRuntime : IJSRuntime
{
    public FakeJsObjectReference Module { get; } = new();

    public ValueTask<T> InvokeAsync<T>(string identifier, object?[]? args)
        => InvokeAsync<T>(identifier, CancellationToken.None, args);

    public ValueTask<T> InvokeAsync<T>(string identifier, CancellationToken cancellationToken, object?[]? args)
        => identifier == "import"
            ? ValueTask.FromResult<T>((T)(object)Module)
            : Module.InvokeAsync<T>(identifier, cancellationToken, args);
}

public sealed class FakeJsObjectReference : IJSObjectReference
{
    private int chargeScriptCalls;
    private int readyChecks;
    private int initUniverCalls;

    public int ChargeScriptCalls => chargeScriptCalls;
    public int AreScriptsReadyCalls => readyChecks;
    public int InitUniverCalls => initUniverCalls;
    public bool ScriptsReady { get; set; }
    public bool ScriptsInHead { get; set; }

    public ValueTask<T> InvokeAsync<T>(string identifier, object?[]? args)
        => InvokeAsync<T>(identifier, CancellationToken.None, args);

    public ValueTask<T> InvokeAsync<T>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        switch (identifier)
        {
            case "chargeScript":
                Interlocked.Increment(ref chargeScriptCalls);
                ScriptsReady = true;
                ScriptsInHead = true;
                return ValueTask.FromResult<T>((T)(object)true);
            case "areScriptsReady":
                Interlocked.Increment(ref readyChecks);
                return ValueTask.FromResult<T>((T)(object)ScriptsReady);
            case "areScriptsInHead":
                if (ScriptsInHead)
                    return ValueTask.FromResult<T>((T)(object)Array.Empty<string>());
                return ValueTask.FromResult<T>((T)(object)args!);
            case "initUniver":
                Interlocked.Increment(ref initUniverCalls);
                return ValueTask.FromResult<T>(default!);
            default:
                return ValueTask.FromResult<T>(default!);
        }
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
