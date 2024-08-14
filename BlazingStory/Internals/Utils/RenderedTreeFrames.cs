using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// This class is used to render a RenderTreeFrame[] to a markup string.
/// </summary>
internal class RenderedTreeFrames
{
    private string? extra = null;

    /// <summary>
    /// Method to render an array of RenderTreeFrames to a markup string
    /// </summary>
    /// <param name="frames">
    /// The frames.
    /// </param>
    /// <returns>
    /// Parsed markup string.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    internal string RenderFrames(RenderTreeFrame[] frames)
    {
        using (var writer = new StringWriter())
        {
            var encoder = HtmlEncoder.Default;

            for (var i = 0; i < frames.Length; i++)
            {
                var frame = frames[i];

                switch (frame.FrameType)
                {
                    case RenderTreeFrameType.Element:
                        this.RenderElementFrame(writer, frames, ref i, frame);
                        break;

                    case RenderTreeFrameType.Component:
                        this.RenderComponentFrame(writer, frames, ref i, frame);
                        break;

                    case RenderTreeFrameType.Attribute:
                        this.RenderAttributeFrame(writer, frame);
                        break;

                    case RenderTreeFrameType.Text:
                        encoder.Encode(writer, frame.TextContent);
                        break;

                    case RenderTreeFrameType.Markup:
                        RenderMarkupFrame(writer, encoder, frame);

                        break;

                    default:
                        // Handle other frame types if needed
                        break;
                }
            }

            var result = writer.ToString();

            return result;
        }
    }

    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    private static void RenderMarkupFrame(StringWriter writer, HtmlEncoder encoder, RenderTreeFrame frame)
    {
        // Determine if it's an HTML tag or a Razor component
        var markupContent = frame.MarkupContent;
        var tagName = markupContent.GetTagName();

        if (!string.IsNullOrWhiteSpace(tagName))
        {
            if (tagName.IsHtmlTag())
            {
                writer.Write(markupContent);
            }
            else
            {
                var componentType = tagName.FindComponentType();

                if (componentType != null)
                {
                    writer.Write(markupContent);
                }
                else
                {
                    var markupString = new MarkupString(markupContent);

                    encoder.Encode(writer, markupString.Value);
                }
            }
        }
        else
        {
            var markupString = new MarkupString(markupContent);

            encoder.Encode(writer, markupString.Value);
        }
    }

    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    private void RenderElementFrame(StringWriter writer, RenderTreeFrame[] frames, ref int i, RenderTreeFrame frame)
    {
        writer.Write($"<{frame.ElementName}");

        var isSelfClosing = frame.ElementSubtreeLength == 0;
        var hasAttributes = i + 1 < frames.Length && frames[i + 1].FrameType.Equals(RenderTreeFrameType.Attribute);
        var attributes = new Dictionary<string, object?>();

        if (hasAttributes)
        {
            for (var j = i + 1; j < frames.Length; j++)
            {
                if (frames[j].FrameType != RenderTreeFrameType.Attribute)
                {
                    break;
                }

                var attributeFrame = frames[j];
                var attributeName = attributeFrame.AttributeName;
                var attributeValue = attributeFrame.AttributeValue;

                attributes.Add(attributeName, attributeValue);
            }

            foreach (var attribute in attributes)
            {
                writer.Write($" {attribute.Key}=\"{attribute.Value}\"");
            }

            i += attributes.Count;
        }

        if (isSelfClosing)
        {
            writer.Write(" />");
            return;
        }

        writer.Write(">");

        var elementSubtreeLength = frame.ElementSubtreeLength - attributes.Count;

        if (elementSubtreeLength > 0)
        {
            var childFrames = new RenderTreeFrame[elementSubtreeLength];
            Array.Copy(frames, i + 1, childFrames, 0, elementSubtreeLength - 1);

            var childRender = this.RenderFrames(childFrames);

            writer.Write(childRender);
            i += (elementSubtreeLength - 1);
        }

        writer.Write($"</{frame.ElementName}>");
    }

    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    private void RenderComponentFrame(StringWriter writer, RenderTreeFrame[] frames, ref int i, RenderTreeFrame frame)
    {
        var toBeAdded = string.Empty;

        var componentName = frame.ComponentType.Name;

        if (componentName.Equals("DataAnnotationsValidator"))
        {
            writer.Write($"<{componentName} />");
        }
        else
        {
            writer.Write($"<{componentName}");

            var isSelfClosing = frame.ComponentSubtreeLength == 0;
            var hasAttributes = i + 1 < frames.Length && frames[i + 1].FrameType.Equals(RenderTreeFrameType.Attribute);
            var attributes = new Dictionary<string, object?>();

            if (hasAttributes)
            {
                for (var j = i + 1; j < frames.Length; j++)
                {
                    if (frames[j].FrameType != RenderTreeFrameType.Attribute)
                    {
                        break;
                    }

                    var attributeFrame = frames[j];
                    var attributeName = attributeFrame.AttributeName;
                    var attributeValue = attributeFrame.AttributeValue;

                    attributes.Add(attributeName, attributeValue);

                    RenderTreeFrame? previousFrame;

                    if (j > 0)
                    {
                        previousFrame = frames[j - 1];
                    }
                    else
                    {
                        previousFrame = null;
                    }

                    this.RenderAttributeFrame(writer, attributeFrame, previousFrame);
                }

                i += attributes.Count;
            }

            if (isSelfClosing)
            {
                writer.Write(" />");
                return;
            }

            writer.Write(">");

            var componentSubtreeLength = frame.ComponentSubtreeLength - attributes.Count;

            if (componentSubtreeLength > 0)
            {
                var childFrames = new RenderTreeFrame[componentSubtreeLength];
                Array.Copy(frames, i + 1, childFrames, 0, componentSubtreeLength - 1);

                var childRender = this.RenderFrames(childFrames);

                writer.Write(childRender);
                i += (componentSubtreeLength - 1);
            }

            if (!string.IsNullOrWhiteSpace(toBeAdded))
            {
                var extraElements = toBeAdded;

                writer.Write(extraElements);
            }

            if (!string.IsNullOrWhiteSpace(this.extra))
            {
                var extraElements = this.extra;

                writer.Write(extraElements);

                this.extra = null;
            }

            writer.Write($"</{componentName}>");
        }
    }

    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    private void RenderAttributeFrame(StringWriter writer, RenderTreeFrame frame, RenderTreeFrame? previousFrame = null)
    {
        var attributeName = frame.AttributeName;
        var attributeValue = frame.AttributeValue;

        if (attributeValue.IsRenderFragment())
        {
            if (attributeValue.IsGenericRenderFragment())
            {
                // Assuming attributeValue is of type RenderFragment<T>
                var attributeType = attributeValue.GetType();
                var genericArguments = attributeType.GetGenericArguments();

                if (genericArguments.Length == 1 && genericArguments[0] == typeof(EditContext))
                {
                    // Handle RenderFragment<EditContext>
                    var renderFragment = (RenderFragment<EditContext>)attributeValue;
                    var editContext = new EditContext(previousFrame.HasValue ? previousFrame.Value.AttributeValue : new object());

                    // Render the fragment into a MarkupString
                    var innerRenderFragment = renderFragment.Invoke(editContext);
                    var markupString = innerRenderFragment.ToMarkupString();

                    this.extra += $"<{attributeName}>{markupString}</{attributeName}>";
                }
                else
                {
                    // Handle other types of RenderFragment<T> if needed For example: var
                    // renderFragment = (RenderFragment<T>)attributeValue;
                    // renderFragment.Invoke(someInstanceOfTypeT, markupBuilder); var markupString =
                    // new MarkupString(markupBuilder.ToString());
                }
            }
            else
            {
                // Handle non-generic RenderFragment (e.g., RenderFragment without type argument)
                var markupString = attributeValue.ToMarkupString();
                this.extra += $"<{attributeName}>{markupString}</{attributeName}>";
            }
        }
        else
        {
            // AttributeValue is not a RenderFragment, write it as a regular attribute
            writer.Write($" {attributeName}=\"{attributeValue}\"");
        }
    }
}
