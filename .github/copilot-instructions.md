# Copilot Instructions

## Project Guidelines
- TODO-*.md files must follow docs/TODO-template.md with item headings `### Short title[Prio:X, Effort:Y, Impact:Z]` (Prio: Low/Medium/High/Critical; Effort/Impact: S/M/L).
- Never commit code to main, only feature branches; committing text/documentation files to main is fine.

## Coding practices
- Keep the code simple and as a mid-level engineer. Only few advanced language features.
- Follow SOLID principles.

- Make atomic code changes and commits: one logical change per commit, and run the tests before each commit.
- Commit messages are short one-liners shorter than 144 characters; two lines are acceptable sometimes.

- Create tests and follow a TDD methodlogy.
- Test methods must include // Arrange, // Act, and // Assert comments.
- Create new tests when adding new functions or code paths.

- Add basic XML comments to be used by DocFX
- Add comments to complicated code