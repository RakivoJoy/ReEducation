# TODO-suggestions - Exercise09 (Garage 3.0)

Suggested weaknesses and improvements for Exercise09 only.

Rating legend: see [TODO-template.md](TODO-template.md). Prio 4 = most valuable, 1 = least valuable.

---

## Weaknesses and inconsistencies

### [Prio:2, Effort:Medium, Impact:Medium] Populate partial success is intended, but the result is misleading

`Garage.Populate` parks what fits and returns `Fail` with the failed vehicles listed, even though some vehicles were parked. Keep partial success, but consider a result that carries the parked count and failures separately so "Fail" does not describe a changed garage.

### [Prio:2, Effort:Medium, Impact:Low] Lookup is a linear scan

`Garage` stores a `List<T>` and every add, find and remove scans it, and `AddVehicle` scans before inserting. Fine for a capacity of tens of vehicles, but a `Dictionary` keyed by registration number would give O(1) duplicate checks and lookups and make the intent explicit.

### [Prio:1, Effort:Low, Impact:Low] `Garage<T>` is a requirement and is only used as `Garage<IVehicle>`

The generic parameter adds no value in current usage.

### [Prio:2, Effort:Low, Impact:Medium] Case and whitespace normalization is spread over several places

Registration numbers are normalized in the `Vehicle` constructor, trimmed again in `FindByRegistration`, and compared with `OrdinalIgnoreCase` in `Search`. Search criteria values (such as color) are not normalized through `VehicleColors`. Pick one place for normalizing input and one comparison rule.

### [Prio:1, Effort:Low, Impact:Low] Mixed code style

Source uses file-scoped namespaces, tests use block-scoped ones. The `Garage` folder uses `namespace SolutionSetup` rather than `SolutionSetup.Garage`. Pick one convention and enforce it with an `.editorconfig`.

---

## Improvements

### [Prio:3, Effort:Low, Impact:High] Enable analyzers and stricter build settings

Add `Directory.Build.props` with `AnalysisLevel`, `EnforceCodeStyleInBuild` and `TreatWarningsAsErrors` (at least for nullable warnings), plus an `.editorconfig`. The project currently disables `ImplicitUsings` and has no analyzer configuration.

### [Prio:3, Effort:Medium, Impact:Medium] Return richer results from the handler

`OperationResult` carries only a `bool` and a message. A typed error (enum or code) lets the UI choose its own wording and lets tests assert on the reason without string matching.

### [Prio:3, Effort:Low, Impact:Medium] Inject the console instead of defaulting to it

`Program.Main` creates `ConsoleUI()` with the default console and handler. Use a small composition root (create `IHandler`, `TextReader` and `TextWriter` in `Program`) so the wiring is visible in one place.

### [Prio:2, Effort:Medium, Impact:Low] Property-based or table-driven tests for validation

Boundary tests are written case by case. A shared table (type, parameter, min, max) used by both validation and tests would keep the ranges and the tests in sync.

### [Prio:2, Effort:High, Impact:Medium] Add a persistence option

The garage is lost on exit. Saving to JSON behind an interface would be a realistic next feature and a good exercise in separating storage from the handler. See [TODO-file-saves.md](TODO-file-saves.md).
