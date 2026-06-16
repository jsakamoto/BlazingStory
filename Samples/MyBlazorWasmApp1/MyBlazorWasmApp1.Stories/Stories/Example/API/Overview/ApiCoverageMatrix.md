---
$attribute: CustomPage("Example/API/Overview/Coverage Matrix")
---

## API coverage matrix

This page maps BlazingStory features to the sample stories that demonstrate them.

| Feature | Where to see it | Notes |
|---|---|---|
| Story grouping with `Stories` attribute | `Button.stories.razor`, `Header.stories.razor`, `Rating.stories.razor` | Group names organize the left navigation tree. |
| Component-level layout | `Button.stories.razor`, `Header.stories.razor` | Uses `<Stories Layout="typeof(...)">`. |
| Story-level layout override | `PlaygroundCard.stories.razor` | Uses `<Story Layout="typeof(...)">` for a single scenario. |
| Args (`Arg`) binding | `Button.stories.razor`, `NullableArgsProbe.stories.razor` | Demonstrates bool, int, string, enum, and render fragment args. |
| Unmatched attribute args | `PlaygroundCard.stories.razor` | Passes dictionary values to `CaptureUnmatchedValues`. |
| Control customization (`ArgType`) | `Button.stories.razor`, `Rating.stories.razor` | Color and radio controls. |
| Explicit default control (`ControlType.Default`) | `NullableArgsProbe.stories.razor` | Demonstrates explicit default controller selection. |
| Enum controls (`Select` vs `Radio`) | `EnumControlProbe.stories.razor` | Side-by-side nullable enum behavior. |
| Long enum behavior | `IconCatalogProbe.stories.razor` | Demonstrates long enum list and the UX difference between select and radio. |
| `ArgType DefaultValue` | `PlaygroundCard.stories.razor`, `Rating.stories.razor` | Sets initial control values in UI. |
| Nullable arg behavior | `NullableArgsProbe.stories.razor` | Null defaults, explicit values, and reset-to-null scenarios. |
| Array parameter controller | `ControllerInputTypesProbe.stories.razor` | Covers string[], int[], bool[], and enum[] editing. |
| RenderFragment parameter controller | `ControllerInputTypesProbe.stories.razor` | Covers fragment text editing path for RenderFragment args. |
| Story descriptions | `Button.stories.razor`, `PlaygroundCard.stories.razor` | Uses `<Description>` for story intent. |
| `EventCallback` interaction | `Button.stories.razor`, `Header.stories.razor` | Shows callback wiring and local state updates. |
| `EventCallback<T>` interaction | `PlaygroundCard.stories.razor` | Demonstrates typed callback synchronization. |
| Markdown custom pages | `Configure.md`, `ExamplesOverview.md`, `ApiCoverageMatrix.md` | Uses `CustomPage` attribute in front matter. |

### Coverage intent

The sample prioritizes distinct API demonstrations over visual duplicates.
If a new BlazingStory feature is added, update this matrix and add a focused story scenario for it.
