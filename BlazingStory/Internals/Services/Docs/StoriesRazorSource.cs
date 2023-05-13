using System.Reflection;
using System.Text.RegularExpressions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Types;

namespace BlazingStory.Internals.Services.Docs;

/// <summary>
/// Provides the source code of the stories from the razor files.
/// </summary>
internal static class StoriesRazorSource
{
    /// <summary>
    /// Gets the source code of the given story.
    /// </summary>
    internal static ValueTask<string> GetSourceCodeAsync(Story story)
    {
        var assemblyOfStoriesRazor = story.StoriesRazorDescriptor.TypeOfStoriesRazor.Assembly;
        var projectMetadata = assemblyOfStoriesRazor.GetCustomAttribute<ProjectMetaDataAttribute>();
        if (projectMetadata == null) return ValueTask.FromResult("");

        var relativePathOfRazor = story.StoriesRazorDescriptor.StoriesAttribute.FilePath.Substring(projectMetadata.ProjectDir.Length).TrimStart('/', '\\');
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(projectMetadata.RootNamespace));

        using var resStream = assemblyOfStoriesRazor.GetManifestResourceStream(resName);
        if (resStream == null) return ValueTask.FromResult("");
        using var reader = new StreamReader(resStream);
        var sourceOfRazor = reader.ReadToEnd();
        resStream.Dispose();

        var sourceOfStory = GetSourceOfStory(sourceOfRazor, story.Name);

        return ValueTask.FromResult(sourceOfStory);
    }

    /// <summary>
    /// Gets the source code of the given story from the given source code of the razor file.
    /// </summary>
    private static string GetSourceOfStory(string sourceOfRazor, string storyName)
    {
        const string closeStory = "</Story>";
        const string openTemplate = "<Template>";
        const string closeTemplate = "</Template>";
        foreach (Match openStoryMatch in Regex.Matches(sourceOfRazor, "<Story[ \t]+[^>]+>"))
        {
            var nameAttrMatch = Regex.Match(openStoryMatch.Value, @"[ \t]+Name=((""(?<n1>[^""]+)"")|('(?<n2>[^']+)')|((?<n3>[^ \t]+)))");
            if (!nameAttrMatch.Success) continue;
            var name =
                nameAttrMatch.Groups["n1"].Success ? nameAttrMatch.Groups["n1"].Value :
                nameAttrMatch.Groups["n2"].Success ? nameAttrMatch.Groups["n2"].Value :
                nameAttrMatch.Groups["n3"].Value;
            if (name != storyName) continue;

            var closeStoryIndex = sourceOfRazor.IndexOf(closeStory, openStoryMatch.Index + openStoryMatch.Length);
            if (closeStoryIndex == -1) continue;

            var openTemplateIndex = sourceOfRazor.IndexOf(openTemplate, openStoryMatch.Index + openStoryMatch.Length, closeStoryIndex - (openStoryMatch.Index + openStoryMatch.Length));
            if (openTemplateIndex == -1) continue;

            var closeTemplateIndex = sourceOfRazor.IndexOf(closeTemplate, openTemplateIndex + openTemplate.Length, closeStoryIndex - (openTemplateIndex + openTemplate.Length));
            if (closeTemplateIndex == -1) continue;

            var templateContents = sourceOfRazor.Substring(openTemplateIndex + openTemplate.Length, closeTemplateIndex - (openTemplateIndex + openTemplate.Length));
            return Deindent(templateContents);
        }
        return "";
    }

    /// <summary>
    /// Removes the minimum indent from all lines of the given text.
    /// </summary>
    private static string Deindent(string text)
    {
        // Split the text into lines with triming
        var lines = text.Split('\n').Select(s => s.TrimEnd('\r')).ToList();

        // Remove empty lines at the beginning and the end
        while (lines.Any() && Regex.IsMatch(lines.First(), @"^[ \t]*$")) lines.RemoveAt(0);
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(lines[i], @"^[ \t]*$")) lines.RemoveAt(i);
            else break;
        }

        // Find the minimum indent of all lines
        var indentToTrim = lines
            .Where(line => !Regex.IsMatch(line, @"^[ \t]*$"))
            .Aggregate(int.MaxValue, (indent, line) => Math.Min(indent, Regex.Match(line, @"^[ \t]*").Length));

        // Remove the minimum indent from all lines
        return string.Join('\n', lines.Select(line => line.Substring(Math.Min(line.Length, indentToTrim))));
    }
}
