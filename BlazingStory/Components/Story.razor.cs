using Microsoft.AspNetCore.Components;

namespace BlazingStory.Components;

/// <summary>
/// The story component is used to define a story for a component. A story is a way to render a
/// component in a specific way, with specific parameters. It is used to show how a component can be
/// used in different scenarios.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
[CascadingTypeParameter(nameof(TComponent))]
public partial class Story<TComponent> : ComponentBase where TComponent : notnull
{
}
