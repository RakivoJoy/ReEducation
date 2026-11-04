# Agent Instructions

These rules apply to every AI coding agent working in this repository.

## Project Guidelines
- Test methods must include // Arrange, // Act, and // Assert comments.
- TODO Markdown files (TODO-*.md) must follow docs/TODO-template.md. Every item heading uses the format `### Short title[Prio:X, Effort:Y, Impact:Z]`, where Prio is Low, Medium, High or Critical, and Effort and Impact are S, M or L.
- New projects must follow the SOLID principles (single responsibility, open/closed, Liskov substitution, interface segregation, dependency inversion). Use TODO-SOLID.md as a reference for common violations to avoid.

## Git Workflow
- Never commit code to the main branch. Commit code changes only to feature branches.
- Committing text and documentation files (for example .md and .txt) to main is allowed.
