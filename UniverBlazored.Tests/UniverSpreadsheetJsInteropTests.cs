using Microsoft.Extensions.Options;
using UniverBlazored.Generic;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Tests;

public class UniverSpreadsheetJsInteropTests
{
    [Fact]
    public async Task Initializes_immediately_when_scripts_are_ready()
    {
        var runtime = new FakeJsRuntime();
        runtime.Module.ScriptsReady = true;
        var interop = new UniverSpreadsheetJsInterop(runtime, CreateOptions());

        await interop.InitializeAsync("instance1", "div1");

        Assert.Equal(1, runtime.Module.InitUniverCalls);
        Assert.Equal(1, runtime.Module.AreScriptsReadyCalls);
    }

    [Fact]
    public async Task Waits_until_scripts_are_ready_then_initializes()
    {
        var runtime = new FakeJsRuntime();
        runtime.Module.ScriptsReady = false;
        var interop = new UniverSpreadsheetJsInterop(runtime, CreateOptions(timeoutSeconds: 5, pollIntervalMs: 20));

        var task = interop.InitializeAsync("instance1", "div1");

        await Task.Delay(80);
        runtime.Module.ScriptsReady = true;

        await task;

        Assert.Equal(1, runtime.Module.InitUniverCalls);
        Assert.True(runtime.Module.AreScriptsReadyCalls > 1);
    }

    [Fact]
    public async Task Throws_timeout_when_scripts_never_become_ready()
    {
        var runtime = new FakeJsRuntime();
        runtime.Module.ScriptsReady = false;
        var interop = new UniverSpreadsheetJsInterop(runtime, CreateOptions(timeoutSeconds: 0.1, pollIntervalMs: 10));

        var exception = await Assert.ThrowsAsync<TimeoutException>(
            () => interop.InitializeAsync("instance1", "div1"));

        Assert.Contains("Univer scripts", exception.Message);
        Assert.Equal(0, runtime.Module.InitUniverCalls);
    }

    private static IOptions<UniverConfig> CreateOptions(double timeoutSeconds = 30, int pollIntervalMs = 200)
        => Options.Create(new UniverConfig
        {
            ScriptLoadTimeoutSeconds = timeoutSeconds,
            ScriptLoadPollIntervalMs = pollIntervalMs
        });
}
