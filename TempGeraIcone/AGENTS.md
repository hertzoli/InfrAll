# Repository Guidelines

## Project Structure & Module Organization

This repository contains a single C# Windows Forms application targeting .NET Framework 4.7.2.

- `IconGenerator.sln` is the Visual Studio solution.
- `IconGenerator.csproj` defines build settings and NuGet dependencies.
- `Program.cs` contains the application entry point.
- `Form1.cs` contains form behavior and event handlers.
- `Form1.Designer.cs` is Visual Studio Designer-generated UI setup.
- `Form1.resx` and `Properties/Resources.resx` store form and application resources.
- `Properties/AssemblyInfo.cs`, `Settings.settings`, and generated designer files hold assembly metadata and settings.
- `bin/` and `obj/` are build outputs and should not be edited manually.

There is currently no dedicated test project or assets directory. Runtime assets found in `bin/Debug/` should be moved into source-controlled resource locations before relying on them.

## Build, Test, and Development Commands

Run commands from the repository root.

```powershell
msbuild IconGenerator.sln /p:Configuration=Debug
```

Builds the application in Debug mode.

```powershell
msbuild IconGenerator.sln /p:Configuration=Release
```

Creates an optimized Release build.

```powershell
.\bin\Debug\IconGenerator.exe
```

Runs the locally built application. You can also open `IconGenerator.sln` in Visual Studio.

## Coding Style & Naming Conventions

Use standard C# conventions: 4-space indentation, `PascalCase` for types, methods, properties, and controls, and `camelCase` for local variables and parameters. Keep namespaces under `IconGenerator`.

For WinForms changes, keep designer-managed declarations, layout properties, and event subscriptions in `Form1.Designer.cs` through Visual Studio Designer when possible. Put application logic in `Form1.cs`; if logic grows, extract focused helper or service classes.

## Testing Guidelines

No automated test framework is currently configured. For now, validate changes by building in Debug and manually exercising the affected UI workflow. If tests are added, prefer a separate test project such as `IconGenerator.Tests`, with test classes named after the subject under test, for example `IconRendererTests`.

## Commit & Pull Request Guidelines

This checkout has no Git history available, so no repository-specific commit convention can be inferred. Use short, imperative commit messages such as `Add icon export validation` or `Fix image scaling preview`.

Pull requests should include a concise description, the reason for the change, manual test steps, and screenshots or screen recordings for visible UI changes. Mention any new dependencies, resource files, or configuration changes.

## Agent-Specific Instructions

Avoid editing generated files unless the change is required and understood. Do not modify `bin/` or `obj/` outputs directly. Keep documentation and code changes scoped to the requested behavior.
