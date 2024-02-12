using Microsoft.AspNetCore.Components;

namespace ComponentPack1;

/// <summary>
/// A base class for components that have an identifier.
/// </summary>
public class IdentifiedComponentBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the identifier for the component. See also the MDN document about the <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }
}
