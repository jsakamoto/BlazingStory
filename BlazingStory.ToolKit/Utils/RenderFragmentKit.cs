using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.ToolKit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// The utility class for operations involved with <see cref="RenderFragment"/>.
/// </summary>
public static class RenderFragmentKit
{
    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment" /> renders.
    /// </summary>
    /// <param name="fragment">RenderFragment to get the string content.</param>
    /// <returns>The string content what is the given <see cref="RenderFragment" /> renders.</returns>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public static async ValueTask<string> ToMarkupStringAsync(this RenderFragment fragment)
    {
        await using var services = new ServiceCollection().BuildServiceProvider();
        await using var htmlRenderer = new HtmlRenderer(services, NullLoggerFactory.Instance);

        var result = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await htmlRenderer.RenderComponentAsync<ContainerComponent<object>>(
                    ParameterView.FromDictionary(new Dictionary<string, object?>
                    {{ "Content", fragment }}));

            return output.ToHtmlString();
        });

        return result;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2060")]
    public static async ValueTask<string> TryToMarkupStringAsync(object? value, Func<object?, string>? fallback = null)
    {
        if (value is string str) return str;
        if (value is RenderFragment renderFragment) return await renderFragment.ToMarkupStringAsync();
        if (value?.GetType().IsGenericTypeOf(typeof(RenderFragment<>)) == true)
        {
            var typeOfContext = value.GetType().GetGenericArguments().First();
            var toMarkupStringAsyncMethod = RenderFragmentKit.ToMarkupStringAsyncT.Value.MakeGenericMethod(typeOfContext);
            var result = toMarkupStringAsyncMethod.Invoke(null, [value]);
            if (result is ValueTask<string> taskResult) return await taskResult;
        }

        return fallback?.Invoke(value) ?? value?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Get the string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.
    /// </summary>
    /// <param name="renderFragment">The <see cref="RenderFragment&lt;TValue&gt;" /> to get the string content.</param>
    /// <returns>The string content what is the given <see cref="RenderFragment&lt;TValue&gt;" /> renders.</returns>
    public static async ValueTask<string?> ToMarkupStringAsync<TContext>(this RenderFragment<TContext>? renderFragment)
    {
        var innerRenderFragment = renderFragment?.Invoke(default!);
        if (innerRenderFragment is null) return string.Empty;

        return await innerRenderFragment.ToMarkupStringAsync();
    }

    private static readonly Lazy<MethodInfo> ToMarkupStringAsyncT = new(() => typeof(RenderFragmentKit)
           .GetMethods(BindingFlags.Public | BindingFlags.Static)
           .Where(m => m.Name == nameof(ToMarkupStringAsync))
           .Where(m => m.IsGenericMethod)
           .First());

    /// <summary>
    /// Returns whether the given value is a <see cref="RenderFragment"/> or <see cref="RenderFragment{TValue}"/>.
    /// </summary>
    /// <param name="value">The value or type to check.</param>
    public static bool IsRenderFragment(object? value)
    {
        var type = value is Type t ? t : value?.GetType();
        return type == typeof(RenderFragment) || type?.IsGenericTypeOf(typeof(RenderFragment<>)) == true;
    }

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public static bool TryToString(object? obj, [NotNullWhen(true)] out string? result) => throw new NotImplementedException();

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public static string ToString<TContext>(RenderFragment<TContext>? renderFragment) => throw new NotImplementedException();

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public static RenderFragment FromString(string text) => throw new NotImplementedException();

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public static object FromString<T>(string text) => throw new NotImplementedException();

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public static object FromString(Type argumentType, string text) => throw new NotImplementedException();
}
