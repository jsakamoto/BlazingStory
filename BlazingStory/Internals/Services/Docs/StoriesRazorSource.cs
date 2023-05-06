using System.Reflection;
using System.Text.RegularExpressions;
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
        var sourceOfRazor = reader.ReadToEnd();
        resStream.Dispose();

        var sourceOfStory = GetSourceOfStory(sourceOfRazor, story.Name);

        return ValueTask.FromResult(sourceOfStory);
    }

    private static string GetSourceOfStory(string sourceOfRazor, string storyName)
    {
        const string closeStory = "</Story>";
        const string openTemplate = "<Template>";
        const string closeTemplate = "</Template>";
        foreach (Match openStoryMatch in Regex.Matches(sourceOfRazor, "<Story[ \t]+(?<attr>[^>]+)>"))
        {
            var nameAttr = openStoryMatch.Groups["attr"].Value.Split(' ').FirstOrDefault(a => a.StartsWith("Name="));
            if (nameAttr == null) continue;
            var name = nameAttr.Split('=')[1].Trim('"');
            if (name != storyName) continue;

            var closeStoryIndex = sourceOfRazor.IndexOf(closeStory, openStoryMatch.Index + openStoryMatch.Length);
            if (closeStoryIndex == -1) continue;

            var openTemplateIndex = sourceOfRazor.IndexOf(openTemplate, openStoryMatch.Index + openStoryMatch.Length, closeStoryIndex - (openStoryMatch.Index + openStoryMatch.Length));
            if (openTemplateIndex == -1) continue;

            var closeTemplateIndex = sourceOfRazor.IndexOf(closeTemplate, openTemplateIndex + openTemplate.Length, closeStoryIndex - (openTemplateIndex + openTemplate.Length));
            if (closeTemplateIndex == -1) continue;

            var templateContents = sourceOfRazor.Substring(openTemplateIndex + openTemplate.Length, closeTemplateIndex - (openTemplateIndex + openTemplate.Length));
            return templateContents.Trim();
        }
        return "";
    }
}
