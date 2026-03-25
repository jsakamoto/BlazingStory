using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.CompilerServices;

namespace BlazingStory.Internals.Components.Basics;

/// <summary>
/// Lightweight generic HTML element wrapper component.
/// Renders the specified <see cref="TagName"/> (defaults to div) with optional content, click handler and extra attributes.
/// </summary>
public class HtmlElement : ComponentBase
{
    /// <summary>
    /// Tag name to render. If null or empty, a <c>div</c> is used.
    /// </summary>
    [Parameter]
    public string? TagName { get; set; }

    /// <summary>
    /// Click event callback (mapped to the <c>onclick</c> DOM event).
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Child content to render inside the element.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional arbitrary attributes applied to the root element.
    /// Unmatched attributes flow here via <see cref="ParameterAttribute.CaptureUnmatchedValues"/>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Builds the render tree for the element.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, string.IsNullOrEmpty(this.TagName) ? "div" : this.TagName);
        builder.AddAttribute(1, "onclick", this.OnClick);

        if (this.AdditionalAttributes != null)
        {
            builder.AddMultipleAttributes(2, RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<string, object>>>(this.AdditionalAttributes));
        }
        if (this.ChildContent != null)
        {
            builder.AddContent(3, this.ChildContent);
        }
        builder.CloseElement();
    }
}
