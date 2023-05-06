using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Types;

namespace BlazingStory.Internals.Services.Docs;

internal static class StoriesRazorSource
{
    internal static ValueTask<string> GetSourceCodeAsync(Story story)
    {
        var assemblyOfStoriesRazor = story.StoriesRazorDescriptor.TypeOfStoriesRazor.Assembly;
        var projectMetadata = assemblyOfStoriesRazor.GetCustomAttribute<ProjectMetaDataAttribute>();
        if (projectMetadata == null) return ValueTask.FromResult("");

        var relativePathOfRazor = Path.GetRelativePath(projectMetadata.ProjectDir, story.StoriesRazorDescriptor.StoriesAttribute.FilePath);
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(projectMetadata.RootNamespace));

        using var resStream = assemblyOfStoriesRazor.GetManifestResourceStream(resName);
        if (resStream == null) return ValueTask.FromResult("");
        using var reader = new StreamReader(resStream);
        var razorSource = reader.ReadToEnd();

        return ValueTask.FromResult(razorSource);
    }
}
