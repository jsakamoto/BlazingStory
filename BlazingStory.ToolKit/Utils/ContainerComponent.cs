using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// This class is a container component that is used to render a RenderFragment.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
internal class ContainerComponent<T> : ComponentBase
{
    [Parameter]
    public RenderFragment? Content { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (this.Content != null)
        {
            builder.AddContent(0, this.Content);
        }
    }
}
