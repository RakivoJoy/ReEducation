# GitHub Enforcement

These steps configure GitHub so the branch rules apply to everyone, not only to local clones. Local hooks are advisory per clone. A branch ruleset on GitHub is the enforcement layer.

Repository: https://github.com/RakivoJoy/ReEducation

## Local setup (each clone)

Git hooks are not shared automatically. Run this once in each clone:

```
git config core.hooksPath .githooks
```

The local hook `.githooks/pre-commit` blocks commits on `main` that contain files other than `.md` or `.txt`.

## GitHub ruleset for `main` (repository admin)

1. Open **Settings > Rules > Rulesets > New ruleset > New branch ruleset**.
2. Name: `Protect main`.
3. Enforcement status: **Active**.
4. Target branches: **Add target > Default branch** (or add `main`).
5. Enable:
   - **Restrict deletions**
   - **Block force pushes**
   - **Require a pull request before merging** (required approvals: `0` for a solo workflow)
6. Bypass list: leave empty, or add yourself only if you need to push documentation directly to `main`.
7. Click **Create**.

## Known limits

- A ruleset applies to all changes on `main`, including documentation. Once PRs are required, direct pushes of `.md` and `.txt` files are blocked unless you are on the bypass list.
- Bypass is all-or-nothing for the branch. The file-type distinction exists only in the local hook.
- `git commit --no-verify` skips the local hook.
- GitHub cannot check file types in a PR. Use the hook for that.

## Verification

- Commit a code file on `main` locally. The commit should be blocked.
- Commit a `.md` file on `main` locally. The commit should succeed.
- Try `git push` of a code change directly to `main`. It should be rejected once the ruleset is active.
