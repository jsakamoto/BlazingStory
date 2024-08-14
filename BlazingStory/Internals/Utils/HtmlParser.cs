using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Utils;

internal static class HtmlParser
{
    internal static List<HtmlElement>? ParseMarkupString(this string markupString)
    {
        var listHtmlElement = ParseElements(markupString);

        return listHtmlElement;
    }

    private static string TransformSelfClosingTags(string input)
    {
        // Define regex pattern to match self-closing tags
        var pattern = @"<(?<tag>\w+)(?<attributes>(?:\s+\w+=""[^""]*""|\s+\w+='[^']*'|\s+\w+=(?:""[^""]*""|'[^']*'|[^>\s]+))*?)\s*/>";

        // Use Regex.Replace to transform the input
        var transformed = Regex.Replace(input, pattern, match =>
        {
            var tag = match.Groups["tag"].Value;
            var attributes = match.Groups["attributes"].Value;

            // Construct the opening and closing tags
            var openingTag = $"<{tag}{attributes}>";
            var closingTag = $"</{tag}>";

            // Return the transformed tag
            return openingTag + closingTag;
        }, RegexOptions.Multiline);

        return transformed;
    }

    private static List<HtmlElement>? ParseElements(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return null!;
        }

        html = TransformSelfClosingTags(html);

        var elements = new List<HtmlElement>();
        var tagPattern = new Regex(@"<(?<tag>\w+)(?<attributes>(?:\s+\w+=""[^""]*""|\s+\w+='[^']*'|\s+\w+=(?:""[^""]*""|'[^']*'|[^>\s]+))*?)\s*\/?>|<\/(?<closingTag>\w+)>|(?<text>[^<]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var stack = new Stack<HtmlElement>();
        HtmlElement? currentElement = null;

        var matches = tagPattern.Matches(html);

        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];

            if (match.Groups["tag"].Success)
            {
                // Opening tag
                var tagName = match.Groups["tag"].Value;
                var attributes = ParseAttributes(match.Groups["attributes"].Value);

                var newElement = new HtmlElement
                {
                    TagName = tagName,
                    Attributes = attributes,
                    Children = []
                };

                if (currentElement != null)
                {
                    stack.Push(currentElement);

                    currentElement.Children ??= [];

                    currentElement.Children.Add(newElement);
                }
                else
                {
                    elements.Add(newElement);
                }

                currentElement = newElement;
            }
            else if (match.Groups["closingTag"].Success)
            {
                // Closing tag
                if (currentElement != null && currentElement.TagName == match.Groups["closingTag"].Value)
                {
                    if (stack.Count > 0)
                    {
                        currentElement = stack.Pop();
                    }
                    else
                    {
                        currentElement = null;
                    }
                }
                else
                {
                    // Handle mismatched or stray closing tags if necessary
                }
            }
            else if (match.Groups["text"].Success)
            {
                // Text content
                if (currentElement != null)
                {
                    currentElement.Content += match.Groups["text"].Value.Trim();
                }
            }
        }

        return elements;
    }

    private static Dictionary<string, string>? ParseAttributes(string? attributes)
    {
        if (string.IsNullOrWhiteSpace(attributes))
        {
            return null!;
        }

        var attributesDict = new Dictionary<string, string>();
        var attributePattern = new Regex(@"(?<name>\w+)\s*=\s*""(?<value>[^""]*)""|(?<nameNoQuotes>\w+)\s*=\s*(?<valueNoQuotes>[^\s>]+)");

        var matches = attributePattern.Matches(attributes);

        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];

            if (match.Groups["name"].Success)
            {
                attributesDict[match.Groups["name"].Value] = match.Groups["value"].Value;
            }
            else if (match.Groups["nameNoQuotes"].Success)
            {
                attributesDict[match.Groups["nameNoQuotes"].Value] = match.Groups["valueNoQuotes"].Value;
            }
        }

        return attributesDict;
    }
}

/// <summary>
/// This class represents an HTML tag.
/// </summary>
public class HtmlElement
{
    public string? TagName { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
    public string? Content { get; set; }
    public List<HtmlElement>? Children { get; set; }
}
