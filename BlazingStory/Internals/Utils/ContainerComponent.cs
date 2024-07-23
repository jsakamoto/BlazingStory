using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// This class is a container component that is used to render a RenderFragment.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
internal class ContainerComponent<T> : ComponentBase
{
    #region Public Properties

    public RenderFragment? Content { get; set; }

    #endregion Public Properties

    #region Internal Methods

    internal void BuildTree(RenderTreeBuilder builder)
    {
        this.BuildRenderTree(builder);
    }

    #endregion Internal Methods

    #region Protected Methods

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (this.Content != null)
        {
            builder.AddContent(0, this.Content);
        }
    }

    #endregion Protected Methods
}
