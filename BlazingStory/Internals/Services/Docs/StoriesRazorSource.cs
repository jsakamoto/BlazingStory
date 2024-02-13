using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Encodings.Web;
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

    internal static string UpdateSourceTextWithArgument(Story story, string codeText)
    {
        var componentTypeNameFragments = story.ComponentType.FullName?.Split('.') ?? Array.Empty<string>();
        var componentTagCandidates = componentTypeNameFragments
            .Reverse()
            .Aggregate(seed: new List<string>(), (list, fragment) =>
            {
                list.Add(list.Any() ? fragment + "\\." + list.Last() : fragment); return list;
            });
        var componentTagPattern = $"({string.Join('|', componentTagCandidates)})";

        return UpdateSourceTextWithArgument(story, codeText, componentTagPattern, story.Context.Args);
    }

    private static string UpdateSourceTextWithArgument(Story story, string codeText, string componentTagPattern, IReadOnlyDictionary<string, object?> args)
    {
        // "        <ButtonComponent       Text="Hello" @attributes=\"context.Args\" />"
        //  ~~~~~~~~ ~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //  ↑ indent  ↑ tagName                    ↑ attrs

        var openTag = Regex.Match(codeText, $"(?<indent>[ \\t]*)<(?<tagName>{componentTagPattern})((?<attrs>\\s+[^>]*))?>");
        if (!openTag.Success) return codeText;
        var attrs = openTag.Groups["attrs"];
        if (!attrs.Success) return codeText;
        var closeTag = attrs.Value.EndsWith('/') ? null : Regex.Match(codeText, $"</{openTag.Groups["tagName"].Value}>");

        var argsAttr = Regex.Match(attrs.Value, "@attributes=(\"\\w+\\.Args\"|'\\w+\\.Args')");
        if (!argsAttr.Success) return codeText;

        var parameters = Regex.Matches(attrs.Value, "(?<gap>\\s+)(?<name>[@\\w]+)=(?<quote>\"|')");
        for (var i = 0; i < parameters.Count; i++)
        {
            if (!TryUpdateCodeText(story, codeText, args, openTag, attrs, parameters, i, out var updatedCodeText)) continue;

            var paramName = parameters[i].Groups["name"].Value;
            return UpdateSourceTextWithArgument(story, updatedCodeText, componentTagPattern, args.Where(item => item.Key != paramName).ToDictionary(item => item.Key, item => item.Value));
        }

        return codeText;
    }

    private static bool TryUpdateCodeText(Story story, string codeText, IReadOnlyDictionary<string, object?> args, Match openTag, Group attrs, MatchCollection parameters, int i, [NotNullWhen(true)] out string? updatedCodeText)
    {
        var parameter = parameters[i];
        var paramName = parameter.Groups["name"];
        var endIndex = (i + 1 < parameters.Count) ? parameters[i + 1].Index : attrs.Value.LastIndexOf(parameter.Groups["quote"].Value) + 1;

        if (paramName.Value == "@attributes")
        {
            var firstParam = parameters[0];
            var indent = openTag.Groups["indent"];
            var attrIndentSrc = codeText.Substring(indent.Index, attrs.Index + firstParam.Groups["name"].Index - indent.Index);
            var attrIndent = Regex.Replace(attrIndentSrc, "[^\\s]", " ");

            var argString = ArgumentsToAttributeStrings(story, args)
                .Select((attr, index) => (attr, index))
                .Select(x => (x.index == 0 && i == 0) ? x.attr : attrIndent + x.attr)
                .ToArray();

            updatedCodeText = string.Concat(
                codeText.Substring(0, attrs.Index + parameter.Index),
                argString.Any() ? (i == 0 ? parameter.Groups["gap"].Value : "\n") : "",
                string.Join('\n', argString),
                codeText.Substring(attrs.Index + endIndex));

            return true;
        }

        else
        {
            if (!args.TryGetValue(paramName.Value, out var value)) { updatedCodeText = null; return false; }

            var argString = ConvertArgToString(paramName.Value, value, parameter.Groups["quote"].Value);
            updatedCodeText = string.Concat(
                  codeText.Substring(0, attrs.Index + paramName.Index),
                  argString,
                  codeText.Substring(attrs.Index + endIndex));

            return true;
        }
    }

    private static IEnumerable<string> ArgumentsToAttributeStrings(Story story, IReadOnlyDictionary<string, object?> args)
    {
        foreach (var param in story.Context.Parameters)
        {
            if (!args.TryGetValue(param.Name, out var value)) continue;
            yield return ConvertArgToString(param.Name, value);
        }
    }

    private static string ConvertArgToString(string name, object? value, string quote = "\"")
    {
        var valueString = value switch
        {
            bool b => b ? "true" : "false",
            Enum e => e.GetType().Name + "." + e,
            _ => value?.ToString() ?? "null"
        };

        return $"{name}={quote}{HtmlEncoder.Default.Encode(valueString)}{quote}";
    }
}
