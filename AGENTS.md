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
  patches/                     - Local patches applied to vendored third-party assets
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

## Vendored Third-Party Assets

Some third-party assets are committed directly into the repository instead of being
restored by a package manager, because this project intentionally has no JavaScript
toolchain such as npm.

### axe-core

`BlazingStory.Addons.BuiltIns/wwwroot/js/axe.js`, `axe.min.js`, and `axe.d.ts` are a
vendored copy of [axe-core](https://github.com/dequelabs/axe-core) (MPL-2.0), used by
the accessibility panel addon. They are **not pristine**: a local patch is applied on
top of the upstream build, and both JavaScript files carry a "NOTICE: This file has
been modified by the Blazing Story project" comment at the top.

The applied patches live in `BlazingStory.Addons.BuiltIns/patches/`:

| Patch | Purpose |
|---|---|
| `axe-core-resolve-css-import-url.patch` | Makes axe-core resolve relative `@import` URLs against the containing stylesheet instead of the document base URL, which otherwise causes spurious 404s during CSSOM preload |

**When upgrading axe-core**, replace the three files with the new upstream build and
then re-apply every patch in that folder. Each patch file documents its own
re-application procedure in its header, including how to hand-apply the change to
`axe.min.js` (a minified bundle is a single line, so no line-based diff is usable and
the minified identifiers change on every upstream release). After re-applying, run
`node --check` on both JavaScript files and open a story page to confirm the
accessibility panel still works.

Keep the Deque Systems copyright notice at the top of these files. axe-core is
MPL-2.0, the same license as Blazing Story, so the modified files remain under
MPL-2.0 and their source form is published in this repository.
