using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Types;

namespace BlazingStory.Internals.Services;

internal class StoryContainerTypesDetector
{
    //public IEnumerable<Type> Detect(IEnumerable<Assembly>? assemblies)
    //{
    //    return (assemblies ?? Enumerable.Empty<Assembly>())
    //        .SelectMany(assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes().Any(attr => attr is StoriesAttribute)))
    //        .ToArray();
    //}

    public IEnumerable<StoryContainer> DetectContainers(IEnumerable<Assembly>? assemblies)
    {
        return (assemblies ?? Enumerable.Empty<Assembly>())
            .SelectMany(assembly => Extract(assembly.GetTypes()))
            .Select(t => new StoryContainer(t.Type, t.Attribute.Title))
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
