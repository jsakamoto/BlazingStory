using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Types;
using BlazingStory.Internals.Utils;

namespace BlazingStory.Internals.Services.Docs;

using Arguments = IReadOnlyDictionary<string, object?>;

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

    internal class UpdateSourceContext
    {
        public readonly IEnumerable<ComponentParameter> Parameters;
        public readonly string ComponentTagPattern;
        public UpdateSourceContext(IEnumerable<ComponentParameter> parameters, string componentTagPattern)
        {
            this.Parameters = parameters;
            this.ComponentTagPattern = componentTagPattern;
        }

        public bool IsRenderFragmentParam(string paramName)
        {
            return this.Parameters.TryGetByName(paramName, out var paramInfo) && RenderFragmentKit.IsRenderFragment(paramInfo.Type);
        }
    }

    /// <summary>
    /// Updates the source code of the given story with the arguments of the given story.
    /// </summary>
    /// <param name="story">The story to update the source code.</param>
    /// <param name="codeText">The source code of the story to update.</param>
    /// <returns></returns>
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

        var context = new UpdateSourceContext(story.Context.Parameters, componentTagPattern);
        return UpdateSourceTextWithArgument(context, codeText, story.Context.Args, hasAtAttributeOnce: false);
    }

    internal class TagMatch
    {
        public readonly Match OpenTag;
        public readonly Group Attrs;
        public readonly Match? CloseTag;
        public readonly Match ArgsAttr;
        public readonly MatchCollection Parameters;

        public TagMatch(Match openTag, Group attrs, Match? closeTag, Match argsAttr, MatchCollection parameters)
        {
            this.OpenTag = openTag;
            this.Attrs = attrs;
            this.CloseTag = closeTag;
            this.ArgsAttr = argsAttr;
            this.Parameters = parameters;
        }
    }

    private static bool TryTagMatch(UpdateSourceContext context, string codeText, bool hasAtAttributeOnce, [NotNullWhen(true)] out TagMatch? tagMatch)
    {
        tagMatch = null;
        var openTag = Regex.Match(codeText, $"(?<indent>[ \\t]*)<(?<tagName>{context.ComponentTagPattern})((?<attrs>\\s+[^>]*))?>");
        if (!openTag.Success) return false;
        var attrs = openTag.Groups["attrs"];
        var closeTag = attrs.Value.EndsWith('/') ? null : Regex.Match(codeText, $"</{openTag.Groups["tagName"].Value}>");

        var argsAttr = Regex.Match(attrs.Value, "@attributes=(\"\\w+\\.Args\"|'\\w+\\.Args')");
        if (!argsAttr.Success && !hasAtAttributeOnce) return false;
        var parameters = Regex.Matches(attrs.Value, "(?<gap>\\s+)(?<name>[@\\w]+)=(?<quote>\"|')");

        tagMatch = new TagMatch(openTag, attrs, closeTag, argsAttr, parameters);
        return true;
    }

    private static string UpdateSourceTextWithArgument(UpdateSourceContext context, string codeText, Arguments args, bool hasAtAttributeOnce = true)
    {
        // "        <ButtonComponent       Text="Hello" @attributes=\"context.Args\" />"
        //  ~~~~~~~~ ~~~~~~~~~~~~~~~ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //  ↑ indent  ↑ tagName                    ↑ attrs
        if (!TryTagMatch(context, codeText, hasAtAttributeOnce, out var tagMatch)) return codeText;

        // Update parameters
        if (tagMatch.ArgsAttr.Success)
        {
            for (var i = 0; i < tagMatch.Parameters.Count; i++)
            {
                if (!TryUpdateCodeText(context, codeText, tagMatch, args, i, out var updatedCodeText, out var nextArgs)) continue;
                return UpdateSourceTextWithArgument(context, updatedCodeText, nextArgs);
            }
        }

        // Update render fragments
        return UpdateRenderFragments(context, codeText, tagMatch, args);
    }

    private static bool TryUpdateCodeText(UpdateSourceContext context, string codeText, TagMatch tagMatch, Arguments args, int i, [NotNullWhen(true)] out string? updatedCodeText, [NotNullWhen(true)] out Arguments? nextArgs)
    {
        var parameter = tagMatch.Parameters[i];
        var paramName = parameter.Groups["name"];
        var endIndex = (i + 1 < tagMatch.Parameters.Count) ? tagMatch.Parameters[i + 1].Index : tagMatch.Attrs.Value.LastIndexOf(parameter.Groups["quote"].Value) + 1;

        if (paramName.Value == "@attributes")
        {
            var firstParam = tagMatch.Parameters[0];
            var indent = tagMatch.OpenTag.Groups["indent"];
            var attrIndentSrc = codeText.Substring(indent.Index, tagMatch.Attrs.Index + firstParam.Groups["name"].Index - indent.Index);
            var attrIndent = Regex.Replace(attrIndentSrc, "[^\\s]", " ");

            var trailingParamNames = tagMatch.Parameters.Skip(i + 1).Select(p => p.Groups["name"].Value).ToArray();
            var availableArgs = args.Exclude(trailingParamNames);
            var argString = ArgumentsToAttributeStrings(context, availableArgs)
                .Select((attr, index) => (attr, index))
                .Select(x => (x.index == 0 && i == 0) ? x.attr : attrIndent + x.attr)
                .ToArray();

            updatedCodeText = string.Concat(
                codeText.Substring(0, tagMatch.Attrs.Index + parameter.Index),
                argString.Any() ? (i == 0 ? parameter.Groups["gap"].Value : "\n") : "",
                string.Join('\n', argString),
                codeText.Substring(tagMatch.Attrs.Index + endIndex));

            nextArgs = args.Exclude(trailingParamNames);

            return true;
        }

        else
        {
            if (!args.TryGetValue(paramName.Value, out var value)) { updatedCodeText = null; nextArgs = null; return false; }

            if (context.IsRenderFragmentParam(paramName.Value))
            {
                updatedCodeText = string.Concat(
                      codeText.Substring(0, tagMatch.Attrs.Index + parameter.Groups["gap"].Index),
                      codeText.Substring(tagMatch.Attrs.Index + endIndex));
                nextArgs = args;
            }
            else
            {
                var argString = ConvertArgToString(paramName.Value, value, parameter.Groups["quote"].Value);
                updatedCodeText = string.Concat(
                      codeText.Substring(0, tagMatch.Attrs.Index + paramName.Index),
                      argString,
                      codeText.Substring(tagMatch.Attrs.Index + endIndex));
                nextArgs = args.Exclude(paramName.Value);
            }

            return true;
        }
    }

    private static string UpdateRenderFragments(UpdateSourceContext context, string codeText, TagMatch tagMatch, Arguments args)
    {
        var renderFragmentArgs = args.Where(item => context.IsRenderFragmentParam(item.Key)).ToArray();

        var childContent = tagMatch.CloseTag == null ? "" : codeText.Substring(tagMatch.OpenTag.Index + tagMatch.OpenTag.Length, tagMatch.CloseTag.Index - (tagMatch.OpenTag.Index + tagMatch.OpenTag.Length));
        var childContentIsEmpty = childContent == "" ? true : Regex.IsMatch(childContent, "^\\s*$", RegexOptions.Singleline);

        var renderFragmentKeys = childContentIsEmpty ? new() :
            renderFragmentArgs
            .Where(arg => Regex.IsMatch(childContent, $"<{arg.Key}>.*</{arg.Key}>", RegexOptions.Singleline))
            .Select(arg => arg.Key)
            .ToHashSet();

        if (!childContentIsEmpty && !renderFragmentKeys.Any()) return codeText;

        var indent = tagMatch.OpenTag.Groups["indent"].Value;
        var indentChild = indent + "    ";
        var childContentLines = new StringBuilder();
        foreach (var arg in renderFragmentArgs)
        {
            if (renderFragmentKeys.Contains(arg.Key)) continue;

            var text = RenderFragmentKit.TryToString(arg.Value, out var t) ? t : arg.Value?.ToString() ?? "";

            if (arg.Key == "ChildContent" && renderFragmentArgs.Length == 1)
            {
                childContentLines.Append(indentChild + text + "\n");
            }
            else
            {
                childContentLines.Append(indentChild + "<" + arg.Key + ">\n");
                childContentLines.Append(indentChild + "    " + text + "\n");
                childContentLines.Append(indentChild + "</" + arg.Key + ">\n");
            }
        }

        if (childContentLines.Length > 0)
        {
            if (tagMatch.CloseTag == null)
            {
                return string.Concat(
                    codeText.Substring(0, tagMatch.Attrs.Index + tagMatch.Attrs.Length - 1).TrimEnd(), ">\n",
                    childContentLines.ToString(),
                    indent, "</", tagMatch.OpenTag.Groups["tagName"].Value, ">",
                    codeText.Substring(tagMatch.OpenTag.Index + tagMatch.OpenTag.Length));
            }
            else
            {
                return string.Concat(
                    codeText.Substring(0, tagMatch.OpenTag.Index + tagMatch.OpenTag.Length), "\n",
                    childContentLines.ToString(),
                    childContent.TrimStart('\n'),
                    codeText.Substring(tagMatch.CloseTag.Index));
            }
        }

        return codeText;
    }

    private static IEnumerable<string> ArgumentsToAttributeStrings(UpdateSourceContext context, Arguments args)
    {
        foreach (var param in context.Parameters)
        {
            if (RenderFragmentKit.IsRenderFragment(param.Type)) continue;
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
