using UniverBlazored.Generic;
using UniverBlazored.Generic.Data;
using UniverBlazored.Spreadsheets.Data.Workbook;

namespace UniverBlazored.Spreadsheets.Services.Commands;

public abstract class USpreadsheetCommandBase<TCommand> where TCommand : USpreadsheetCommandBase<TCommand>
{
    protected readonly USpreadsheetSnapshot Snapshot;

    protected readonly IUniverJsInterop UniverJS;

    protected USpreadsheetCommandBase(USpreadsheetSnapshot snapshot, IUniverJsInterop univerJs)
    {
        Snapshot = snapshot ?? new USpreadsheetSnapshot();
        UniverJS = univerJs;
    }

    protected abstract TCommand Clone(USpreadsheetSnapshot context);

    public TCommand OnSheet(USheetInfo? sheet = null)
        => Clone(Snapshot.WithSheet(sheet));

    public TCommand OnRange(URange? range = null)
        => Clone(Snapshot.WithRange(range));

    protected UniverQueue CreateQueue(OperationKind kind = OperationKind.Sheet)
        => new(UniverJS, Snapshot.ToContext(kind));

    protected Task ExecuteAtomically(Func<IUniverJsInterop, Task> operation)
        => UniverJS.ExecuteAtomicAsync(Snapshot.ToContext(), operation);

    protected Task<T> ExecuteAtomically<T>(Func<IUniverJsInterop, Task<T>> operation)
        => UniverJS.ExecuteAtomicAsync(Snapshot.ToContext(), operation);

    protected void UseSheet(UniverQueue queue)
    {
        queue.SetAction("getActiveWorkbook");
        if (Snapshot.SheetSelected == null)
        {
            throw new InvalidOperationException("Sheet context is required. Call OnSheet(...) before executing this command.");
        }
        queue.SetAction("getSheetBySheetId", Snapshot.SheetSelected.Value.id);
    }

    protected void UseRange(UniverQueue queue)
    {
        UseSheet(queue);
        if (Snapshot.RangeSelected == null)
        {
            throw new InvalidOperationException("Range context is required. Call OnRange(...) before executing this command.");
        }
        queue.SetAction("getRange", Snapshot.RangeSelected.Value);
    }
}
