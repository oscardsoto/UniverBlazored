using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using UniverBlazored.Generic;
using UniverBlazored.Generic.Services;
using UniverBlazored.Spreadsheets.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;
using UniverBlazored.Spreadsheets.Services;

namespace UniverBlazored.Spreadsheets.Components;

public partial class UniverSpreadsheetControl
{
    [Inject]
    protected IUniverJsInterop? UniverInterop { get; set; }

    [Inject]
    protected IUniverSpreadsheetListener? Listeners { get; set; }

    [Inject]
    protected IOptions<UniverConfig>? ConfigOptions { get; set; }

    private bool isComplete = false;

    public string Id { get; } = $"unv-{Guid.NewGuid():N}";

    [Parameter]
    public string CssClass { get; set; } = "";

    /// <summary>
    /// Per-component initialization configuration sent to UniverJS.
    /// Presets enabled here must have their scripts preloaded via the global UniverConfig.InitialConfig manifest.
    /// </summary>
    [Parameter]
    public UniverInit InitConfig { get; set; } = new();

    bool loading = false;

    public bool IsLoading
    { 
        get => loading; 
        set 
        { 
            loading = value; 
            StateHasChanged();
        } 
    }

    static readonly Dictionary<string, string> loadingTexts = new()
    {
        ["en-US"] = "Loading...",
        ["ru-RU"] = "Загрузка...",
        ["zh-CN"] = "加载中...",
        ["vi-VN"] = "Đang tải...",
        ["fa-IR"] = "در حال بارگذاری...",
        ["ja-JP"] = "読み込み中...",
        ["ko-KR"] = "불러오는 중...",
        ["es-ES"] = "Cargando...",
        ["ca-ES"] = "Carregant..."
    };

    public string LoadingText =>
        loadingTexts.TryGetValue(ConfigOptions?.Value?.Language?.Value ?? "", out var text)
            ? text : loadingTexts["en-US"];

    [Parameter]
    public Func<UniverSpreadsheetAgent, UniverUserManager, Task>? OnAfterComplete { get; set; }

    public UniverSpreadsheetAgent? Agent { get; private set; }
    
    public UniverUserManager? UserManager => Agent?.UserManager;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await UniverInterop?.InitializeAsync(Id, Id, InitConfig);
            await Listeners?.InitializeListenersAsync(Id);
            Agent = new(UniverInterop, Id);

            isComplete = true;
            StateHasChanged();
            return;
        }

        if (!firstRender && isComplete)
        {
            isComplete = false;
            IsLoading = true;
            try
            {
                if (OnAfterComplete is not null)
                    await OnAfterComplete(Agent, UserManager);
            }
            finally
            {
                IsLoading = false;
            }
            return;
        }
    }
}
