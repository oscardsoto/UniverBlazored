using Microsoft.Extensions.DependencyInjection;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Services;
using UniverBlazored.Spreadsheets.Data.Workbook;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Spreadsheets;

/// <summary>
/// Univer spreadsheet service to Blazor.
/// </summary>
public static class UniverSpreadsheetService
{
    /// <summary>
    /// Adds the Univer's service for spreadsheets
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Configuration object</param>
    public static void AddUniverSpreadsheets(this IServiceCollection services, Action<UniverConfig>? configuration = null)
    {
        services.Configure(configuration == null ? config => {} : configuration);
        services.AddScoped<ISpreadsheetCommandScheduler, SpreadsheetCommandScheduler>();
        services.AddScoped<UniverSpreadsheetJsInterop>();
        services.AddScoped<IUniverJsInterop>(sp =>
            new SafeUniverJsInterop(
                sp.GetRequiredService<UniverSpreadsheetJsInterop>(),
                sp.GetRequiredService<ISpreadsheetCommandScheduler>(),
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SafeUniverJsInterop>>()));
        services.AddScoped<IUniverSpreadsheetListener, UniverSpreadsheetListener>();
    }
}
