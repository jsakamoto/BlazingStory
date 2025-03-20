using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Types;

namespace BlazingStory.Internals.Services;

/// <summary>
/// This class detects types of custom Razor component and its <see cref="CustomPageAttribute"/> from assemblies.
/// </summary>
internal static class CustomPageRazorDetector
{
    /// <summary>
    /// Gets a type of Stories custom component and its <see cref="CustomPageAttribute"/> from assemblies.
    /// </summary>
    /// <param name="assemblies">Assemblies to detect types of custom Razor component.</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    internal static IEnumerable<CustomPageRazorDescriptor> Detect(IEnumerable<Assembly>? assemblies)
    {
        return (assemblies ?? [])
            .SelectMany(assembly => Extract(assembly.GetTypes()))
#pragma warning disable IL2077
            .Select(t => new CustomPageRazorDescriptor(t.Type, t.Attribute))
#pragma warning restore IL2077
            .ToArray();
    }

    /// <summary>
    /// Detects types of custom page Razor component and its <see cref="CustomPageAttribute"/> from assemblies and register them to the provided store.
    /// </summary>
    /// <param name="assemblies"></param>
    /// <param name="store"></param>
    public static void DetectAndRegisterToStore(IEnumerable<Assembly>? assemblies, CustomPageStore store)
    {
        var descriptors = Detect(assemblies);
        foreach (var descriptor in descriptors)
        {
            var container = new CustomPageContainer(descriptor);
            store.RegisterCustomPageContainer(container);
        }
    }

    private static IEnumerable<(Type Type, CustomPageAttribute Attribute)> Extract(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            var customPageAttribute = type.GetCustomAttribute<CustomPageAttribute>();
            if (customPageAttribute == null) continue;
            yield return (type, customPageAttribute);
        }
    }
}
