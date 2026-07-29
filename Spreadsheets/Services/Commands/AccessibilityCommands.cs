using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Accessibility;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

/// <summary>
/// Commands focused on workbook/worksheet accessibility and permissions.
/// Based on Univer's FWorkbookPermission and FWorksheetPermission APIs.
/// </summary>
public class AccessibilityCommands : USpreadsheetCommandBase<AccessibilityCommands>
{
    /// <summary>
    /// Initializes accessibility commands.
    /// </summary>
    public AccessibilityCommands(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs) : base(snapshot, univerJs) { }

    /// <summary>
    /// Sets workbook permission mode.
    /// </summary>
    public async Task SetWorkbookMode(EWorkbookMode mode)
    {
        var queue = CreateQueue();
        await queue.SetAction("getActiveWorkbook")
                   .SetAction("getWorkbookPermission")
                   .SetAction("setMode", mode.ToUniverMode())
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Sets workbook to read-only mode.
    /// </summary>
    public async Task SetWorkbookReadOnly() => await SetWorkbookMode(EWorkbookMode.Viewer);

    /// <summary>
    /// Sets workbook to editable mode.
    /// </summary>
    public async Task SetWorkbookEditable() => await SetWorkbookMode(EWorkbookMode.Editor);

    /// <summary>
    /// Sets a workbook permission point.
    /// </summary>
    public async Task SetWorkbookPoint(EWorkbookPermissionPoint point, bool value)
        => await SetWorkbookPermission(point, value);

    /// <summary>
    /// Gets a workbook permission point.
    /// </summary>
    public async Task<bool> GetWorkbookPoint(EWorkbookPermissionPoint point)
        => await GetWorkbookPermission(point);

    /// <summary>
    /// Gets workbook permission snapshot.
    /// </summary>
    public async Task<Dictionary<string, bool>> GetWorkbookSnapshot()
        => await GetWorkbookPermissions();

    /// <summary>
    /// Checks if workbook is editable.
    /// </summary>
    public async Task<bool> CanEditWorkbook()
    {
        var queue = CreateQueue();
        return await queue.SetAction("getActiveWorkbook")
                          .SetAction("getWorkbookPermission")
                          .SetAction("canEdit")
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Sets one workbook permission point.
    /// </summary>
    public async Task SetWorkbookPermission(EWorkbookPermissionPoint point, bool value)
    {
        var queue = CreateQueue();
        await queue.SetAction("getActiveWorkbook")
                   .SetAction("getWorkbookPermission")
                   .SetAction("setPoint", point.ToUniverPoint(), value)
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Gets one workbook permission point.
    /// </summary>
    public async Task<bool> GetWorkbookPermission(EWorkbookPermissionPoint point)
    {
        var queue = CreateQueue();
        return await queue.SetAction("getActiveWorkbook")
                          .SetAction("getWorkbookPermission")
                          .SetAction("getPoint", point.ToUniverPoint())
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Gets workbook permission snapshot.
    /// </summary>
    public async Task<Dictionary<string, bool>> GetWorkbookPermissions()
    {
        var queue = CreateQueue();
        return await queue.SetAction("getActiveWorkbook")
                          .SetAction("getWorkbookPermission")
                          .SetAction("getSnapshot")
                          .ResolveQueueAsync<Dictionary<string, bool>>();
    }

    /// <summary>
    /// Sets worksheet permission mode.
    /// </summary>
    public async Task SetWorksheetMode(EWorksheetMode mode)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("getWorksheetPermission")
                   .SetAction("setMode", mode.ToUniverMode())
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Sets worksheet permission to read-only.
    /// </summary>
    public async Task SetWorksheetReadOnly() => await SetWorksheetMode(EWorksheetMode.ReadOnly);

    /// <summary>
    /// Sets worksheet permission to editable.
    /// </summary>
    public async Task SetWorksheetEditable() => await SetWorksheetMode(EWorksheetMode.Editable);

    /// <summary>
    /// Checks whether selected worksheet is editable.
    /// </summary>
    public async Task<bool> CanEditWorksheet()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getWorksheetPermission")
                          .SetAction("canEdit")
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Checks whether a worksheet cell is editable.
    /// </summary>
    public async Task<bool> CanEditCell(int row, int col)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getWorksheetPermission")
                          .SetAction("canEditCell", row, col)
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Checks whether a worksheet cell is viewable.
    /// </summary>
    public async Task<bool> CanViewCell(int row, int col)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getWorksheetPermission")
                          .SetAction("canViewCell", row, col)
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Applies worksheet permission configuration in a single operation.
    /// </summary>
    public async Task ApplyWorksheetConfig(UWorksheetPermissionConfig config)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("getWorksheetPermission")
                   .SetAction("applyConfig", config.ToUniverConfig())
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Locks or unlocks the selected sheet.
    /// </summary>
    public async Task SetSheetLocked(bool isLocked)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("getWorksheetPermission")
                   .SetAction(isLocked ? "setReadOnly" : "setEditable")
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Gets sheet lock status.
    /// </summary>
    public async Task<bool> GetSheetLocked()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        var canEdit = await queue.SetAction("getWorksheetPermission")
                                 .SetAction("canEdit")
                                 .ResolveQueueAsync<bool>();
        return !canEdit;
    }

    /// <summary>
    /// Sets one worksheet permission point.
    /// </summary>
    public async Task SetWorksheetPermission(EWorksheetPermissionPoint point, bool value)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        await queue.SetAction("getWorksheetPermission")
                   .SetAction("setPoint", point.ToUniverPoint(), value)
                   .ResolveQueueAsync();
    }

    /// <summary>
    /// Gets one worksheet permission point.
    /// </summary>
    public async Task<bool> GetWorksheetPermission(EWorksheetPermissionPoint point)
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getWorksheetPermission")
                          .SetAction("getPoint", point.ToUniverPoint())
                          .ResolveQueueAsync<bool>();
    }

    /// <summary>
    /// Gets worksheet permission snapshot.
    /// </summary>
    public async Task<Dictionary<string, bool>> GetWorksheetPermissions()
    {
        var queue = CreateQueue();
        UseSheet(queue);
        return await queue.SetAction("getWorksheetPermission")
                          .SetAction("getSnapshot")
                          .ResolveQueueAsync<Dictionary<string, bool>>();
    }

    /// <summary>
    /// Gets worksheet permission snapshot.
    /// </summary>
    public async Task<Dictionary<string, bool>> GetWorksheetSnapshot()
        => await GetWorksheetPermissions();

    /// <summary>
    /// Lists range protection rules for selected sheet.
    /// </summary>
    public async Task<URangeProtectionRuleInfo[]> ListRangeProtectionRules()
        => await GetProtectedRanges();

    /// <summary>
    /// Creates protection rules from range configurations.
    /// </summary>
    public async Task ProtectRanges(params UProtectRangeConfig[] configs)
        => await UniverJS.ResolveActionAsync("protectRangesInSheet", Snapshot, configs);

    /// <summary>
    /// Lists protected range rules for selected sheet.
    /// </summary>
    public async Task<URangeProtectionRuleInfo[]> GetProtectedRanges()
        => await UniverJS.ResolveActionAsync<URangeProtectionRuleInfo[]>("getProtectedRangesInSheet", Snapshot);

    /// <summary>
    /// Removes protection rules by rule ids.
    /// </summary>
    public async Task UnprotectRules(params string[] ruleIds)
        => await UniverJS.ResolveActionAsync("unprotectRuleIdsInSheet", Snapshot, ruleIds);

    /// <summary>
    /// Locks or unlocks the selected/active range.
    /// </summary>
    public async Task SetRangeLocked(bool isLocked, URangeProtectionOptions? options = null)
        => await UniverJS.ResolveActionAsync("setActiveRangeLock", Snapshot, isLocked, options ?? new URangeProtectionOptions());

    /// <summary>
    /// Gets whether selected/active range is locked.
    /// </summary>
    public async Task<bool> GetRangeLocked()
        => await UniverJS.ResolveActionAsync<bool>("isActiveRangeLocked", Snapshot);

    /// <inheritdoc />
    protected override AccessibilityCommands Clone(USpreadsheetSnapshot context)
        => new(context, UniverJS);
}

internal static class UAccessibilityMappings
{
    public static string ToUniverMode(this EWorkbookMode mode) => mode switch
    {
        EWorkbookMode.Owner => "owner",
        EWorkbookMode.Editor => "editor",
        EWorkbookMode.Viewer => "viewer",
        EWorkbookMode.Commenter => "commenter",
        _ => "editor"
    };

    public static string ToUniverMode(this EWorksheetMode mode) => mode switch
    {
        EWorksheetMode.Editable => "editable",
        EWorksheetMode.ReadOnly => "readOnly",
        EWorksheetMode.FilterOnly => "filterOnly",
        _ => "editable"
    };

    public static string ToUniverPoint(this EWorkbookPermissionPoint point) => point switch
    {
        EWorkbookPermissionPoint.Edit => "WorkbookEdit",
        EWorkbookPermissionPoint.View => "WorkbookView",
        EWorkbookPermissionPoint.Print => "WorkbookPrint",
        EWorkbookPermissionPoint.Export => "WorkbookExport",
        EWorkbookPermissionPoint.Share => "WorkbookShare",
        EWorkbookPermissionPoint.CopyContent => "WorkbookCopy",
        EWorkbookPermissionPoint.DuplicateFile => "WorkbookDuplicate",
        EWorkbookPermissionPoint.Comment => "WorkbookComment",
        EWorkbookPermissionPoint.ManageCollaborator => "WorkbookManageCollaborator",
        EWorkbookPermissionPoint.CreateSheet => "WorkbookCreateSheet",
        EWorkbookPermissionPoint.DeleteSheet => "WorkbookDeleteSheet",
        EWorkbookPermissionPoint.RenameSheet => "WorkbookRenameSheet",
        EWorkbookPermissionPoint.MoveSheet => "WorkbookMoveSheet",
        EWorkbookPermissionPoint.HideSheet => "WorkbookHideSheet",
        EWorkbookPermissionPoint.ViewHistory => "WorkbookViewHistory",
        EWorkbookPermissionPoint.ManageHistory => "WorkbookHistory",
        EWorkbookPermissionPoint.RecoverHistory => "WorkbookRecoverHistory",
        EWorkbookPermissionPoint.CreateProtection => "WorkbookCreateProtect",
        EWorkbookPermissionPoint.InsertRow => "WorkbookInsertRow",
        EWorkbookPermissionPoint.InsertColumn => "WorkbookInsertColumn",
        EWorkbookPermissionPoint.DeleteRow => "WorkbookDeleteRow",
        EWorkbookPermissionPoint.DeleteColumn => "WorkbookDeleteColumn",
        EWorkbookPermissionPoint.CopySheet => "WorkbookCopySheet",
        _ => "WorkbookEdit"
    };

    public static string ToUniverPoint(this EWorksheetPermissionPoint point) => point switch
    {
        EWorksheetPermissionPoint.Edit => "WorksheetEdit",
        EWorksheetPermissionPoint.View => "WorksheetView",
        EWorksheetPermissionPoint.Copy => "WorksheetCopy",
        EWorksheetPermissionPoint.SetCellValue => "WorksheetSetCellValue",
        EWorksheetPermissionPoint.SetCellStyle => "WorksheetSetCellStyle",
        EWorksheetPermissionPoint.SetRowStyle => "WorksheetSetRowStyle",
        EWorksheetPermissionPoint.SetColumnStyle => "WorksheetSetColumnStyle",
        EWorksheetPermissionPoint.InsertRow => "WorksheetInsertRow",
        EWorksheetPermissionPoint.InsertColumn => "WorksheetInsertColumn",
        EWorksheetPermissionPoint.DeleteRow => "WorksheetDeleteRow",
        EWorksheetPermissionPoint.DeleteColumn => "WorksheetDeleteColumn",
        EWorksheetPermissionPoint.Sort => "WorksheetSort",
        EWorksheetPermissionPoint.Filter => "WorksheetFilter",
        EWorksheetPermissionPoint.PivotTable => "WorksheetPivotTable",
        EWorksheetPermissionPoint.InsertHyperlink => "WorksheetInsertHyperlink",
        EWorksheetPermissionPoint.EditExtraObject => "WorksheetEditExtraObject",
        EWorksheetPermissionPoint.ManageCollaborator => "WorksheetManageCollaborator",
        EWorksheetPermissionPoint.DeleteProtection => "WorksheetDeleteProtection",
        EWorksheetPermissionPoint.SelectProtectedCells => "WorksheetSelectProtectedCells",
        EWorksheetPermissionPoint.SelectUnProtectedCells => "WorksheetSelectUnProtectedCells",
        _ => "WorksheetEdit"
    };

    public static object ToUniverConfig(this UWorksheetPermissionConfig config)
    {
        var mode = config.Mode?.ToUniverMode();
        var rangeProtections = config.RangeProtections?.Select(item => new
        {
            rangeRefs = item.RangeRefs,
            options = item.Options
        }).ToArray();

        return new
        {
            mode,
            points = config.Points,
            rangeProtections
        };
    }
}
