using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Components;

/// <summary>
/// The Stories component indicates to the Blazing Story runtime what is the target component of the
/// story with its TComponent type parameter. The "Controls" panel will be built from this
/// component-type information. The Stories component can include one or more Story components. The
/// Story component has the Name parameter, which will be shown in the sidebar navigation tree to
/// identify each story.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
[CascadingTypeParameter(nameof(TComponent))]
public partial class Stories<TComponent> : ComponentBase where TComponent : notnull
{
}

public class ArgProp
{
    public string? Name { get; set; }
    public ControlType Control { get; set; } = ControlType.Default;
    public object? DefaultValue { get; set; }
    public string[]? Options { get; set; }
}
