#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Types;

/// <summary>
/// Provides pre-constructed <see cref="IComponentRenderMode"/> instances that may be used during rendering.
/// </summary>
public static class RenderMode
{
    /// <summary>
    /// Gets an <see cref="IComponentRenderMode"/> that represents rendering interactively on the server via Blazor Server hosting. (without server-side prerendering.)
    /// </summary>
    public static readonly InteractiveServerRenderMode InteractiveServerNoPreRender = new(prerender: false);
}
#endif
