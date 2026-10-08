# Release Procedure

This document describes how to prepare a new version of the Blazing Story packages.
The steps below use `1.0.0-preview.93` as an example. Replace it with the actual new version.

Do not commit unless the user asks. When the user asks, the release commit message is
`v.1.0.0-preview.93 release`.

## 1. Update the version number

Change `<Version>` in `VersionInfo.props` at the repository root.

The `VersionInfo.cs` files in `BlazingStory`, `BlazingStory.ToolKit`, and `BlazingStory.Addons.BuiltIns`
are generated from `VersionInfo.props` during the build. Do not edit them by hand. They are committed,
so include them in the release commit. `GetBuildTimestamp()` in `BlazingStory/VersionInfo.cs` also
changes to the current month.

## 2. Update the Blazing Story package references

Replace the previous version with the new version in the following files.

- `ProjectTemplate/Content/BlazingStoryServer/StoryServerApp.1.csproj` (`BlazingStory` and `BlazingStory.McpServer`)
- `ProjectTemplate/Content/BlazingStoryWasm/Client/StoryWasmApp.1.Client.csproj` (one reference for each target framework)
- `Samples/MyBlazorWasmApp1/MyBlazorWasmApp1.Stories/MyBlazorWasmApp1.Stories.csproj`
- `Tests/Fixtures/BlazorToDoApp/BlazorToDoApp.Stories/BlazorToDoApp.Stories.csproj` (inside a commented-out block)

Run `git grep -n "1.0.0-preview.92"` (with the previous version) to make sure that only `RELEASE-NOTES.txt` still has it.

These files are CRLF in the working tree. If you edit them with GNU `sed` on Windows, use `sed -b -i` to keep the line endings.

## 3. Update the Blazor WebAssembly package references

Check the latest stable versions of the following packages on NuGet for each major version
(8.0, 9.0, and 10.0), and update them if newer versions exist.

- `Microsoft.AspNetCore.Components.WebAssembly`
- `Microsoft.AspNetCore.Components.WebAssembly.DevServer`
- `Microsoft.AspNetCore.Components.WebAssembly.Server`

The NuGet API returns the version list of a package.

```
curl -s https://api.nuget.org/v3-flatcontainer/microsoft.aspnetcore.components.webassembly/index.json
```

Update them in the same files as step 2, plus `ProjectTemplate/Content/BlazingStoryWasm/Server/StoryWasmApp.1.Server.csproj`.
Always do this step when the project template is updated, so that new projects start with the latest patches.

## 4. Write the release notes

Add a new block for the new version at the top of `RELEASE-NOTES.txt`. Follow the format of the existing entries.

- Each item starts with a tag. Use `Feature:`, `Fix:`, `Improve:`, or `Obsolete:`.
- Start the description with a past-tense verb, such as "Resolved" or "Added".
- Describe only what package users can see. Leave out internal changes, such as refactoring,
  build system changes, and changes to the test or sample projects.
- Do not explain the technical cause of a fixed bug.
- Leave out bugs that were added and fixed between the two releases.
- Credit outside contributors, for example `(PR #135 contributed by @Kebechet)`.

Use `git log --oneline <previous release commit>..HEAD` to find the changes since the previous release.
The previous release commit is the one whose subject is `v.<previous version> release`.

The release notes are embedded in every NuGet package, so finish this step before step 5.

## 5. Build the NuGet packages

Build each package project in the Release configuration. Each one produces its `.nupkg` in `_dist/`
(`GeneratePackageOnBuild` is enabled for Release builds).

```
dotnet build BlazingStory.Abstractions/BlazingStory.Abstractions.csproj -c Release
dotnet build BlazingStory.Addons/BlazingStory.Addons.csproj -c Release
dotnet build BlazingStory.ToolKit/BlazingStory.ToolKit.csproj -c Release
dotnet build BlazingStory.Addons.BuiltIns/BlazingStory.Addons.BuiltIns.csproj -c Release
dotnet build BlazingStory/BlazingStory.csproj -c Release
dotnet build BlazingStory.McpServer/BlazingStory.McpServer.csproj -c Release
```

Build each project on its own. A project that is built only as a project reference of another one does not produce a package.

Then pack the project templates.

```
dotnet pack ProjectTemplate/BlazingStory.ProjectTemplates.msbuild -c Release
```

This step needs the GitHub CLI (`gh`), because it downloads the agent skills that are bundled into the templates.

## 6. Check the results

- `_dist/` has seven `.nupkg` files with the new version.
  BlazingStory, BlazingStory.Abstractions, BlazingStory.Addons, BlazingStory.Addons.BuiltIns,
  BlazingStory.ToolKit, BlazingStory.McpServer, and BlazingStory.ProjectTemplates.
- The `.csproj` files inside the project templates package refer to the new version.
- The `<releaseNotes>` in the `.nuspec` of each package has the new release notes.
