using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Types;

namespace BlazingStory.Internals.Services;

/// <summary>
/// This class detects types of Stories Razor component (..stories.razor) and its <see cref="StoriesAttribute"/> from assemblies.
/// </summary>
internal static class StoriesRazorDetector
{
    /// <summary>
    /// Gets a type of Stories Razor component (..stories.razor) and its <see cref="StoriesAttribute"/> from assemblies.
    /// </summary>
    /// <param name="assemblies">Assemblies to detect types of Stories Razor component (..stories.razor).</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    internal static IEnumerable<StoriesRazorDescriptor> Detect(IEnumerable<Assembly>? assemblies)
    {
        return (assemblies ?? Enumerable.Empty<Assembly>())
            .SelectMany(assembly => Extract(assembly.GetTypes()))
            .Select(t => new StoriesRazorDescriptor(t.Type, t.Attribute))
            .ToArray();
    }

    private static IEnumerable<(Type Type, StoriesAttribute Attribute)> Extract(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            var storiesAttribute = type.GetCustomAttribute<StoriesAttribute>();
            if (storiesAttribute == null) continue;
            yield return (type, storiesAttribute);
        }
    }
}
