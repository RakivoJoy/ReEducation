# TODO-difficult - Exercise09 (Garage 3.0)

Parts of the code a mid-level engineer (C# Unity3D, Java, C++ background) will find hardest to understand.
Items are grouped by file or area and ordered by difficulty.

Rating legend: see [TODO-template.md](TODO-template.md).

Each item uses the format `### Title[Prio:X, Effort:Y, Impact:Z]`, where Critical = explain or simplify first.

---

## Garage\SearchCriteria.cs

### Definitions table, object boxing and lambdas[Prio:Critical, Effort:M, Impact:L]

Each entry is a positional record with four lambdas. `Parse` returns `object?` and `null` means "invalid", so values are boxed and later unboxed with casts (`(int)value`, `(IntRange)value`).
A cast mismatch only fails at runtime (`InvalidCastException`). Java and C++ developers may know generics but not this "untyped table + cast" pattern.
Idea: a generic `Definition<T>` that hides the casts. Also add a comment that `Parse` returning `null` is the "invalid" signal.

### Nullable value types vs nullable reference types[Prio:Critical, Effort:S, Impact:L]

`int? NumberOfWheels` is `Nullable<int>` (a struct), while `string? Color` is only an annotation.
`text => int.TryParse(text, out int wheels) ? wheels : null` relies on a target-typed conditional (`int` and `null` become `int?`, then boxed to `object?`). This is a C# 9 feature with no direct Java or C++ equivalent. Unity projects often use older language versions.

### Two Get overloads with `where T : struct`[Prio:High, Effort:S, Impact:M]

`Get<T>` (structs) and `Get` (string) differ only by the generic constraint. The constraint is what makes `T?` mean `Nullable<T>`. Rename to `GetValue<T>` / `GetText`, as already noted in TODO-readable.md.

### Property setters that remove entries[Prio:Medium, Effort:S, Impact:M]

Setting a property to `null` removes the filter (`Set`). Properties are therefore views over the `_filters` dictionary, not plain fields. `Matches` uses `Find(filter.Key)!` (null-forgiving operator).

---

## Persistence\JsonGarageStorage.cs

### async/await, await using, CancellationToken[Prio:Critical, Effort:M, Impact:L]

`SaveAsync` and `LoadAsync` use `await using` (async disposal of `FileStream`), `FileOptions.Asynchronous`, and a `CancellationToken` with a default value. Unity developers usually know coroutines or UniTask rather than Task-based async. Explain what the await resumes on and why the stream is disposed before `File.Move`.

### Safe save sequence (temp file, .bak, move)[Prio:High, Effort:M, Impact:M]

Write temp, copy old save to `.bak`, move temp over save, delete temp in `finally`. The reasons (no half-written save file) are not stated step by step. The `finally` block deletes the temp file only if the move did not already consume it.

### Exception filters with pattern matching[Prio:High, Effort:S, Impact:M]

`catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)` combines an exception filter (`when`) with the `or` pattern (C# 9). This is different from Java multi-catch.

### Static JsonSerializerOptions and collection initializer[Prio:Medium, Effort:S, Impact:S]

`Converters = { new JsonStringEnumConverter() }` is a collection initializer on a property that is already created. It calls `Add`; it does not assign.

---

## Garage\GarageHandler.cs

### Sync-over-async: `.GetAwaiter().GetResult()`[Prio:Critical, Effort:S, Impact:L]

`SaveGarage` and `LoadGarage` block on async methods because `IHandler` is synchronous. This is correct here (console app, no `SynchronizationContext`) but is a known deadlock risk in UI frameworks. It needs a comment explaining why it is acceptable, or `IHandler` should become async.

### Expression-bodied members and `?? throw`[Prio:Medium, Effort:S, Impact:M]

`RequireGarage` uses `_garage ?? throw new ...`. Many one-line `=>` members hide the `InvalidOperationException` contract; it is only visible in the class summary.

---

## Garage\Garage.cs

### Generic class with constraint and explicit interface implementation[Prio:High, Effort:M, Impact:M]

`Garage<T> : IEnumerable<T> where T : IVehicle` needs two `GetEnumerator` methods: one public generic and one explicit non-generic (`IEnumerable.GetEnumerator`). Java developers lack explicit interface implementation; C++ developers may expect templates without constraints.
Also, `T?` on a type constrained only to an interface (`FindByRegistration`) is a nullable annotation, not `Nullable<T>`.

### Collection expressions and related syntax[Prio:Medium, Effort:S, Impact:M]

`List<T> _vehicles = [];` and `List<string> failures = [];` use C# 12 collection expressions. The file also uses `is not null` and `static readonly int MaxCapacity` (not `const`, so it is not baked into callers at compile time).

### LINQ GroupBy/ToDictionary[Prio:Medium, Effort:S, Impact:S]

`ListVehicleCountByType` is a LINQ pipeline with deferred execution that is forced by `ToDictionary`.

---

## Persistence\GarageMapper.cs

### Switch expressions with type patterns[Prio:High, Effort:M, Impact:M]

`ToDto(IVehicle)` uses `vehicle switch { Car car => ..., _ => throw ... }`. Type patterns that declare variables and `throw` as an expression are new to Java and C++ developers. The order of arms matters if inheritance is added (more specific types first).

### Named arguments with optional record parameters[Prio:High, Effort:M, Impact:M]

`new VehicleDto(..., FuelType: car.FuelType)` skips the other optional parameters. Reading this requires knowing positional records and optional parameters (see `VehicleDto.cs`).

### Required<T> helper[Prio:Medium, Effort:S, Impact:M]

`where T : struct` and `T?` make `Required<T>(int?)` return `int`. It unwraps `Nullable<T>` or throws.

---

## UI\ConsoleUI.Helpers.cs

### Custom delegate with out parameter, generic ReadValidated[Prio:High, Effort:M, Impact:M]

`TryParser<T>` exists because `Func<>` cannot have out parameters. Lambdas like `(string text, out int value) => int.TryParse(text, out value)` need explicit parameter types.
`ReadValidated<T>` takes a prompt, a `Func` for the error text and a delegate; it is hard to follow at call sites. The `while (true)` loop exits only through `return` or an exception.

### TryParseFuelType[Prio:Medium, Effort:S, Impact:M]

Mixes a ternary, a null-coalescing `?? line`, `Enum.GetNames<T>()` (generic, .NET 5+) and `Enum.IsDefined`. Split into named steps.

### EndOfStreamException as control flow[Prio:Medium, Effort:S, Impact:S]

`ReadValidated` throws when input ends; `ReadLineOrEmpty` returns `""`. These are two different behaviours for end of input. Document where the exception is caught.

---

## UI\ConsoleUI*.cs (partial class)

### Partial class split over five files[Prio:High, Effort:S, Impact:M]

`ConsoleUI` is one class spread over `ConsoleUI.cs`, `.Garage.cs`, `.Helpers.cs`, `.Search.cs` and `.Vehicles.cs`. Unity developers know `partial` only from generated code. Add a short note in `ConsoleUI.cs` listing what each file holds, so readers do not hunt for private fields.

### Range and index operators[Prio:Medium, Effort:S, Impact:S]

`pair[..separatorIndex]` and `pair[(separatorIndex + 1)..]` in `ConsoleUI.Search.cs` are C# 8 slicing; they replace `Substring`.

---

## Vehicles\Vehicle.cs and subclasses, Garage\IntRange.cs

### ThrowIfOutOfRange with six arguments[Prio:Medium, Effort:M, Impact:M]

Validation helper called from every subclass constructor; argument order is easy to mix up (see TODO-readable.md). Check how it interacts with the base constructor call chain.

### IntRange.TryParse and `out IntRange?`[Prio:Medium, Effort:S, Impact:M]

Used from a lambda in `SearchCriteria`. Range text such as `10-30` is parsed there; negative numbers and a single value (`size-size`) need explanation.
