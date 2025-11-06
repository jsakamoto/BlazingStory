﻿# ![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/icon.min.64x64.svg) Blazing Story

[![tests](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml/badge.svg)](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml) [![NuGet Package](https://img.shields.io/nuget/v/BlazingStory.svg)](https://www.nuget.org/packages/BlazingStory/) [![Discord](https://img.shields.io/discord/798312431893348414?style=flat&logo=discord&logoColor=white&label=Blazor%20Community&labelColor=5865f2&color=gray)](https://discord.com/channels/798312431893348414/1202165955900473375)

## 📝 Summary

The clone of ["Storybook"](https://storybook.js.org/) for Blazor, a front-end workshop for building UI components and pages in isolation.

[![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/social-preview.png)](https://jsakamoto.github.io/BlazingStory/)

The "Blazing Story" is built on **almost 100% Blazor native** (except only a few JavaScript helper codes), so you don't have to care about `npm`, `package.json`, `webpack`, and any JavaScript/TypeScript code. You can create a UI catalog application **on the Blazor way!**

In addition, Blazing Story also provides an **MCP server feature.** This allows Blazing Story to expose information about its components and stories to AI agents, enabling highly accurate code generation.

You can try it out from the live demonstration site at the following link: https://jsakamoto.github.io/BlazingStory/

## 🚀 Getting Started

### Example scenario

For the example scenario, you already have a Blazor WebAssembly application project, "MyBlazorWasmApp1", that includes the "Button" component.

> [!Note]  
> Blazing Story supports Blazor Server application projects as well as Blazor WebAssembly application projects.

```
📂 (working directory)
    + 📄 MyBlazorWasmApp1.sln
    + 📂 MyBlazorWasmApp1
        + 📄 MyBlazorWasmApp1.csproj
        + ...
        + 📂 Components
            + 📄 Button.razor
            + ...
```

### Preparation

Close all Visual Studio IDE instances (if you use Visual Studio IDE), and install the "Blazing Story" project template with the following command. (This installation instruction is enough to execute once in your development environment.)

```shell
dotnet new install BlazingStory.ProjectTemplates
```

### Creating a Blazing Story app and stories

#### Step 1 - Create a new Blazing Story app project

Open the solution file (.sln) with Visual Studio, and add a new **"Blazing Story (WebAssembly App)"** project from the project templates. (In this example scenario, we named it "MyBlazorWasmApp1.Stories")

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/add-a-new-project.png)

> [!Note]  
> If you are working on a Blazor Server application project, you should add a new **"Blazing Story (Server App)"** project instead of the "Blazing Story (WebAssembly App)" project.

> [!Note]  
> To use the MCP server feature, you need to add a new **"Blazing Story (Server App)"** project and check the "Enable the MCP server feature" checkbox in the project creation dialog.  
> Note that the MCP server feature is not available in the Blazing Story app when running on Blazor WebAssembly.

If you are working on dotnet CLI, you can do that with the following commands in a terminal.

> [!Note]  
> Please remind again that this example scenario assumes that there is already a solution file (.sln) in the current directory with an existing Blazor WebAssembly app project.

```shell
# Create a new Blazing Story app
dotnet new blazingstorywasm -n MyBlazorWasmApp1.Stories
# Add the Blazing Story app project to the solution
dotnet sln add ./MyBlazorWasmApp1.Stories/
```

> [!Note]  
> If you are working on a Blazor Server application project, you should run the `dotnet new blazingstoryserver` command.

> [!Note]  
> To use the MCP server feature, you need to run the `dotnet new blazingstoryserver -mcp` command.
> Note that the MCP server feature is not available in the Blazing Story app when running on Blazor WebAssembly.

The file layout will be the following tree.

```
📂 (working directory)
    + 📄 MyBlazorWasmApp1.sln
    + 📂 MyBlazorWasmApp1
        + ...
    + 📂 MyBlazorWasmApp1.Stories
        + 📄 MyBlazorWasmApp1.Stories.csproj✨ 👈 Add this
```

#### Step 2 - Add a project reference of the Blazor Wasm app to the Blazing Story project

Next, add a project reference in the Blazing Story App project "MyBlazorWasmApp1.Stories" that refers to the Blazor WebAssembly app project "MyBlazorWasmApp1".

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/add-a-project-reference.png)

If you are working on dotnet CLI, you can do that with the following commands in a terminal.

```shell
dotnet add ./MyBlazorWasmApp1.Stories reference ./MyBlazorWasmApp1
```

```
📂 (working directory)
    + 📄 MyBlazorWasmApp1.sln
    + 📂 MyBlazorWasmApp1 <--- refers --+
        + ...                           |
    + 📂 MyBlazorWasmApp1.Stories ------+
        + ...
```

#### Step 3 - Add a "stories" file

Add a new "stories" file to the Blazing Story App project "MyBlazorWasmApp1.Stories".

A "stories" file is a normal Razor Component file (.razor), but it is annotated with the `[Stories]` attribute and includes a markup of the `<Stories>` component. There is no restriction on file layout of "stories" files, but usually, we place it in the "Stories" subfolder.

> [!Warning]  
> Currently, The file name of the "stories" files must end with ".stories.razor". This is a requirement of the naming convention for available the "Show code" feature in the "Docs" pages.

In this example scenario, we are going to write a "stories" for the `Button` component lived in the "MyBlazorWasmApp1" project, so we would add a new story file named "Button.stories.razor" in the "Stories" subfolder where is under the "MyBlazorWasmApp1.Stories" project.

```
📂 (working directory)
    + 📄 MyBlazorWasmApp1.sln
    + 📂 MyBlazorWasmApp1
        + ...
    + 📂 MyBlazorWasmApp1.Stories
        + 📄 MyBlazorWasmApp1.Stories.csproj
        + 📂 Stories
            + 📄 Button.stories.razor✨ 👈 Add this
```

#### Step 4 - Implement the "stories"

Implement a stories.

The "Button.stories.razor" would be like the below.

```html
@using MyBlazorWasmApp1.Components
@attribute [Stories("Components/Button")]

<Stories TComponent="Button">

    <Story Name="Primary">
        <Template>
            <Button Label="Button" Primary="true" @attributes="context.Args" />
        </Template>
    </Story>

</Stories>
```

#### Step 5 - Configuring a Subpath for Blazor Storybook

To run the Blazing Story app under a subpath (e.g., `/blazor`), you need to configure the application to handle routing and static file serving correctly. This is particularly important for Blazor Server apps or when deploying to environments like IIS or Azure, where the app may run under a subpath.

1. **Update `Program.cs`**:
   - Configure the subpath (e.g., `/blazor`) in `appsettings.json`, `appsettings.Development.json`, or via an environment variable (`BlazorSubpath`).
   - Use `UsePathBase` to strip the subpath from requests, ensuring Blazor routing works correctly.
   - Add `UseStaticFiles` in the branched pipeline to serve static files (e.g., JavaScript, CSS) under the subpath.
   - Include `UseAntiforgery` in the branch to support Blazor Server's anti-forgery requirements.
   - Example `Program.cs` configuration:

```csharp
using BlazingStory.Components;
using BlazingStory.Configurations;
using BlazorStorybook.Components.Pages;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Razor components with interactive server-side rendering enabled
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient with base server URL (read from config or fallback to default localhost port)
var serverUrl = builder.Configuration[WebHostDefaults.ServerUrlsKey]?.Split(';').FirstOrDefault() ?? "https://localhost:7248";
builder.Services.TryAddScoped(sp => new HttpClient { BaseAddress = new Uri(serverUrl) });

// Add health checks (e.g. for monitoring / container orchestration readiness/liveness probes)
builder.Services.AddHealthChecks();

// Read optional subpath for mounting the Blazor app (config key: "BlazorSubpath"), default to "/"
string subPath = builder.Configuration["BlazorSubpath"] ?? "/";
if (string.IsNullOrWhiteSpace(subPath))
{
    subPath = "/"; // Ensure root fallback
}
else if (!subPath.StartsWith('/'))
{
    throw new InvalidOperationException("BlazorSubpath must start with '/'.");
}

// Normalize subpath by trimming trailing slashes (except when it's root "/")
subPath = subPath == "/" ? subPath : subPath.TrimEnd('/');

// Register the normalized subpath as a singleton config for DI
builder.Services.AddSingleton(new BlazorSubpathConfig { Subpath = subPath });

// Build the application
var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true); // Production error handling
    app.UseHsts(); // Enable HTTP Strict Transport Security
}

app.UseHttpsRedirection();

// Allow the app to be embedded in iframes (by overriding X-Frame-Options)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "ALLOWALL";
    await next();
});

app.UseStaticFiles(); // Serve global static files (wwwroot)
app.UseRouting(); // Enable routing
app.MapHealthChecks("/health"); // Expose health endpoint

// Redirect root ("/") requests to the configured subpath (so Blazor is always mounted under subPath)
app.MapGet("/", context =>
{
    context.Response.Redirect($"{subPath}/");
    return Task.CompletedTask;
});

// Mount Blazor app at the configured subpath with a branched middleware pipeline
app.Map(subPath, branch =>
{
    branch.UsePathBase(subPath); // Adjusts request path base so routing works under subpath
    branch.UseStaticFiles();     // Serve Blazor static assets (_content, _framework, etc.)
    branch.UseRouting();         // Enable routing for the Blazor app
    branch.UseAntiforgery();     // Protect against CSRF
    branch.UseEndpoints(endpoints =>
    {
        // Map Blazor storybook server component (IndexPage inside iframe page container)
        endpoints.MapRazorComponents<BlazingStoryServerComponent<IndexPage, IFramePage>>()
            .AddInteractiveServerRenderMode();
    });
});

// Run the application
await app.RunAsync();
```

2. **Configure Subpath**:
   - **Option 1: Update `appsettings.Development.json`**:
     Set the subpath to `/blazor` to match your deployment.
     ```json
     {
       "DetailedErrors": true,
       "Logging": {
         "LogLevel": {
           "Default": "Information",
           "Microsoft.AspNetCore": "Debug"
         }
       },
       "BlazorSubpath": "/blazor"
     }
     ```
   - **Option 2: Environment Variable (overrides `appsettings.json`)**:
     - Set via command line:
       ```shell
       dotnet run --BlazorSubpath=/custompath
       ```
     - Set in OS:
       - Linux/Mac: `export BlazorSubpath=/custompath`
       - Windows: Set `BlazorSubpath` in Environment Variables
     - In Docker:
       ```shell
       docker run -e BlazorSubpath=/app ...
       ```

3. **Update the IndexPage.razor and the IFramePage.razor**:
   - Replace `<base href="@(new Uri(NavigationManager.BaseUri).AbsolutePath)" />` with `<base href="@(new Uri(NavigationManager.ToAbsoluteUri(SubpathConfig.Subpath + "/").ToString()).AbsolutePath)" />`
   - Add also `@inject BlazorSubpathConfig SubpathConfig`

4. **Fix Routing and Static File Issues**:
   - If you encounter routing errors like `AmbiguousMatchException`, ensure only one `@page` directive is used in `BlazingStoryServerComponent.razor` (e.g., `@page "/{*urlPath}"` without `@page "/"`).
   - If JavaScript/CSS files (e.g., `iframe-communication.js`, `blazor.web.js`) fail to load with MIME type errors, verify `UseStaticFiles()` is in the branched pipeline after `UsePathBase`.

5. **Build and Run Commands**:
   - Clean and rebuild the project to ensure no stale artifacts:
     ```shell
     dotnet clean
     dotnet build
     ```
   - Run the Blazing Story app:
     ```shell
     dotnet run --project ./MyBlazorWasmApp1.Stories
     ```
   - For hot reloading (preview, unstable):
     ```shell
     dotnet watch --project ./MyBlazorWasmApp1.Stories
     ```
     Note: If the app freezes (e.g., on Docs pages), restart with `Ctrl+R` in `dotnet watch`.

6. **Testing**:
   - Access `https://localhost:7248/` (adjust port as needed) to redirect to `/blazor/`.
   - Verify `/blazor/` loads `IndexPage.razor` and `/blazor/iframe.html` loads `IFramePage.razor`.
   - Check the Network tab in browser dev tools (F12) to ensure static files (e.g., `/blazor/js/iframe-communication.js`, `/blazor/_framework/blazor.web.js`) return 200 with `application/javascript` MIME type.

#### Step 6 - Run it!

If you are working on Visual Studio, right-click the "MyBlazorWasmApp1.Stories" project in the solution explorer to show the context menu, click the "Set as Startup Project" menu item, and hit the `Ctrl` + `F5` key.

If you are working on dotnet CLI, you can do that with the following commands in a terminal.

```shell
dotnet run --project ./MyBlazorWasmApp1.Stories
```

Then you will see the clone of the "Storybook" built on Blazor! 🎉

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/first-run-of-blazingstory.gif)

## 📓 Documentation

For more details about Blazing Story, please refer to the following documentation site. You can access more advanced features of Blazing Story by reading the documentation.

- [Blazing Story Documentation Site](https://blazingstory.github.io/docs/structure-of-stories)

## 📌 System Requirements

.NET SDK ver.8 or later

## 🙇 Disclaimer

The "Blazing Story" is just my hobby work and a personal technical exhibition, so it is not an officially derived product from the "Storybook". I'd like to keep improving this product for now, but it might be abandoned if I cannot spend enough time on this product in the future. I welcome that somebody forks this product and organizes the maintainers for it.

From another perspective, the "Blazing Story" is an almost 100% Blazor-native application that re-implemented  Storybook's look, behavior, and functionality to mimic it. However, this means that **none of the Storybook community's contributions, numerous add-ins, and related services are available in the "Blazing Story"**.

Therefore, the combination of Blazor's WebComponents custom element and Storybook might be more promising for the future of UI component catalog solutions for Blazor application development than the "Blazing Story".

However, on the "Blazing Story" side, Blazor application developers can get a Storybook-like UI component catalog solution under the familiar .NET ecosystem without being annoyed by a complex JavaScript front-end development toolchain. This is the most important point I wanted to illustrate through the development of Blazing Story.


## 🤔 Frequently Asked Questions

**Q:** How can I write or configure addons?  
**A:** You can't do that for now because the add-on architecture is not completed yet. I'll finish it in the future version.

## 🎉 Release Notes

Release notes are [here](https://github.com/jsakamoto/BlazingStory/blob/main/RELEASE-NOTES.txt).

## ⚠️ Attention

Assembly files of the Blazing Story app include the project file path string that the app was built as its metadata.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/project-dir-in-assembly-metadata.png)

If you need the path to the project should be secret, you may have to avoid using Blazing Story.

## 📢 License & Third Party Notice

[Mozilla Public License Version 2.0](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/LICENSE)

The third party notice is [here](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/THIRD-PARTY-NOTICES.txt).
