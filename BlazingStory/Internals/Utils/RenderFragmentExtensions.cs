using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazingStory.Internals.Utils;

public static class RenderFragmentExtensions
{
    #region Public Methods

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public static string ToMarkupString(this RenderFragment fragment)
    {
        var renderer = new TestHtmlRenderer();
        var component = new ContainerComponent { Content = fragment };
        var renderTree = new RenderTreeBuilder();
        component.BuildTree(renderTree);

        var frameRange = renderTree.GetFrames();
        var framesArray = new RenderTreeFrame[frameRange.Count];
        Array.Copy(frameRange.Array, framesArray, frameRange.Count);

        var result = renderer.RenderFrames(framesArray);
        return result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public static string ToMarkupString(this object fragment)
    {
        var renderer = new TestHtmlRenderer();
        var component = new ContainerComponent { Content = (RenderFragment)fragment };
        var renderTree = new RenderTreeBuilder();
        component.BuildTree(renderTree);

        var frameRange = renderTree.GetFrames();
        var framesArray = new RenderTreeFrame[frameRange.Count];
        Array.Copy(frameRange.Array, framesArray, frameRange.Count);

        var result = renderer.RenderFrames(framesArray);
        return result;
    }

    public static RenderFragment ToRenderFragment(this string markup)
    {
        return builder =>
        {
            builder.AddMarkupContent(0, markup);
        };
    }

    #endregion Public Methods
}

public class ContainerComponent : ComponentBase
{
    #region Public Properties

    public RenderFragment? Content { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void BuildTree(RenderTreeBuilder builder)
    {
        BuildRenderTree(builder);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Content != null)
        {
            builder.AddContent(0, Content);
        }
    }

    #endregion Protected Methods
}

public class TestHtmlRenderer
{
    #region Public Methods

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public string RenderFrames(RenderTreeFrame[] frames)
    {
        using (var writer = new StringWriter())
        {
            var encoder = HtmlEncoder.Default;

            foreach (var frame in frames)
            {
                switch (frame.FrameType)
                {
                    case RenderTreeFrameType.Text:
                        encoder.Encode(writer, frame.TextContent);
                        break;

                    case RenderTreeFrameType.Element:
                        writer.Write($"<{frame.ElementName}>");
                        break;

                    case RenderTreeFrameType.Attribute:
                        writer.Write($" {frame.AttributeName}=\"{frame.AttributeValue}\"");
                        break;

                    case RenderTreeFrameType.Region:
                        // Region frames usually indicate a group of other frames. You might not
                        // need to do anything special here.
                        break;

                    case RenderTreeFrameType.Markup:
                        // Markup frames contain pre-rendered HTML. Encode to make sure it is safe
                        // to insert.
                        encoder.Encode(writer, frame.MarkupContent);
                        break;

                    case RenderTreeFrameType.None:
                        // Log or ignore
                        break;

                    case RenderTreeFrameType.Component:
                        // Components render themselves, might not need to do anything here. If you
                        // really need to render child components, you'll need to dig deeper.
                        break;

                    case RenderTreeFrameType.ElementReferenceCapture:
                        // Capturing a reference to a DOM element, probably not applicable for
                        // string representation.
                        break;

                    case RenderTreeFrameType.ComponentReferenceCapture:
                        // Capturing a reference to a Component, probably not applicable for string representation.
                        break;

                    default:
                        // You might want to log or handle other frame types.
                        break;
                }
            }

            return writer.ToString();
        }
    }

    #endregion Public Methods
}
