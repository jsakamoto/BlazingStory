# ![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/icon.min.64x64.svg) Blazing Story

[![tests](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml/badge.svg)](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml) [![NuGet Package](https://img.shields.io/nuget/v/BlazingStory.svg)](https://www.nuget.org/packages/BlazingStory/) [![Discord](https://img.shields.io/discord/798312431893348414?style=flat&logo=discord&logoColor=white&label=Blazor%20Community&labelColor=5865f2&color=gray)](https://discord.com/channels/798312431893348414/1202165955900473375)

## 📝 Summary

The clone of ["Storybook"](https://storybook.js.org/) for Blazor — a frontend workshop for building UI components and pages in isolation.

[![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/social-preview.png)](https://jsakamoto.github.io/BlazingStory/)

Built **almost 100% on Blazor native** (only a few JavaScript helpers), so you can create a UI catalog application **the Blazor way** — no `npm`, `package.json`, `webpack`, or JS/TS code required.

Blazing Story also provides an **MCP server feature** that exposes component and story information to AI agents for highly accurate code generation.

👉 Try the live demo: <https://jsakamoto.github.io/BlazingStory/>

## 🚀 Getting Started

This walkthrough assumes you already have a Blazor app project (e.g. `MyBlazorApp1`) with components you want to catalog.

### 1. Install the project template (one-time)

```shell
dotnet new install BlazingStory.ProjectTemplates
```

### 2. Create a Blazing Story app project

Add a Blazing Story app project alongside your existing app:

```shell
# For Blazor WebAssembly:
dotnet new blazingstorywasm -n MyBlazorApp1.Stories

# For Blazor Server (add -mcp to enable the MCP server feature):
dotnet new blazingstoryserver -n MyBlazorApp1.Stories

dotnet sln add ./MyBlazorApp1.Stories/
```

> [!Note]  
> The MCP server feature is only available with the Blazor Server variant.

> [!Tip]  
> Visual Studio users can do the equivalent through **Add → New Project** and pick the **"Blazing Story (WebAssembly App)"** or **"Blazing Story (Server App)"** template.

### 3. Reference your component project

```shell
dotnet add ./MyBlazorApp1.Stories reference ./MyBlazorApp1
```

### 4. Add a story file

Place a `*.stories.razor` file (typically under a `Stories/` folder) in the Blazing Story app project:

```html
@* MyBlazorApp1.Stories/Stories/Button.stories.razor *@
@using MyBlazorApp1.Components
@attribute [Stories("Components/Button")]

<Stories TComponent="Button">
    <Story Name="Primary">
        <Template>
            <Button Label="Button" Primary="true" @attributes="context.Args" />
        </Template>
    </Story>
</Stories>
```

> [!Important]  
> The file name must end with `.stories.razor` — this is required by the "Show code" feature on the "Docs" pages.

### 5. Run it!

```shell
dotnet run --project ./MyBlazorApp1.Stories
```

🎉 You'll see the Storybook clone built on Blazor!

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/first-run-of-blazingstory.gif)

## 📓 Documentation

For advanced topics — story structure, args, controls, decorators, addons, and more — see the [Blazing Story Documentation Site](https://blazingstory.github.io/docs/).

## 🤖 AI Agent Skills

Agent skills are published to help AI coding assistants implement stories and custom addons for Blazing Story:

- `blazing-story-story` — generate `.stories.razor` files for your components
- `blazing-story-addon` — scaffold and register custom addons (toolbar, panel, preview decorator)

Install with [GitHub CLI](https://cli.github.com/) (v2.90.0+):

```shell
gh skill install BlazingStory/agent-skills blazing-story-story
gh skill install BlazingStory/agent-skills blazing-story-addon
```

For details and the latest information, see the upstream repository: <https://github.com/BlazingStory/agent-skills>

## 📌 System Requirements

.NET SDK 8 or later

## ⚠️ Caveat

Assembly files of a Blazing Story app embed the project file path as metadata.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/project-dir-in-assembly-metadata.png)

If the project path must remain confidential, avoid using Blazing Story.

## 🙇 Disclaimer

Blazing Story is a personal hobby project and a technical exhibition — it is **not** an officially derived product of Storybook. I plan to keep improving it, but it may be abandoned if I cannot spend enough time on it. Forks and community maintenance are welcome.

Because Blazing Story is a Blazor-native re-implementation that mimics Storybook's look, behavior, and functionality, **none of the Storybook community's contributions, addons, or related services are available here**. The trade-off is that Blazor developers get a Storybook-like component catalog within the familiar .NET ecosystem, free from the JavaScript front-end toolchain.

## 🎉 Release Notes

See [RELEASE-NOTES.txt](https://github.com/jsakamoto/BlazingStory/blob/main/RELEASE-NOTES.txt).

## 📢 License & Third Party Notice

[Mozilla Public License Version 2.0](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/LICENSE)

The third party notice is [here](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/THIRD-PARTY-NOTICES.txt).
