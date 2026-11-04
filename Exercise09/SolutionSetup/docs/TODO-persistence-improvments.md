# TODO-persistence-improvments - Exercise09 (Garage 3.0)

Suggested follow-up work for garage persistence. Not started.

Rating legend: see [TODO-template.md](TODO-template.md).

---

## 1. Delta journaling and merge-on-exit

Decisions are in [TODO-file-saves.md](TODO-file-saves.md).

### Write delta file and merge on exit[Prio:High, Effort:L, Impact:L]

- Write `garage.delta.json` on each change, merge it into `garage.json` on exit, and replay it on the next start after a crash.
- Manual Save and Load stay in the menu alongside the delta.

---

## 2. Startup and exit behavior

### Load last save at startup[Prio:Medium, Effort:S, Impact:M]

Offer to load the last save at startup.

### Save on exit when there are unsaved changes[Prio:Medium, Effort:S, Impact:M]

Offer to save on exit when the garage has unsaved changes.

---

## 3. Tests

### UI test for storage failure message[Prio:High, Effort:S, Impact:M]

Add a UI test for a storage failure message (for example read-only or unauthorized) using a fake storage.

### Use fake or temp-path storage in end-to-end tests[Prio:High, Effort:S, Impact:M]

The end-to-end tests currently use `new GarageHandler()`, which points at the real `%LOCALAPPDATA%` save file. Give them a fake or temp-path storage.

---

## 4. Sync-over-async bridge

### Remove GetAwaiter().GetResult() by making IHandler and ConsoleUI async[Prio:Low, Effort:L, Impact:S]

`GarageHandler` calls `GetAwaiter().GetResult()` on the async storage. This is acceptable in a console app but could be removed by making `IHandler` and `ConsoleUI` async. That is a larger change.

---

## 5. User feedback

### Show the save file path in save and load messages[Prio:Medium, Effort:S, Impact:M]

Print the save file path in the save and load messages.

### Warn when the garage has changed since the last save[Prio:Medium, Effort:S, Impact:M]

Warn when the garage has changed since the last save.
