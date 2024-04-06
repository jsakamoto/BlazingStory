using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BlazingStory.Internals.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// The utility class for operations involved with <see cref="RenderFragment"/>.
/// </summary>
internal static class RenderFragmentKit
{
    /// <summary>
    /// When the given object is <see cref="RenderFragment"/> or <see cref="RenderFragment&lt;TValue&gt;"/>, returns true and the string content what is the given <see cref="RenderFragment"/> renders.
    /// </summary>
    /// <param name="obj">The object to convert to string if it is <see cref="RenderFragment"/> or <see cref="RenderFragment&lt;TValue&gt;"/>.</param>
    /// <param name="result">The string content what is the given <see cref="RenderFragment"/> or <see cref="RenderFragment&lt;TValue&gt;"/> renders.</param>
    /// <returns>True if the given object is <see cref="RenderFragment"/> or <see cref="RenderFragment&lt;TValue&gt;"/>. Otherwise, false.</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2060")]
    [DynamicDependency(NonPublicMethods, "BlazingStory.Internals.Utils.RenderFragmentKit", "BlazingStory")]
    internal static bool TryToString(object? obj, [NotNullWhen(true)] out string? result)
    {
        if (obj is RenderFragment renderFragment)
        {
            result = ToString(renderFragment);
            return true;
        }
        else if (obj?.GetType().IsGenericTypeOf(typeof(RenderFragment<>)) == true)
        {
            var typeOfContext = obj.GetType().GetGenericArguments().First();
            var toStringMethod = ToStringMethodT.Value.MakeGenericMethod(typeOfContext);

            result = toStringMethod.Invoke(null, new[] { obj }) as string ?? "";
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    private static readonly Lazy<MethodInfo> ToStringMethodT = new(() => typeof(RenderFragmentKit)
           .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
           .Where(m => m.Name == nameof(ToString))
           .Where(m => m.IsGenericMethod)
           .First());

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment&lt;TValue&gt;"/> renders.
    /// </summary>
    /// <param name="renderFragment">The <see cref="RenderFragment&lt;TValue&gt;"/> to get the string content.</param>
    /// <returns>The string content what is the given <see cref="RenderFragment&lt;TValue&gt;"/> renders.</returns>
    internal static string ToString<TContext>(RenderFragment<TContext>? renderFragment)
    {
        if (renderFragment == null) return string.Empty;
        var innerRenderFragment = renderFragment.Invoke(default!);
        return ToString(innerRenderFragment);
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

    private static readonly Lazy<MethodInfo> AddContentMethod = new(() => typeof(RenderTreeBuilder).GetMethod(nameof(RenderTreeBuilder.AddContent), new[] { typeof(int), typeof(string) }) ?? throw new InvalidOperationException());

    private static readonly Lazy<ParameterExpression> BuilderParam = new(() => Expression.Parameter(typeof(RenderTreeBuilder), "builder"));

    /// <summary>
    /// Convert the given string to <see cref="RenderFragment"/>.
    /// </summary>
    /// <param name="text">The string content what is the <see cref="RenderFragment&lt;TValue&gt;"/> will render.</param>
    /// <returns>The <see cref="RenderFragment"/> that will render the given string content.</returns>
    internal static RenderFragment FromString(string text)
    {
        return (builder) => builder.AddContent(0, text);
    }

    /// <summary>
    /// Convert the given string to <see cref="RenderFragment&lt;TValue&gt;"/>.
    /// </summary>
    /// <typeparam name="T">The type argument of <see cref="RenderFragment&lt;TValue&gt;"/>.</typeparam>
    /// <param name="text">The string content what is the <see cref="RenderFragment&lt;TValue&gt;"/> will render.</param>
    /// <returns>The <see cref="RenderFragment&lt;TValue&gt;"/> that will render the given string content.</returns>
    internal static object FromString<T>(string text)
    {
        return FromString(typeof(T), text);
    }

    /// <summary>
    /// Convert the given string to <see cref="RenderFragment&lt;TValue&gt;"/>.
    /// </summary>
    /// <param name="argumentType">The type argument of <see cref="RenderFragment&lt;TValue&gt;"/>.</param>
    /// <param name="text">The string content what is the <see cref="RenderFragment&lt;TValue&gt;"/> will render.</param>
    /// <returns>The <see cref="RenderFragment&lt;TValue&gt;"/> that will render the given string content.</returns>
    internal static object FromString(Type argumentType, string text)
    {
        var addContentCall = Expression.Call(BuilderParam.Value, AddContentMethod.Value, Expression.Constant(0), Expression.Constant(text));
        var renderFragment = Expression.Lambda(typeof(RenderFragment), addContentCall, BuilderParam.Value);

        var argParam = Expression.Parameter(argumentType, "arg");
        var renderFragmentTDelegateType = typeof(RenderFragment<>).MakeGenericType(argumentType);
        var renderFragmentT = Expression.Lambda(renderFragmentTDelegateType, renderFragment, argParam);

        return renderFragmentT.Compile();
    }

    internal static bool IsRenderFragment(object? value)
    {
        var type = value is Type t ? t : value?.GetType();
        return type == typeof(RenderFragment) || type?.IsGenericTypeOf(typeof(RenderFragment<>)) == true;
    }
}
