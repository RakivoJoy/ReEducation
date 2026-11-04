# TODO-readable - Exercise09 (Garage 3.0)

Suggested improvements for readability.
Items are grouped by file or area.

Rating legend: see [TODO-template.md](TODO-template.md).

---

## Garage\Garage.cs

### Simplify Populate[Prio:Medium, Effort:S, Impact:M]

`Populate` keeps three accumulators (total, parked, failures) and builds the message at the end. Collect the `OperationResult` per vehicle in a list, then derive the counts with `Count()`. Fewer mutable variables to follow.

### Use shorter collection expressions[Prio:Low, Effort:S, Impact:S]

`ListAllVehicles` uses `new List<T>(_vehicles).AsReadOnly()`; `[.. _vehicles]` is shorter. The explicit `IEnumerable.GetEnumerator` forwarders can use expression bodies like the rest of the file.

---

## Garage\SearchCriteria.cs

### Make the Definitions table easier to read[Prio:Medium, Effort:M, Impact:M]

Each entry uses four lambdas with object casts (`(string)value`, `(int)value`). Consider a small generic `Definition<T>` (or a helper that builds an entry from a parser and a match function) so casts are in one place and each row is shorter.

### Rename the two Get overloads[Prio:Low, Effort:S, Impact:S]

`Get<T>` (structs) and `Get` (string) differ only by type constraint. Names like `GetValue<T>` and `GetText` make call sites easier to understand.

---

## Vehicles\Vehicle.cs and subclasses

### Remove repeated subclass boilerplate[Prio:Medium, Effort:M, Impact:M]

Subclasses such as `Boat` repeat the same shape: Min/Max constants, a `ThrowIfOutOfRange` call with a description, unit and `paramName`, and a `Details` override. `ThrowIfOutOfRange` takes six arguments; consider an overload without unit for the common case, and check the other subclasses (`Bus`, `Truck`, `Motorcycle`, `Airplane`) for the same pattern.
