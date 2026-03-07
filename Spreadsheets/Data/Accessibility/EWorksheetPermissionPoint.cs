namespace UniverBlazored.Spreadsheets.Data.Accessibility;

/// <summary>
/// Worksheet permission points from Univer FWorksheetPermission.
/// </summary>
public enum EWorksheetPermissionPoint
{
    Edit,
    View,
    Copy,
    SetCellValue,
    SetCellStyle,
    SetRowStyle,
    SetColumnStyle,
    InsertRow,
    InsertColumn,
    DeleteRow,
    DeleteColumn,
    Sort,
    Filter,
    PivotTable,
    InsertHyperlink,
    EditExtraObject,
    ManageCollaborator,
    DeleteProtection,
    SelectProtectedCells,
    SelectUnProtectedCells
}
