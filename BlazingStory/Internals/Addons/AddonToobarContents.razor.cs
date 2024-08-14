using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Addons;

public partial class AddonToobarContents : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
