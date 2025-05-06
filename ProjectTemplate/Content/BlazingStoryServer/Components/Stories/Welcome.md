---
$attribute: CustomPage("Examples/Welcome")
---

# <img src="https://raw.githubusercontent.com/jsakamoto/BlazingStory/main/assets/icon.min.64x64.svg" style="vertical-align: middle;" /> Welcome to Blazing Story!

Blazing Story is a UI component explorer for Blazor - helping you build, document, and test your UI components in isolation.

## 🚀Getting Started

### 📝Adding Your First Story

Adding a new component story is simple:

1. **Create a story file** in your Stories folder with the `.stories.razor` extension
2. **Implement your story** with the Stories component

> [!Warning]  
> Currently, The file name of the "stories" files must end with `.stories.razor`. This is a requirement of the naming convention for available the "Show code" feature in the "Docs" pages.

### 📝Example Story File

```html
@using YourNamespace.Components
@attribute [Stories("Components/Button")]
<Stories TComponent="Button">

  <Story Name="Primary">
    <Template>
      <Button Label="Button" Primary="true" @attributes="context.Args" />
    </Template>
  </Story>

  <Story Name="Secondary">
    <Arguments>
      <Arg For="_ => _.Primary" Value="false" />
    </Arguments>
    <Template>
      <Button Label="Button" @attributes="context.Args" />
    </Template>
  </Story>

</Stories>
```

## 🧩Story Structure

- Use `[Stories]` attribute to define the navigation path
- The `<Stories>` component specifies your target component
- Each `<Story>` represents a different state or variant
- The `context.Args` connects user input from the Controls panel

Explore the sidebar to see documentation, controls, and more features!