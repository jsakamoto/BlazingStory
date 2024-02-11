using Microsoft.AspNetCore.Components;

namespace ComponentPack1;

/// <summary>
/// A base class for components that have an identifier.
/// </summary>
public class IdentifiedComponentBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the identifier for the component.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }
}
