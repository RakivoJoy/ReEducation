# Title

SOLID improvements for Exercise09 (Garage 3.0)

## Summary

This PR works through the SOLID backlog in `Exercise09/SolutionSetup/docs/TODO-SOLID.md`. It contains 17 commits ahead of `main`, grouped below by principle. Each commit is a single logical change with tests passing.

## Changes

**Process and docs**
- `c1b28d7` Add agent rules: atomic commits, tests, new tests, short commit messages
- `24007a6`, `d34ad69`, `8ef5d79`, `44b7b25` Update SOLID backlog statuses and instructions

**Dependency inversion (DIP)**
- `8969a63` Compose dependencies in `Program` and remove default constructors
- `5ac3b20` Inject `ISampleDataProvider` into `GarageHandler`
- `054b4fe` Introduce `IGarage<T>` and depend on it in handler, storage and mapper

**Single responsibility (SRP)**
- `8f35a63` Return typed results from `Garage` and map them to messages in the handler
- `60350b4` Extract `SearchCriteriaParser` so `SearchCriteria` only holds typed filters
- `db184d9` Add `VehicleFactory` and build vehicles through it instead of `ConsoleUI`
- `177d385` Pass `VehicleRequest` from UI to handler (`ConsoleUI` no longer builds vehicles)
- `3b18352` Extract registration number rules into a `RegistrationNumber` helper

**Interface segregation (ISP)**
- `8e34476` Split `IHandler` into focused lifecycle, operation, query and persistence interfaces

**Liskov substitution (LSP) and open/closed (OCP)**
- `0404620` Pass `VehicleType` to the `Vehicle` base constructor instead of an abstract property
- `c88e29a` Look up parking size limits per vehicle type instead of a switch
- `a307b3c` Use polymorphic vehicle DTOs (one DTO per type, `kind` discriminator)

## Breaking changes

- **Save file format changed.** JSON now uses `kind` instead of `type`, and each vehicle contains only its own fields. Existing garage files fail to load with a failure result. This was approved; no migration is provided.

## Testing

- Build succeeds.
- Full `SolutionSetupTests` suite: 342 passed, 0 failed.
- Persistence load tests were updated for the `kind` discriminator. Mapping tests cover each DTO type.

## Reviewer notes

- `Vehicle` still owns parking-size validation, color validation, the range helper and `ToString`. These are listed as partly done in the backlog.
- `ParkingSizeLimits` usage was not changed.
- Remaining backlog items (`IGarageStorage` split, static dependencies, `IHandler` follow-ups, and so on) are not included.

## Breaking changes

- **Save file format changed.** The JSON now uses `kind` instead of `type`, and each vehicle has only its own fields. Existing garage files will fail to load with a clear failure result. This was approved as acceptable; no migration is provided.

## Testing

- Build succeeds.
- Full `SolutionSetupTests` suite: 342 passed, 0 failed.
- Persistence load tests were updated for the `kind` discriminator. A new test set covers the mapping for each DTO type.

## Reviewer notes

- `Vehicle` still owns parking-size validation, color validation, the range helper and `ToString`. These are listed as partly done in the backlog.
- `ParkingSizeLimits` usage was not changed.
- The remaining backlog items (IHandler split follow-ups, `IGarageStorage`, static dependencies, and so on) are not included.
