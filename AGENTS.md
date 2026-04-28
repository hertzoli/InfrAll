# Repository Guidelines

## Project Structure & Module Organization
The main application lives in `SourceCode/` as a Windows Forms project targeting .NET Framework 4.6. Use `SourceCode/GerenciadorSistemas.sln` or `SourceCode/GerenciadorSistemas.csproj` as the primary entry point. Core files include `Program.cs` for startup, `Form1.cs` and `Form1.Designer.cs` for the main UI, and `PropertyGridRuntimeBuilder.cs` for dynamic property-grid behavior. Static assets are stored in `SourceCode/Imagens/`. `Documentacao/` contains project notes and templates; it is not part of the runtime build. `bin/` and `obj/` are generated output and should not be edited manually.

## Build, Test, and Development Commands
Run commands from the repository root or `SourceCode/`.

- `msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Debug` builds the app for local development.
- `msbuild SourceCode\GerenciadorSistemas.csproj /p:Configuration=Debug /p:PostBuildEvent=` validates compilation without running the post-build signing/obfuscation step.
- `devenv SourceCode\GerenciadorSistemas.sln` opens the solution in Visual Studio when available.

Compilation validation must always use the `Debug` configuration and must never run the post-build command.

There is no automated test suite in the current tree, so validation is primarily manual through the WinForms UI.

## Coding Style & Naming Conventions
Follow the existing C# style in the repository: 4-space indentation, braces on their own lines, and one top-level class per file where practical. Keep WinForms control and event names descriptive, for example `buttonSalvar_Click` and `propertyGridItem_SelectedGridItemChanged`. Preserve the current Portuguese domain vocabulary in user-facing labels and form code. Avoid manual edits to `.Designer.cs`, `.resx`, and generated `Properties/*.Designer.cs` files unless the change requires designer-generated code.

## Testing Guidelines
Because there is no `*.Tests` project yet, test changes manually before submitting:

- Build in `Debug` with the post-build command disabled, then confirm the application launches when manual UI validation is needed.
- Exercise property add, remove, refresh, and selection flows in `Form1`.
- Verify image and config changes still load from `SourceCode/Imagens/` and `App.config`.

If you add automated tests later, place them in a dedicated sibling project such as `SourceCode/GerenciadorSistemas.Tests`.

## Commit & Pull Request Guidelines
This workspace snapshot does not include `.git` history, so no local commit pattern can be verified. Follow the convention already suggested in `README.md`: short imperative commits such as `feat: add property validation` or `fix: refresh property grid after delete`. Pull requests should include a concise summary, impacted screens or files, manual test notes, and screenshots for UI changes.
