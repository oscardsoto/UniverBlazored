using Microsoft.Extensions.DependencyInjection;
using UniverBlazored.Generic;
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
        services.AddScoped<IUniverJsInterop, UniverSpreadsheetJsInterop>();
        services.AddScoped<IUniverSpreadsheetListener, UniverSpreadsheetListener>();
    }
}