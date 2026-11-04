# TODO-suggestions - Exercise09 (Garage 3.0)

Suggested weaknesses and improvements for Exercise09 only.

Critical = most valuable, Low = least valuable.

---

## Weaknesses and inconsistencies

### Populate partial success is intended, but the result is misleading[Prio:Medium, Effort:M, Impact:M]

`Garage.Populate` parks what fits and returns `Fail` with the failed vehicles listed, even though some vehicles were parked. Keep partial success, but consider a result that carries the parked count and failures separately so "Fail" does not describe a changed garage.

### Lookup is a linear scan[Prio:Medium, Effort:M, Impact:S]

`Garage` stores a `List<T>` and every add, find and remove scans it, and `AddVehicle` scans before inserting. Fine for a capacity of tens of vehicles, but a `Dictionary` keyed by registration number would give O(1) duplicate checks and lookups and make the intent explicit.

### `Garage<T>` is a requirement and is only used as `Garage<IVehicle>`[Prio:Low, Effort:S, Impact:S]

The generic parameter adds no value in current usage.

### Case and whitespace normalization is spread over several places[Prio:Medium, Effort:S, Impact:M]

Registration numbers are normalized in the `Vehicle` constructor, trimmed again in `FindByRegistration`, and compared with `OrdinalIgnoreCase` in `Search`. Search criteria values (such as color) are not normalized through `VehicleColors`. Pick one place for normalizing input and one comparison rule.

### Mixed code style[Prio:Low, Effort:S, Impact:S]

Source uses file-scoped namespaces, tests use block-scoped ones. The `Garage` folder uses `namespace SolutionSetup` rather than `SolutionSetup.Garage`. Pick one convention and enforce it with an `.editorconfig`.

---

## Improvements

### Enable analyzers and stricter build settings[Prio:High, Effort:S, Impact:L]

Add `Directory.Build.props` with `AnalysisLevel`, `EnforceCodeStyleInBuild` and `TreatWarningsAsErrors` (at least for nullable warnings), plus an `.editorconfig`. The project currently disables `ImplicitUsings` and has no analyzer configuration.

### Return richer results from the handler[Prio:High, Effort:M, Impact:M]

`OperationResult` carries only a `bool` and a message. A typed error (enum or code) lets the UI choose its own wording and lets tests assert on the reason without string matching.

### Inject the console instead of defaulting to it[Prio:High, Effort:S, Impact:M]

`Program.Main` creates `ConsoleUI()` with the default console and handler. Use a small composition root (create `IHandler`, `TextReader` and `TextWriter` in `Program`) so the wiring is visible in one place.

### Property-based or table-driven tests for validation[Prio:Medium, Effort:M, Impact:S]

Boundary tests are written case by case. A shared table (type, parameter, min, max) used by both validation and tests would keep the ranges and the tests in sync.

### Add a persistence option[Prio:Medium, Effort:L, Impact:M]

The garage is lost on exit. Saving to JSON behind an interface would be a realistic next feature and a good exercise in separating storage from the handler. See [TODO-file-saves.md](TODO-file-saves.md).
