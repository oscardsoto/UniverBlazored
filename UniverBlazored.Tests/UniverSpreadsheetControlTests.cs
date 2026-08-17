using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using UniverBlazored.Spreadsheets.Components;
using UniverBlazored.Spreadsheets.Data;

namespace UniverBlazored.Tests;

public class UniverSpreadsheetControlTests
{
    [Fact]
    public async Task Initialization_completes_on_first_render()
    {
        var interop = new FakeUniverJsInterop();
        var control = new TestableUniverSpreadsheetControl(interop, new FakeListener());

        var renderer = new TestRenderer();
        await renderer.RenderAsync(control);

        Assert.Equal(1, interop.InitializationCount);
        Assert.NotNull(control.Agent);
        Assert.NotNull(control.UserManager);
        Assert.Empty(renderer.Exceptions);
    }

    [Fact]
    public async Task OnAfterComplete_is_invoked_after_initialization()
    {
        var interop = new FakeUniverJsInterop();

        UniverSpreadsheetAgent? completedAgent = null;
        UniverUserManager? completedUserManager = null;
        Action<UniverSpreadsheetAgent, UniverUserManager> onAfterComplete = (agent, userManager) =>
        {
            completedAgent = agent;
            completedUserManager = userManager;
        };

        var control = new TestableUniverSpreadsheetControl(interop, new FakeListener());
        var renderer = new TestRenderer();
        await renderer.RenderAsync(control, ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            [nameof(UniverSpreadsheetControl.OnAfterComplete)] = onAfterComplete,
        }));

        Assert.Same(control.Agent, completedAgent);
        Assert.Same(control.UserManager, completedUserManager);
    }

    private sealed class TestRenderer : Renderer
    {
        public TestRenderer()
            : base(new EmptyServiceProvider(), new NullLoggerFactory())
        {
        }

        public override Dispatcher Dispatcher { get; } = new InlineDispatcher();

        public List<Exception> Exceptions { get; } = new();

        public Task RenderAsync(IComponent component)
            => RenderAsync(component, ParameterView.Empty);

        public Task RenderAsync(IComponent component, ParameterView parameters)
            => Dispatcher.InvokeAsync(async () =>
            {
                var componentId = AssignRootComponentId(component);
                await RenderRootComponentAsync(componentId, parameters);
            });

        protected override void HandleException(Exception exception)
            => Exceptions.Add(exception);

        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
            => Task.CompletedTask;
    }

    private sealed class InlineDispatcher : Dispatcher
    {
        public override bool CheckAccess() => true;

        public override Task InvokeAsync(Action workItem)
        {
            workItem();
            return Task.CompletedTask;
        }

        public override Task InvokeAsync(Func<Task> workItem)
            => workItem();

        public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
            => Task.FromResult(workItem());

        public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
            => workItem();
    }

    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => null;
    }

    private sealed class NullLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string categoryName) => NullLogger.Instance;

        public void Dispose() { }
    }

    private sealed class NullLogger : ILogger
    {
        public static readonly NullLogger Instance = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
    }

    private sealed class TestableUniverSpreadsheetControl : UniverSpreadsheetControl
    {
        public TestableUniverSpreadsheetControl(
            IUniverJsInterop? interop,
            IUniverSpreadsheetListener? listener)
        {
            UniverInterop = interop;
            Listeners = listener;
        }

        public Task CallOnAfterRenderAsync(bool firstRender) => OnAfterRenderAsync(firstRender);
    }

    private sealed class FakeListener : IUniverSpreadsheetListener
    {
        public Dictionary<UniverSpreadsheetListenerData, Func<object, Task>> Listeners { get; } = new();

        public Task InitializeListenersAsync() => Task.CompletedTask;

        public Task InitializeListenersAsync(string instanceId) => Task.CompletedTask;

        public Task AddListenerAsync(UniverSpreadsheetListenerData data, Func<object, Task> _event)
            => Task.CompletedTask;

        public Task RemoveListenerAsync(UniverSpreadsheetListenerData data) => Task.CompletedTask;

        public Task RemoveAllListenersAsync(string instanceId) => Task.CompletedTask;

        public UniverSpreadsheetListenerData[] GetListeners() => [];

        public Task OnDataChanged(UniverSpreadsheetListenerData data, object value) => Task.CompletedTask;
    }
}
