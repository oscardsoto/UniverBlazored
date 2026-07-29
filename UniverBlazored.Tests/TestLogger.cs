using Microsoft.Extensions.Logging;

namespace UniverBlazored.Tests;

internal sealed class TestLogger<T> : ILogger<T>
{
    public static readonly TestLogger<T> Instance = new();
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => false;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
}
