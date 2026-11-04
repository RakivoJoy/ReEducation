# TODO-SOLID - Exercise09 (Garage 3.0)

Review of the code against the SOLID principles. Suggestions only; nothing has been changed.
Items are grouped by principle. Overlapping items in [TODO-suggestions.md](TODO-suggestions.md) are referenced where relevant.

Rating legend: see [TODO-template.md](TODO-template.md).

---

## What already follows SOLID

- `IHandler`, `IUI`, `IGarageStorage` and `IVehicle` exist, so the UI, storage and garage can be replaced or faked in tests (DIP).
- `GarageHandler` receives `IGarageStorage` through its constructor, and `ConsoleUI` receives `TextReader`, `TextWriter` and `IHandler` through an internal constructor (DIP).
- `GarageMapper` and the DTOs keep persistence concerns out of the `Vehicle` classes (SRP).
- `ParkingSizeLimits`, `VehicleColors` and `SearchCriteria.Definitions` each keep one set of rules in one place (SRP).
- Vehicle subclasses only add their own field and `Details`, and are used through `Vehicle` / `IVehicle` (LSP, OCP).

---

## Single Responsibility Principle (SRP)

### ConsoleUI does menu, input parsing, vehicle construction and output[Prio:High, Effort:M, Impact:M]

`ConsoleUI` (five partial files) owns the menu loop, reading and validating input, the per-type prompts and factories (`BuildVehicleTypes` creates `Motorcycle`, `Car`, ... and prompts for their extra fields), search-expression parsing, and printing. Splitting it into partial files hides the size but it is still one class with one set of private fields.
Suggestion: extract an input reader (prompt + validate + parse), a vehicle-creation component (type list and factories), and keep `ConsoleUI` as menu loop plus dispatch.

### Garage mixes storage, rules, user messages and reporting[Prio:High, Effort:M, Impact:M]

`Garage<T>` holds the list, enforces capacity and uniqueness, builds user-facing message strings (`"The garage is full."`, `NotFoundMessage`), runs batch `Populate`, and does reporting (`ListVehicleCountByType`) and searching. The UI calls the static `Garage<IVehicle>.NotFoundMessage`, so presentation text lives in the domain.
Suggestion: have `Garage` return a typed result or reason (see the richer results item in TODO-suggestions.md) and let the UI or handler turn it into text. Consider moving `Populate` and the per-type counting to the handler or a query helper.

### SearchCriteria is a filter, a text parser and an error-message source[Prio:Medium, Effort:M, Impact:M]

`SearchCriteria` stores filters, parses console text (`TrySetFilter`, `Parse` lambdas), owns user-facing error messages and implements the match rules. Parsing and messages are UI concerns; matching is a domain concern.
Suggestion: keep `SearchCriteria` as typed values plus `Matches`, and move text parsing and messages to the UI layer (for example a `SearchCriteriaParser`).

### GarageHandler owns lifecycle, delegation, persistence orchestration and sample data[Prio:Low, Effort:S, Impact:S]

The handler creates and replaces the garage, forwards every call, bridges async storage (`GetAwaiter().GetResult()`), and pulls in `SampleData`. It is thin enough to be acceptable, but it grows with every menu option.
Suggestion: keep it as a facade, and move sample data behind an interface (see the DIP section).

### JsonGarageStorage combines serialization and the safe-write sequence[Prio:Low, Effort:M, Impact:S]

One class does JSON options, directory creation, temp file, `.bak` copy, move, cleanup, and error-to-result mapping.
Suggestion: if more formats or locations are added, separate "how to serialize" from "how to write a file safely".

### Vehicle validates registration, color, size and formats its own description[Prio:Low, Effort:M, Impact:S]

`Vehicle` contains the registration regex and normalization, parking size validation, a range-check helper and the `ToString` format. Display formatting and validation rules are separate reasons to change.
Suggestion: move registration rules to a small `RegistrationNumber` helper or value type, and leave formatting to a presenter if more output formats appear.

---

## Open/Closed Principle (OCP)

### Adding a vehicle type requires edits in many files[Prio:High, Effort:L, Impact:L]

A new type touches: the `VehicleType` enum, the subclass, `ParkingSizeLimits.For` (switch), `GarageMapper.ToDto` and `ToVehicle` (switches), `VehicleDto` (another optional field), and `ConsoleUI.BuildVehicleTypes`. Forgetting one fails only at runtime (`NotSupportedException`, `ArgumentException`).
Suggestion: describe each type once (a descriptor or registry holding limits, DTO mapping and UI creation), or use a polymorphic DTO hierarchy (`[JsonPolymorphic]`, as originally planned in TODO-file-saves.md) so the mapper does not need a switch per type.

### ParkingSizeLimits is a switch over the enum[Prio:Medium, Effort:S, Impact:M]

Limits per type are kept in a central switch instead of next to the type they describe. A new `VehicleType` falls through to the `throw`.
Suggestion: let each vehicle type expose its limits (static abstract member or a descriptor entry) so the table extends without modification.

### SearchCriteria knows concrete vehicle types[Prio:Low, Effort:S, Impact:S]

The fuel definition uses `v is Car car`, so the search code depends on `Car`. A second fuel-capable type would need an edit.
Suggestion: introduce a small interface such as `IFuelVehicle` or move type-specific filters next to the type.

### Wide flat VehicleDto grows with every type[Prio:Medium, Effort:M, Impact:M]

`VehicleDto` has one nullable field per subclass-specific property. Every new type adds a field and mapper branches.
Suggestion: part of the polymorphic DTO suggestion above.

---

## Liskov Substitution Principle (LSP)

### Garage<IVehicle> accepts any IVehicle but only built-in types can be saved[Prio:Medium, Effort:S, Impact:M]

`GarageMapper.ToDto(IVehicle)` throws `NotSupportedException` for any `IVehicle` that is not one of the six known classes. A different `IVehicle` implementation can be parked but breaks saving, so it is not a true substitute.
Suggestion: either restrict the garage to `Vehicle`, or make the mapper extensible so each implementation supplies its own DTO mapping.

### Vehicle constructor reads the abstract Type property[Prio:Medium, Effort:S, Impact:S]

`ValidateParkingSize` in the base constructor calls `ParkingSizeLimits.For(Type)`, where `Type` is an abstract member overridden by a subclass that has not been constructed yet. It works only while every override returns a constant.
Suggestion: pass the type (or its limits) to the base constructor, or document and test the constant-only contract.

---

## Interface Segregation Principle (ISP)

### IHandler is a wide interface[Prio:Medium, Effort:M, Impact:M]

`IHandler` has 11 members covering garage lifecycle, parking and removal, queries, search and persistence. Any fake must implement all of it, and a consumer needing only queries still depends on save and load.
Suggestion: split into focused interfaces (for example garage lifecycle, vehicle operations, queries, persistence) and let `GarageHandler` implement all of them. `ConsoleUI` can keep taking the combined interface until the UI is split.

### IGarageStorage couples save and load[Prio:Low, Effort:S, Impact:S]

Read-only or write-only consumers (a startup loader, an exit saver) still depend on both methods.
Suggestion: only split if a consumer appears that needs one side; otherwise leave it.

---

## Dependency Inversion Principle (DIP)

### Default constructors create concrete dependencies[Prio:High, Effort:S, Impact:M]

`Program` creates `new ConsoleUI()`, which creates `new GarageHandler()`, which creates `new JsonGarageStorage(DefaultFilePath)`. The default handler constructor also makes end-to-end tests write to the real `%LOCALAPPDATA%` file (see TODO-persistence-improvments.md).
Suggestion: build the object graph in `Program` (composition root) and remove or reduce the default constructors. See "Inject the console instead of defaulting to it" in TODO-suggestions.md.

### Handler and storage depend on the concrete Garage<IVehicle>[Prio:Medium, Effort:M, Impact:M]

`GarageHandler`, `IGarageStorage`, `JsonGarageStorage` and `GarageMapper` all use the concrete `Garage<IVehicle>` class, so the storage contract is tied to one implementation.
Suggestion: introduce an `IGarage` abstraction for the operations that consumers need, or have storage work on a snapshot (`GarageDto`) instead of the live garage.

### Static dependencies cannot be replaced[Prio:Medium, Effort:S, Impact:M]

`SampleData`, `GarageMapper`, `ParkingSizeLimits`, `VehicleColors` and `Garage<T>.MaxCapacity` are static. `GarageHandler.PopulateGarage` always uses `SampleData.CreateSampleVehicles()`, so tests cannot supply their own sample set.
Suggestion: wrap the ones that vary behind small interfaces (for example `ISampleDataProvider`) and inject them. Leave pure constant tables static.

### UI builds domain objects and depends on concrete vehicle classes[Prio:Medium, Effort:L, Impact:M]

`ConsoleUI` constructs `Car`, `Bus`, `Boat`, ... directly and hands an `IVehicle` to `IHandler.ParkVehicle`. The UI therefore knows every domain subclass and its constructor rules.
Suggestion: pass a request object (type, registration, color, wheels, size, type-specific values) to the handler and let a domain-side factory create the vehicle. The UI then depends only on abstractions.

---

## Suggested order

1. Composition root and removing default constructors (DIP, quick win, helps tests).
2. Typed results from `Garage` and handler, moving message text to the UI (SRP).
3. Split `IHandler` (ISP).
4. One descriptor per vehicle type or polymorphic DTO (OCP, LSP). This is the largest change and should come last.
