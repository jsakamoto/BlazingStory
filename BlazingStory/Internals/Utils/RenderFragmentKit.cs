using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// The utility class for operations involved with <see cref="RenderFragment"/>.
/// </summary>
internal static class RenderFragmentKit
{
    /// <summary>
    /// When the given object is <see cref="RenderFragment"/>, returns true and the string content what is the given <see cref="RenderFragment"/> renders.
    /// </summary>
    /// <param name="obj">The object to convert to string if it is <see cref="RenderFragment"/>.</param>
    /// <param name="result">The string content what is the given <see cref="RenderFragment"/> renders.</param>
    /// <returns>True if the given object is <see cref="RenderFragment"/>. Otherwise, false.</returns>
    internal static bool TryToString(object? obj, [NotNullWhen(true)] out string? result)
    {
        if (obj is RenderFragment renderFragment)
        {
            result = ToString(renderFragment);
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

#pragma warning disable BL0006 // Do not use RenderTree types

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment"/> renders.
    /// </summary>
    /// <param name="renderFragment">The <see cref="RenderFragment"/> to get the string content.</param>
    /// <returns>The string content what is the given <see cref="RenderFragment"/> renders.</returns>
    internal static string ToString(RenderFragment? renderFragment)
    {
        var renderTreeBuilder = new RenderTreeBuilder();
        renderFragment?.Invoke(renderTreeBuilder);

        var frames = renderTreeBuilder.GetFrames();
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < frames.Count; i++)
        {
            var frame = frames.Array[i];
            switch (frame.FrameType)
            {
                case RenderTreeFrameType.Text:
                    stringBuilder.Append(frame.TextContent);
                    break;
                case RenderTreeFrameType.Markup:
                    stringBuilder.Append(frame.MarkupContent);
                    break;
                default:
                    break;
            }
        }

        return stringBuilder.ToString();
    }
#pragma warning restore BL0006 // Do not use RenderTree types

}
