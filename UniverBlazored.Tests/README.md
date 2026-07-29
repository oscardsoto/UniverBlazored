# UniverBlazored.Tests

Proyecto de pruebas unitarias con xUnit para `UniverBlazored`.

## Requisitos

- .NET 8 SDK
- Dependencias NuGet (se restauran automáticamente con `dotnet restore`)

## Ejecutar

```bash
dotnet test
```

## Estructura

| Archivo | Propósito |
|---------|-----------|
| `FakeUniverJsInterop.cs` | Sustituto en memoria de `IUniverJsInterop` que registra acciones en lugar de ejecutar JS real. |
| `TestLogger.cs` | `ILogger<T>` dummy para inyectar en servicios. |
| `SchedulerFifoTests.cs` | FIFO por hoja, barreras globales, operaciones estructurales, orden. |
| `SchedulerCancellationTests.cs` | Cancelación anticipada, drenado en dispose, doble dispose, operaciones post-dispose. |
| `TwoInstancesTests.cs` | Aislamiento entre dos instancias independientes y sheets del mismo instance. |
| `ContextRoutingTests.cs` | Ruteo explícito por `SpreadsheetOperationContext`, batch scope, idempotency key. |
| `BulkOperationsTests.cs` | Operaciones masivas concurrentes, batch scope, deadlock prevention. |

## Arquitectura de pruebas

Todas las pruebas usan `SpreadsheetCommandScheduler` real y `FakeUniverJsInterop`:

```
Test → SpreadsheetCommandScheduler → SheetWorker
  → SafeUniverJsInterop → FakeUniverJsInterop (registra acciones)
```

Esto permite verificar el orden FIFO, el aislamiento entre hojas/instancias y la cancelación sin necesidad de navegador ni instancia Univer real.

## Fakes

### FakeUniverJsInterop

Implementa `IUniverJsInterop` con `ConcurrentQueue<string> Actions` que registra cada método invocado. Los métodos que retornan valor devuelven defaults (`null`, `0`, lista vacía). Usar `ResolveActionAsync(actionName)` para iniciar una operación que el scheduler debe procesar.

### TestLogger

Singleton `ILogger<T>` vía `TestLogger<T>.Instance` para pruebas que requieren logging.
