# DocFX documentation workflow

The GitHub Actions workflow `.github/workflows/docfx.yml` builds the DocFX site for
`Exercise09/SolutionSetup` and publishes it to GitHub Pages.

## When it runs

- A push to `main` or `excercise/09-garage3` that changes files under `Exercise09/SolutionSetup/`.
- Manually, from the **Actions** tab (**Build and deploy DocFX** → **Run workflow**).

## One-time setup on GitHub

1. Commit and push the workflow file to the repository.
2. Open the repository on GitHub and go to **Settings → Pages**.
3. Under **Build and deployment**, set **Source** to **GitHub Actions**.
4. Go to the **Actions** tab, select **Build and deploy DocFX**, and click **Run workflow**.
5. Wait for the `build` and `deploy` jobs to finish. Open the `deploy` job to find the site URL.

The site is published at `https://rakivojoy.github.io/ReEducation/` (adjust if the repository or owner name differs).

## Check the result

- Green checkmark on the run: the site was built and deployed.
- Red X: open the failed job and read the log. Common causes are a YAML syntax error in the
  workflow file, a C# build error in the project, or a DocFX warning treated as an error.

## Adding an exercise

- The workflow publishes each `Exercise*/<project>/docfx/` site under a **lowercase** folder name, for example `exercise10/`. GitHub Pages URLs are case-sensitive, so links must use lowercase.
- Add a link to `docs/site-index.html` using the same lowercase path, for example `<a href="exercise10/">Exercise 10</a>`.

## Notes

- GitHub Pages has one site per repository. If both branches deploy, the last successful run replaces the site.
- Public repositories can use GitHub Pages and Actions for free. Private repositories need a paid plan for Pages.
- To build locally, run `Exercise09\SolutionSetup\docfx\build-docs.bat`. Use `build-docs.bat serve` to preview at `http://localhost:8080`.
