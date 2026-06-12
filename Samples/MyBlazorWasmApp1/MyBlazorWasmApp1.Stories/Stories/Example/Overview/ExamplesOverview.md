---
$attribute: CustomPage("Example/Overview")
---

## Example coverage in this sample app

This sample catalog demonstrates the key BlazingStory patterns you can reuse in your own project.

### Story organization

- Grouped entries using `@attribute [Stories("...")]`
- One component per `.stories.razor` file

### Args and Controls

- `Arg` values for primitive, enum, render fragment, and complex parameters
- Nullable args coverage for bool?, int?, enum?, and string?
- Control customization through `ArgType` (color, select, and radio)
- Explicit `ControlType.Default` example in nullable args stories
- Side-by-side nullable enum controls with both Select and Radio
- Long enum list behavior with Select vs Radio controls
- Default control values through `ArgType DefaultValue`
- Runtime updates through `@attributes="context.Args"`
- Array parameter controller coverage for string, bool, number, and enum arrays
- RenderFragment parameter controller coverage

### Layout strategies

- Component-level layout with `<Stories Layout="typeof(...)">`
- Story-level layout override with `<Story Layout="typeof(...)">`

### Interaction scenarios

- Callback examples in header and button stories
- `EventCallback<T>` synchronization in playground card stories
- Edge cases in rating stories (zero value and value greater than max)

### Story metadata

- Story descriptions through `<Description>` to document intent

### Documentation pages

- Markdown-based custom pages with the `CustomPage` attribute
- API feature-to-story mapping in `Example/API/Overview/Coverage Matrix`

For all BlazingStory features and APIs, see the official docs:

- https://blazingstory.github.io/docs/