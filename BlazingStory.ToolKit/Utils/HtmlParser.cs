using System.Text.RegularExpressions;

namespace BlazingStory.ToolKit.Utils;

internal static class HtmlParser
{
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

    internal static List<HtmlElement>? ParseMarkupString(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return null;

        html = TransformSelfClosingTags(html);

        var elements = new List<HtmlElement>();
        var tagPattern = new Regex(@"<(?<tag>\w+)(?<attributes>(?:\s+\w+=""[^""]*""|\s+\w+='[^']*'|\s+\w+=(?:""[^""]*""|'[^']*'|[^>\s]+))*?)\s*\/?>|<\/(?<closingTag>\w+)>|(?<text>[^<]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var stack = new Stack<HtmlElement>();
        var currentElement = default(HtmlElement?);

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

                    // If the text precedes this child element, keep it as a text node to preserve the order of the text and the child elements.
                    if (IsHtmlElement(currentElement) && currentElement.Children.Count == 0 && !string.IsNullOrWhiteSpace(currentElement.RawContent))
                    {
                        currentElement.Children.Add(new HtmlElement { Content = currentElement.RawContent });
                        currentElement.Content = null;
                        currentElement.RawContent = null;
                    }

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
                    var text = match.Groups["text"].Value;
                    if (IsHtmlElement(currentElement) && currentElement.Children is { Count: > 0 })
                    {
                        // The text follows a child element, so keep it as a text node to preserve the order of the text and the child elements.
                        currentElement.Children.Add(new HtmlElement { Content = text });
                    }
                    else
                    {
                        currentElement.Content += text.Trim();
                        currentElement.RawContent += text;
                    }
                }
                else
                {
                    // The text is at the top level, next to the top level elements.
                    elements.Add(new HtmlElement { Content = match.Groups["text"].Value });
                }
            }
        }

        // If there is no element at all, the given string is a plain text, and the caller renders it as it is.
        if (!elements.Any(element => element.TagName != null)) return [];

        return elements;
    }

    private static bool IsHtmlElement(HtmlElement element) => element.TagName?.IsHtmlTag() == true;

    private static Dictionary<string, string>? ParseAttributes(string? attributes)
    {
        if (string.IsNullOrWhiteSpace(attributes)) return null;

        var attributesDict = new Dictionary<string, string>();
        var attributePattern = new Regex(@"(?<name>\w+)\s*=\s*""(?<value>[^""]*)""|(?<nameNoQuotes>\w+)\s*=\s*(?<valueNoQuotes>[^\s>]+)");

        var matches = attributePattern.Matches(attributes);

        foreach (Match match in matches)
        {
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
/// This class represents an HTML tag, or a text node when the <see cref="TagName"/> is null.
/// </summary>
internal class HtmlElement
{
    public string? TagName { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
    public string? Content { get; set; }

    /// <summary>
    /// The text content as it was written, including the leading and trailing white spaces that the <see cref="Content"/> doesn't have.
    /// </summary>
    public string? RawContent { get; set; }

    public List<HtmlElement>? Children { get; set; }
}
