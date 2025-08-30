# ![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/icon.min.64x64.svg) Blazing Story

[![tests](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml/badge.svg)](https://github.com/jsakamoto/BlazingStory/actions/workflows/tests.yml) [![NuGet Package](https://img.shields.io/nuget/v/BlazingStory.svg)](https://www.nuget.org/packages/BlazingStory/) [![Discord](https://img.shields.io/discord/798312431893348414?style=flat&logo=discord&logoColor=white&label=Blazor%20Community&labelColor=5865f2&color=gray)](https://discord.com/channels/798312431893348414/1202165955900473375)

## 📝 Summary

The clone of ["Storybook"](https://storybook.js.org/) for Blazor, a frontend workshop for building UI components and pages in isolation.

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

#### Step 5 - Run it!

If you are working on Visual Studio, right-click the "MyBlazorWasmApp1.Stories" project in the solution explorer to show the context menu, click the "Set as Startup Project" menu item, and hit the `Ctrl` + `F5` key.

If you are working on dotnet CLI, you can do that with the following commands in a terminal.

```shell
dotnet run --project ./MyBlazorWasmApp1.Stories
```

Then you will see the clone of the "Storybook" built on Blazor! 🎉

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/first-run-of-blazingstory.gif)

## 📂 Structure of "stories"

- The `[Stories]` attribute on a Razor component indicates to the Blazing Story runtime that the Razor component will include stories.
- The slash-separated title string of the parameter of the `[Stories]` attribute configures the navigation tree and represents the title of the stories container (we called it "Component").
- The `<Stories>` component indicates to the Blazing Story runtime what is the target component of the story with its `TComponent` type parameter. The "Controls" panel will be built from this component-type information.
- The `<Stories>` component can include one or more `<Story>` components. The `<Story>` component has the `Name` parameter, which will be shown in the sidebar navigation tree to identify each story.
- The `<Template>` render fragment parameter inside the `<Story>` component will be rendered in the canvas area when that story is selected in the sidebar navigation tree.
- The Blazing Story runtime passes the parameters users inputted from the "Control" panel through the `context.Args` inside the `<Template>` render fragment, so you should apply it to the component by using the `@attributes=...`,  [attribute splatting syntax](https://learn.microsoft.com/aspnet/core/blazor/components/#attribute-splatting-and-arbitrary-parameters).

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/structure-of-story.png)

## ⚙️ Configure layouts

If you want to apply the same layout for every story, you can specify the layout component to use when displaying a story. This architecture is mostly the same as Blazor's standard layout mechanism (see also: _["ASP.NET Core Blazor layouts | Microsoft Learn"](https://learn.microsoft.com/aspnet/core/blazor/components/layouts)_). A layout component must inherit from the `LayoutComponentBase` class and should have the rendering `@Body` in its markup. If you apply the layout component like the one below to stories,

```html
@inherits LayoutComponentBase
<YourThemeProvider>
    <div style="padding: 24px;">
        @Body
    </div>
</YourThemeProvider>
```

The story which applied the layout component above will be rendered as a child component of the `<YourThemeProvider>` component (imagine you implemented that component such a component including some cascading values) and will have 24-pixel padding.

### Application level layout

The specified layout component for stories is in multiple levels.

First, you can specify the layout for the application level via the `DefaultLayout` property of the `BlazingStoryApp` component, which is usually marked up in your `App.razor` file. 

```html
@* 📄App.razor *@
<BlazingStoryApp ... DefaultLayout="typeof(DefaultLayout)">
</BlazingStoryApp>
```

In the above case, the layout component `DefaultLayout.razor` will be used when displaying every story.

### Component (Stories) level layout

Second, you can specify the layout for the component (stories) level via the `Layout` parameter of the `<Stories>` component.

```html
@* 📄...stories.razor *@
@attribute [Stories(...)]

<Stories ... Layout="typeof(StoriesLayout)">
    ...
```

In the above case, when displaying stories within the `<Stories>` markup, the layout component `StoriesLayout.razor` will be utilized.

### Story level layout

Third, you can specify the layout for the story level via the `Layout` parameter of the `<Story>` component.

```html
@* 📄...stories.razor *@
@attribute [Stories(...)]

<Stories ...>
    <Story Name="..." Layout="typeof(StoryLayout)">
        ...
```

In the above case, when displaying the story named "...", the layout component `StoryLayout.razor` will be utilized.

### The order of applying the layouts

The order of applying the layout component is the application level layout will use on the outermost level, and the story level layout will use on the innermost level. The component (stories) level layout will use on the middle level of them.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/order-of-applying-layouts.png)

## ⚙️ Configure arguments 

### Configure the control type

You can configure the type of control for the component parameters via the `<ArgType>` component inside the `<Stories>` component. The `For` lambda expression parameter of the `<ArgType>` component indicates which parameters to be applied the control type configuration. And the `Control` parameter of the `<ArgType>` component is used for specifying which type of control uses in the "Control" panel to input parameter value.

For example, if you apply the `ControlType.Color` control type to the `BackgroundColor` parameter like this, 

```html
<Stories TComponent="Button">
    <ArgType For="_ => _.BackgroundColor" Control="ControlType.Color" />
    ...
```

you will be able to specify the component's background color with the color picker, like in the following picture.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/control-type-colorpicker.png)

For another example, you can make the "Control" panel use dropdown list control to choose one of the values of the `enum` type instead of the radio button group control, with the following code.

```html
<Stories TComponent="Button">
    ....
    <ArgType For="_ => _.Size" Control="ControlType.Select" />
    ...
```

Then, you will get the result in the following picture.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/control-type-dropdownlist.png)

### Configure the initial value of the component parameter

You can specify the initial value of the component parameter by using the `<Arg>` component inside the `<Arguments>` render fragment parameter of the `<Story>` component like this:

```html
    ...
    <Story Name="Secondary">
        <Arguments>
            <Arg For="_ => _.Primary" Value="false" />
        </Arguments>
        ...
```

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/configure-arguments-arg.png)

### Configure component parameters which type is `RenderFragment`

Blazing Story supports the `RenderFragment` type parameter in its control panel. For example, if you have a story for a component such as having a `RenderFragment ChildContent` parameter, you can dynamically set text to that `ChildContent` parameter from the control panel UI at runtime.

(NOTICE: Currently, Blazing Story allows you to set only text to the `RenderFragment` type parameter in its control panel UI. You cannot set fragments consisting of other components or HTML tags to the `RenderFragment` type parameter. This is a limitation of Blazing Story.)

However, if you mark up the `ChildContent` parameter inside of the component's markup, you will not be able to set text to that parameter from the control panel UI. Because the `ChildContent` parameter is already set with the component's markup.

```html
    ...
    <Story Name="Default">

        <Template>
            <MyButton @attributes="context.Args">
                <!-- ❌ DON'T DO THIS! -->
                Click me
            </MyButton>
        </Template>
        ...
```

Instead, you should set the `ChildContent` parameter through the `<Arguments>` render fragment parameter of the `<Story>` component, like below.

```html
    <!-- 👍 DO THIS! -->
    ...
    <Story Name="Default">

        <Arguments>
            <Arg For="_ => _.ChildContent" Value="_childContent" />
        </Arguments>

        <Template>
            <MyButton @attributes="context.Args">
            </MyButton>
        </Template>
        ...
@code
{
    RenderFragment _childContent = @<text>Click me</text>;
}
```

## ✍️ Documentation Enhancement

By default, the "Docs" pages in Blazing Story do not contain detailed descriptions.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/docs-page-with-no-description.png)

### Adding Descriptions to Parameters

To add more detailed descriptions of component parameters to the "Docs" pages, write a summary of the component parameters in your UI component’s .razor file using the ["XML document comment"](https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/) format, as shown below.

```csharp
@code {
    ...
    // 👇 Add these triple slash comments.
    //    See also: https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/

    /// <summary>
    /// Set a text that is button caption.
    /// </summary>
    [Parameter, EditorRequired]
    public string? Text { get; set; }

    /// <summary>
    /// Set a color of the button. <see cref="ButtonColor.Default"/> is default.
    /// </summary>
    [Parameter]
    public ButtonColor Color { get; set; } = ButtonColor.Default;
    ...
```

And next, enable generating an XML documentation file in your UI component library's project file (.csproj).

If you are working on Visual Studio, you can do that by turning on the option `Documentation file - Generate a file containing API documentation" from the property GUI window of the project (You can find that option in the "Build" > "Output" category. You can also find it more easily by searching with the "Documentation file" keyword inside the project property GUI window).

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/configure-xml-doc-comment.png)

Or, you can also do that by adding the `<GenerateDocumentationFile>` MSBuild property with `true` in a project file (.csproj) of your UI component library, like below.

```xml
<!-- 📄 This is a project file (.csproj) of your UI component library -->
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    ...
    <!-- 👇 Add this entry. -->
    <GenerateDocumentationFile>True</GenerateDocumentationFile>
    ...
```

After doing that, the XML documentation comments you added to parameters will appear on the "Docs" pages.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/docs-page-with-parameter-description.png)

### Adding Descriptions to Component

To add a description for the component itself, rather than for each of its parameters, you have to create a partial class file for the .razor file.

For example, if you have a "Button.razor" component, you should add a corresponding "Button.razor.cs" file to your project and include the component's summary in that .razor.cs file, as shown below.

```csharp
// 📄 This is a partial class file "Button.razor.cs" 
//     of the "Button.razor" Razor component file.

// 👇 Add the triple slash comments to the component class.

/// <summary>
/// This is basic button component.
/// </summary>
public partial class Button
{
}
```

After doing that, the XML documentation comments you added will appear on the "Docs" pages.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/docs-page-with-component-description.png)

### Adding Descriptions to Individual Stories

You can provide descriptions for individual stories in addition to component and parameter descriptions. This is useful for explaining a particular story's specific context or purpose.

To add a description to a story, use the `<Description>` render fragment parameter within the `<Story>` component in your `.stories.razor` file. The content of the `<Description>` render fragment will be rendered in the "Docs" page, below the story's name.

Here's an example of how to add a description to a story:

```html
    ...
    <Story Name="Default">
        <!--
        👇 Add a description for this story inside 
        the <Description> render fragment, as shown below:
        -->
        <Description>
            <b>Description:</b>
            <span>
            This section describes the usual usage of
            the <code>&lt;IconButton&gt;</code> component.
            </span>
        </Description>
        ...
```

This will display the provided HTML content as the description for the "Default" story on the "Docs" page, as shown below:

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/docs-page-with-story-description.png)

You can use any HTML markup within the `<Description>` render fragment to format your story's description.

## 📝 Custom Pages & Markdown

Blazing Story allows you to add custom pages that can include any content you want. These pages will be shown in the sidebar's navigation tree. It is helpful for adding a "Welcome" page, "Getting Started" page, or any other custom page. The custom pages are a Razor component so you can use any Blazor features.

You can add a custom page by creating a new Razor component file with the `[CustomPage]` attribute, like below. (The pages only need the `CustomPage` attribute, no filename requirements like "page.custom.razor.")

```csharp
@attribute [CustomPage("Examples/Getting Started")]

<h2>🚀 Getting Started</h2>
<h3>Example scenario</h3>
<p>For the example scenario, you already have a Blazor WebAssembly application project, "MyBlazorWasmApp1", that includes the "Button" component.</p>
...

```

After creating a custom page like the one above, you will see the custom page like the following example.

![Example of Custom Pages](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/example-of-custom-pages.png)

You can set the custom page's position in the sidebar navigation tree by specifying the title string of the `CustomPage` attribute parameter, which includes a slash separator. The slash separator works to represent the hierarchy of the sidebar navigation tree, like the one inside the `Stories` attribute parameter.

### Using Markdown files for Custom Pages

In addition to Razor components, Blazing Story now supports using **pure** Markdown files (.md) as Custom Pages through integration with the [**"MD2RazorGenerator"**](https://github.com/jsakamoto/MD2RazorGenerator) NuGet package [![NuGet Package](https://img.shields.io/nuget/v/MD2RazorGenerator.svg)](https://www.nuget.org/packages/MD2RazorGenerator/). This allows you to write documentation in Markdown rather than Razor syntax for a more streamlined authoring experience.

To use Markdown files as Custom Pages:

1. Add the "MD2RazorGenerator" NuGet package to your Blazing Story project:

```
dotnet add package MD2RazorGenerator
```

2. Create Markdown (.md) files in your project and use YAML frontmatter with the `$attribute` key to specify the `CustomPage` attribute:

```markdown
---
$attribute: CustomPage("Examples/Getting Started")
---
## 🚀 Getting Started<
### Example scenario
For the example scenario, you already have a Blazor WebAssembly application project, "MyBlazorWasmApp1", that includes the "Button" component.
...
```

3. The MD2RazorGenerator will automatically convert your Markdown files into Razor components with the `CustomPage` attribute applied.

This approach combines the simplicity of Markdown for documentation with the power of Blazor's component model.

## ↕️ Sorting the navigation tree items

By default, items at the same level in the sidebar navigation tree are sorted alphabetically by title, with containers/components listed before custom pages. (Even if a custom page’s title would come first alphabetically, it appears after all containers/components.)

To override the default order, set the `NavigationTreeOrder` parameter of the `BlazingStoryApp` in your `App.razor`. This parameter accepts a declarative list built with `NavigationTreeOrderBuilder.N[...]`.

Rules:
- Docs and Story items under a component keep their original order and are always listed first.
- If an item is immediately followed by an `N[...]` group, that group specifies the order of that item’s children (applied recursively).
- Unspecified siblings are appended in the default order (alphabetical, components before custom pages).

Basic example (top-level and nested ordering):

```razor
@* 📄 App.razor *@
@using static BlazingStory.Types.NavigationTreeOrderBuilder

<BlazingStoryApp Assemblies="[typeof(App).Assembly]" 
    NavigationTreeOrder="@N[
      "Welcome",                    // top-level items 
      "Components", 
         N["Layouts",               // children under "Components" 
             N["Header", "Footer"], // children under "Layouts" 
           "Button"], 
      "Templates"]" />
```

Notes:

- Titles must match the captions shown at that level (e.g., top-level “Components”, then child “Layouts”, etc.).
- Even if you list custom pages inside a component’s children, they still appear after Docs and Story items for that component.
- Any items not mentioned in `NavigationTreeOrder` remain sorted by the default.
 
## ⚙️ Configure prefers color scheme

By default, the "Blazing Story" app respects the system's color scheme, such as "dark" or "light". If you want to keep the "Blazing Story" app's color scheme to be either "dark" or "light" regardless of the system's color scheme, you can do that by setting the `AvailableColorScheme` parameter of the `BlazingStoryApp` component in your `App.razor` file.

If you set that parameter with `Dark` or `Light`, except `Both`, the "Blazing Story" app will be displayed with the specified color scheme regardless of the system's color scheme.

```razor
@* 📄 App.razor *@
<BlazingStoryApp 
    Assemblies="[typeof(App).Assembly]"  
    AvailableColorSchemes="AvailableColorSchemes.Light">
    @* This app will be displayed "light" mode only 👆*@
</BlazingStoryApp>
```

## 🪄 Include custom CSS or JavaScript files for your stories

If you need to add `<link>` or `<script>` elements to include CSS or JavaScript files for canvas views of your stories, you should do that in the **`iframe.html`** file, not in the `index.html` file.

## 🎀 Customize the title and brand logo

### Customize the title

If you want to customize the title of your Blazing Story app, set the `Title` parameter of the `BlazingStoryApp` component in your `App.razor` file.

```html
@* 📄 App.razor *@
<BlazingStoryApp Title="My Story" ...>
</BlazingStoryApp>
```

The string value specified in the `Title` parameter of the `BlazingStoryApp` will appear in the title of your Blazing Story app document and in the brand logo area placed at the top of the sidebar.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/change-the-title.png)

### Customize the brand logo

You can also replace the brand logo contents at the top of the sidebar entirely by marking up the `BrandLogoArea` render fragment parameter of the `BlazingStoryApp` component, like below.

```html
@* 📄 App.razor *@
<BlazingStoryApp Title="My Story" ...>

    <!-- replace the brand logo contents entirely. -->
    <BrandLogoArea>
        <div style="font-family:'Comic Sans MS';">
            @context.Title
        </div>
    </BrandLogoArea>

</BlazingStoryApp>
```

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/customize-brand-logo.png)

You can refer to the `BlazingStoryApp` component instance via the `context` argument, so you can retrieve the title string, which is specified in the `Title` parameter of the `BlazingStoryApp` component.

> [!Note]  
> The 'BlazingStoryApp' component instance is also provided as a cascade parameter value. So you can get the reference to the instance of the `BlazingStoryApp` component anywhere in your Razor component implemented in your Blazing Story app.

If you want to only customize the logo icon or the URL of the link, not want to change the entire brand logo HTML contents, you can use the `BrandLogo` built-in component instead. The `BrandLogo` component will render the default design of the brand logo of the Blazing Story app and expose some parameters, such as `IconUrl`, `Url`, and `Title`, to customize them.

```html
@* 📄 App.razor *@
<BlazingStoryApp Title="My Story" ...>
    <BrandLogoArea>
        <BrandLogo IconUrl="https://github.githubassets.com/apple-touch-icon-76x76.png" Url="..." />
    </BrandLogoArea>
</BlazingStoryApp>
```

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/customize-brand-logo-icon.png)

> [!Note]  
> The `BrandLogo` component uses the `Title` parameter of the `BlazingStoryApp` component by default for rendering the title text inside it. But if the `BrandLogo` component's `Title` parameter is specified, the `BrandLogo` component uses it rather than the `Title` parameter of the `BlazingStoryApp` component.

## 🔥 Hot Reloading [Preview] [Unstable]

Unfortunately, the hot reloading feature won't work well on the "Blazing Story". Now, contributors are working on enabling the hot reloading feature for Blazing Story, but it is not completed yet.

However, if you want to try it, you can do that by setting the `EnableHotReloading` parameter of the `BlazingStoryApp` component to `true` explicitly in your `App.razor` file.

```html
<BlazingStoryApp EnableHotReloading="true">
    <!-- ... -->
</BlazingStoryApp>
```

After doing that, you will see the changes you made in the "stories" files, and component library projects will be reflected in the preview area of "Blazing Story" without restarting the app.

But please remember that it is really unstable. In our experience, it doesn't work on a "Doc" page. Visiting a "Doc" page often stops the entire "Blazing Story" app. Once it happens, there is no way to recover it except to re-launch the app as far as we know (when you use the `dotnet watch` command, hit Ctrl + R).

Therefore, the hot reloading feature is still a preview feature. We are working on it, but we cannot guarantee that it will work well in the future.

## 🎛️ MCP Server Feature

### Summary

Blzing Story provides a **MCP server** feature that allows Blazing Story to expose information about its components and stories to AI agents, enabling highly accurate code generation.

To catch up he power of the Blazing Story's MCP server feature, you can watch the following introduction video.

https://github.com/user-attachments/assets/1e4ced81-8d06-4714-ad89-4b0987d958c9

Currently, the MCP server feature is available only in the Blazing Story app running on Blazor Server. It is not available in the Blazing Story app running on Blazor WebAssembly.

### Create a new Blazing Story app with the MCP server feature

When you create a new Blazing Story app project, you can enable the MCP server feature by checking the "Enable the MCP server feature" checkbox in the project creation dialog.

![Creating a new Blazing Story app with the MCP Server option](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/add-a-new-project-mcp-option.png)

Or, if you are working on dotnet CLI, you can do that by running the `dotnet new blazingstoryserver` command with the `-mcp` option, like below:

```shell
dotnet new blazingstoryserver -mcp
```

### Migration your Blazing Story app to enable the MCP server feature

To enable the MCP server feature in your existing Blazing Story app, you need to follow these steps:

1. Upgrade your Blazing Story app project to reference the latest version of the Blazing Story NuGet package.

2. Add the `BlazingStory.McpServer` package reference to your Blazing Story app project.

   If you are working on Visual Studio, you can do that by right-clicking the project in the solution explorer and selecting "Manage NuGet Packages...", then searching for the `BlazingStory.McpServer` package and installing it.

   If you are working on dotnet CLI, you can do that with the following command:

   ```shell
   dotnet add package BlazingStory.MCPServer
   ```

3. Add the `AddBlazingStoryMcpServer` method call to the `Program.cs` file of your Blazing Story app project to register the MCP server services in the dependency injection container.

   ```csharp
   // 📄 Program.cs
   ...
   // 👇 Add the necessary using directive for the MCP server.
   using BlazingStory.McpServer;
   ...
   var builder = WebApplication.CreateBuilder(args);
   ...
    // 👇 Add services to the container.
   builder.Services.AddBlazingStoryMcpServer();
   ...
   ```  
4. Add the `MapBlazingStoryMcp` method call to the `app` object in the `Program.cs` file to register the Blazing Story MCP server middleware.

   ```csharp
   // 📄 Program.cs
   ...
   app.UseHttpsRedirection();

   // 👇 Register the Blazing Story MCP server middleware.
   app.MapBlazingStoryMcp();
   app.MapStaticAssets();
   app.UseRouting();
   app.UseAntiforgery();
   ...
   ```

### Usage

The tranport type of the MCP server feature of the Blazing Story is **Streamable HTTP** and **SSE**. It doesn't support STDIO transport type for now.

Once you have enabled the MCP server feature in your Blazing Story app, you can access the MCP server at the `/mcp/blazingstory` endpoint of your Blazing Story app for Streamable HTTP access, or at the `/mcp/blazingstory/sse` endpoint for SSE access.

## 📌 System Requirements

.NET SDK ver.8 or later

## 🙇 Disclaimer

The "Blazing Story" is just my hobby work and a personal technical exhibition, so it is not an officially derived product from the "Storybook". I'd like to keep improving this product for now, but it might be abandoned if I cannot spend enough time on this product in the future. I welcome that somebody forks this product and organizes the maintainers for it.

From another perspective, the "Blazing Story" is an almost 100% Blazor-native application that re-implemented  Storybook's look, behavior, and functionality to mimic it. However, this means that **none of the Storybook community's contributions, numerous add-ins, and related services are available in the "Blazing Story"**.

Therefore, the combination of Blazor's WebComponents custom element and Storybook might be more promising for the future of UI component catalog solutions for Blazor application development than the "Blazing Story".

However, on the "Blazing Story" side, Blazor application developers can get a Storybook-like UI component catalog solution under the familiar .NET ecosystem without being annoyed by a complex JavaScript front-end development toolchain. This is the most important point I wanted to illustrate through the development of Blazing Story.


## 🤔 Frequently Asked Questions

**Q:** How can I write or configure addons?  
**A:** You can't do that for now because the addon architecture is not completed yet. I'll finish it in the future version.

## 🎉 Release Notes

Release notes are [here](https://github.com/jsakamoto/BlazingStory/blob/main/RELEASE-NOTES.txt).

## ⚠️ Attention

Assembly files of the Blazing Story app include the project file path string that the app was built as its metadata.

![](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/readme-images/project-dir-in-assembly-metadata.png)

If you need the path to the project should be secret, you may have to avoid using Blazing Story.

## 📢 License & Third Party Notice

[Mozilla Public License Version 2.0](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/LICENSE)

The third party notice is [here](https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/THIRD-PARTY-NOTICES.txt).
