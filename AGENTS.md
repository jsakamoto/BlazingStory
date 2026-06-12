# AGENTS.md

## Project Overview

**Blazing Story** is a clone of [Storybook](https://storybook.js.org/) for Blazor, providing a frontend workshop for building and cataloging UI components and pages in isolation. It is built on almost 100% Blazor native, requiring no JavaScript toolchain such as npm or webpack. It also offers an MCP (Model Context Protocol) server feature to expose component information to AI agents.

License: MPL-2.0

Documentation: https://blazingstory.github.io/docs/

## NuGet Packages

This repository produces the following NuGet packages.

| Package | Role |
|---|---|
| **BlazingStory** | Main library providing Storybook-like UI catalog functionality |
| **BlazingStory.Abstractions** | Foundation layer defining abstract interfaces and base types |
| **BlazingStory.Addons** | Framework for addon extensions |
| **BlazingStory.Addons.BuiltIns** | Built-in addon implementations |
| **BlazingStory.ToolKit** | Shared utility toolkit |
| **BlazingStory.McpServer** | MCP server integration (AI/LLM support) |
| **BlazingStory.ProjectTemplates** | Project templates for `dotnet new` |

## Folder Structure

```
BlazingStory/                  - Main library (BlazingStory package)
BlazingStory.Abstractions/     - Abstract types & interfaces (BlazingStory.Abstractions package)
BlazingStory.Addons/           - Addon framework (BlazingStory.Addons package)
BlazingStory.Addons.BuiltIns/  - Built-in addons (BlazingStory.Addons.BuiltIns package)
BlazingStory.ToolKit/          - Utilities (BlazingStory.ToolKit package)
BlazingStory.McpServer/        - MCP server (BlazingStory.McpServer package)
BlazingStory.Stories/          - Demo/reference Blazor WebAssembly app (no package output)
ProjectTemplate/               - dotnet templates (BlazingStory.ProjectTemplates package)
Samples/                       - Sample applications
Tests/
  BlazingStory.Test/           - Main test project
  BlazingStory.Build.Test/     - Build-related test project
  Fixtures/                    - Test fixture projects
build/                         - Custom MSBuild .targets files
```
