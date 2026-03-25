using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.ToolKit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// This class contains extension methods for the RenderFragment class.
/// </summary>
public static class RenderFragmentExtensions
{
    private static readonly Lazy<MethodInfo> ToStringMethodT = new(() => typeof(RenderFragmentExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(ToString))
            .First(m => m.IsGenericMethod));

    /// <summary>
    /// When the given object is <see cref="RenderFragment" /> or <see
    /// cref="RenderFragment&lt;TValue&gt;" />, returns true and the string content what is the
    /// given <see cref="RenderFragment" /> renders.
    /// </summary>
    /// <param name="obj">
    /// The object to convert to string if it is <see cref="RenderFragment" /> or <see
    /// cref="RenderFragment&lt;TValue&gt;" />.
    /// </param>
    /// <param name="result">
    /// The string content what is the given <see cref="RenderFragment" /> or <see
    /// cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </param>
    /// <returns>
    /// True if the given object is <see cref="RenderFragment" /> or <see
    /// cref="RenderFragment&lt;TValue&gt;" />. Otherwise, false.
    /// </returns>
    [UnconditionalSuppressMessage("Trimming", "IL2060")]
    [DynamicDependency(NonPublicMethods, "BlazingStory.Internals.Utils.RenderFragmentKit", "BlazingStory")]
    public static bool TryToString(this object? obj, [NotNullWhen(true)] out string? result)
    {
        if (obj?.GetType().IsGenericTypeOf(typeof(RenderFragment<>)) == true)
        {
            var typeOfContext = obj.GetType().GetGenericArguments().First();
            var toStringMethod = ToStringMethodT.Value.MakeGenericMethod(typeOfContext);

            result = toStringMethod.Invoke(null, new[] { obj }) as string ?? "";

            return true;
        }
        else if (obj is RenderFragment renderFragment)
        {
            result = renderFragment.ToMarkupString();

            return true;
        }
        else
        {
            result = null;

            return false;
        }
    }

    /// <summary>
    /// Check if the given object is <see cref="RenderFragment" /> or <see
    /// cref="RenderFragment&lt;TValue&gt;" />.
    /// </summary>
    /// <param name="value">
    /// </param>
    /// <returns>
    /// </returns>
    public static bool IsRenderFragment(this object? value)
    {
        var type = value is Type t ? t : value?.GetType();

        var result = type == typeof(RenderFragment) || type.IsGenericRenderFragment();

        return result;
    }

    public static bool IsGenericRenderFragment(this object? value)
    {
        var type = value is Type t ? t : value?.GetType();

        var result = type?.IsGenericTypeOf(typeof(RenderFragment<>)) == true;

        return result;
    }

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </summary>
    /// <param name="renderFragment">
    /// The <see cref="RenderFragment&lt;TValue&gt;" /> to get the string content.
    /// </param>
    /// <returns>
    /// The string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </returns>
    public static string ToString<TContext>(RenderFragment<TContext>? renderFragment)
    {
        if (renderFragment == null)
        {
            return string.Empty;
        }

        var innerRenderFragment = renderFragment.Invoke(default!);

        var result = innerRenderFragment.ToMarkupString();

        return result;
    }

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment" /> renders.
    /// </summary>
    /// <param name="fragment">
    /// RenderFragment to get the string content.
    /// </param>
    /// <returns>
    /// The string content what is the given <see cref="RenderFragment" /> renders.
    /// </returns>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public static string ToMarkupString(this RenderFragment fragment)
    {
        var renderer = new RenderedTreeFrames();
        var component = new ContainerComponent<object> { Content = fragment };
        var renderTree = new RenderTreeBuilder();
        component.BuildTree(renderTree);

        var frameRange = renderTree.GetFrames();
        var framesArray = new RenderTreeFrame[frameRange.Count];
        Array.Copy(frameRange.Array, framesArray, frameRange.Count);

        var result = renderer.RenderFrames(framesArray);

        return result;
    }

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </summary>
    /// <param name="fragment">
    /// The <see cref="RenderFragment&lt;TValue&gt;" /> to get the string content.
    /// </param>
    /// <returns>
    /// The string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </returns>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public static string ToMarkupString(this object fragment)
    {
        return fragment switch
        {
            RenderFragment renderFragment => renderFragment.ToMarkupString(),
            string str => str,
            _ => string.Empty
        };
    }

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </summary>
    /// <param name="renderFragment">
    /// The <see cref="RenderFragment&lt;TValue&gt;" /> to get the string content.
    /// </param>
    /// <returns>
    /// The string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </returns>
    public static string? ToMarkupString<TContext>(this RenderFragment<TContext>? renderFragment)
    {
        if (renderFragment == null)
        {
            return string.Empty;
        }

        var innerRenderFragment = renderFragment.Invoke(default!);

        var result = innerRenderFragment?.ToMarkupString();

        return result;
    }
}
