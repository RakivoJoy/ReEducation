# TODO-persistence-improvments - Exercise09 (Garage 3.0)

Suggested follow-up work for garage persistence. Not started.

Rating legend: see [TODO-template.md](TODO-template.md).

---

## 1. Delta journaling and merge-on-exit

Decisions are in [TODO-file-saves.md](TODO-file-saves.md).

### [Prio:3, Effort:High, Impact:High] Write delta file and merge on exit

- Write `garage.delta.json` on each change, merge it into `garage.json` on exit, and replay it on the next start after a crash.
- Manual Save and Load stay in the menu alongside the delta.

---

## 2. Startup and exit behavior

### [Prio:2, Effort:Low, Impact:Medium] Load last save at startup

Offer to load the last save at startup.

### [Prio:2, Effort:Low, Impact:Medium] Save on exit when there are unsaved changes

Offer to save on exit when the garage has unsaved changes.

---

## 3. Tests

### [Prio:3, Effort:Low, Impact:Medium] UI test for storage failure message

Add a UI test for a storage failure message (for example read-only or unauthorized) using a fake storage.

### [Prio:3, Effort:Low, Impact:Medium] Use fake or temp-path storage in end-to-end tests

The end-to-end tests currently use `new GarageHandler()`, which points at the real `%LOCALAPPDATA%` save file. Give them a fake or temp-path storage.

---

## 4. Sync-over-async bridge

### [Prio:1, Effort:High, Impact:Low] Remove GetAwaiter().GetResult() by making IHandler and ConsoleUI async

`GarageHandler` calls `GetAwaiter().GetResult()` on the async storage. This is acceptable in a console app but could be removed by making `IHandler` and `ConsoleUI` async. That is a larger change.

---

## 5. User feedback

### [Prio:2, Effort:Low, Impact:Medium] Show the save file path in save and load messages

Print the save file path in the save and load messages.

### [Prio:2, Effort:Low, Impact:Medium] Warn when the garage has changed since the last save

Warn when the garage has changed since the last save.
