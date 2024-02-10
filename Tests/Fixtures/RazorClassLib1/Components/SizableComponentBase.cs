using Microsoft.AspNetCore.Components;

namespace RazorClassLib1.Components;

public class SizableComponentBase : ComponentBase
{
    /// <summary>
    /// Set a size of the component. <see cref="ComponentSize.Medium"/> is default.
    /// </summary>
    [Parameter]
    public ComponentSize Size { get; set; }
}
