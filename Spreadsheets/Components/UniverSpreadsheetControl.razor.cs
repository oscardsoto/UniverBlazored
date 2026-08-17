using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
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

    private bool isComplete = false;

    string id = "uXlsxComp";

    [Parameter]
    public string Id
    {
        get { return id; }
        set
        {
            if (isComplete)
                return;
            id = value;
        }
    }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public Action<UniverSpreadsheetAgent, UniverUserManager> OnAfterComplete { get; set; }

    public UniverSpreadsheetAgent? Agent { get; private set; }
    
    public UniverUserManager? UserManager => Agent?.UserManager;

    public string InstanceId { get; } = Guid.NewGuid().ToString("N");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await UniverInterop?.InitializeAsync(InstanceId, Id);
            await Listeners?.InitializeListenersAsync(InstanceId);
            Agent = new(UniverInterop, InstanceId);

            isComplete = true;
            StateHasChanged();
            return;
        }

        if (!firstRender && isComplete)
        {
            isComplete = false;
            OnAfterComplete?.Invoke(Agent, UserManager);
            return;
        }
    }
}
