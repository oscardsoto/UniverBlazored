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

/// <summary>
/// Spreadsheet component to Univer
/// </summary>
public partial class UniverSpreadsheetControl
{
    /// <summary>
    /// Interop to Univer
    /// </summary>
    [Inject]
    protected IUniverJsInterop? UniverInterop { get; set; }

    /// <summary>
    /// Listeners to the cells in Univer
    /// </summary>
    [Inject]
    protected IUniverSpreadsheetListener? Listeners { get; set; }

    private bool isComplete = false;

    string id = "uXlsxComp";

    /// <summary>
    /// Id for the component (Default is "uXlsxComp")
    /// </summary>
    /// <value></value>
    [Parameter]
    public string Id
    {
        get
        {
            return id;
        }
        
        set
        {
            if (isComplete)
                return;
            id = value;
        }
    }

    /// <summary>
    /// Css Classes for the component
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = "";

    /// <summary>
    /// Event when the component finishes to load scripts and listeners in the component
    /// </summary>
    [Parameter]
    public Action<UniverSpreadsheetAgent, UniverUserManager> OnAfterComplete { get; set; } 

    /// <summary>
    /// Agent for the most common operations in Univer
    /// </summary>
    public UniverSpreadsheetAgent? Agent { get; private set; }

    /// <summary>
    /// User Manager Service for the Univer's component
    /// </summary>
    public UniverUserManager? UserManager { get; private set; }

    /// <summary>
    /// Initialize both the Agent, the user Manager and the listeners after rendering the component
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await UniverInterop?.InitializeAsync(Id);
            await Listeners?.InitializeListenersAsync();
            Agent = new(UniverInterop);
            UserManager = new(UniverInterop);

            // It needs to re-render, to correctly apply changes
            isComplete = true;
            StateHasChanged();
            return;
        }

        if (!firstRender && isComplete)
        {
            isComplete = false;
            OnAfterComplete?.Invoke(Agent, UserManager);
            
            // There's no need to re-render again here!
            return;
        }
    }
}