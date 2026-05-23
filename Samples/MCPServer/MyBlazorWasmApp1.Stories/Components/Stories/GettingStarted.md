---
$attribute: CustomPage("Getting Started")
---

## Getting Started

This UI catalog is built with **Blazing Story**, a clone of [Storybook](https://storybook.js.org/) for Blazor.

### Quick setup

Install the project template and create a new Blazing Story app:

```shell
dotnet new install BlazingStory.ProjectTemplates
dotnet new blazingstorywasm -n MyApp.Stories
dotnet sln add ./MyApp.Stories/
dotnet add ./MyApp.Stories reference ./MyApp
```

### Writing stories

Add a `.stories.razor` file for each component you want to document:

```html
@using MyApp.Components
@attribute [Stories("Components/Button")]

<Stories TComponent="Button">
    <Story Name="Primary">
        <Template>
            <Button Label="Click me" Primary="true" @attributes="context.Args" />
        </Template>
    </Story>
</Stories>
```

### Layouts

You can apply layout components at three levels:

- **Application** — `DefaultLayout` on `BlazingStoryApp`
- **Component** — `Layout` on `<Stories>`
- **Story** — `Layout` on `<Story>`

For full documentation visit [blazingstory.github.io/docs](https://blazingstory.github.io/docs/).
