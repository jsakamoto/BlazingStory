using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// This class contains extension methods for the HTML elements.
/// </summary>
internal static class HtmlHelper
{
    internal static readonly HashSet<string> HtmlTags = new HashSet<string>
    {
        // Document metadata
        "html", "head", "title", "base", "link", "meta", "style", "script", "noscript",

        // Sections
        "body", "section", "nav", "article", "aside", "h1", "h2", "h3", "h4", "h5", "h6",
        "header", "footer", "address", "main", "figure", "figcaption",

        // Grouping content
        "p", "hr", "pre", "blockquote", "ol", "ul", "li", "dl", "dt", "dd", "div",

        // Text-level semantics
        "a", "em", "strong", "small", "s", "cite", "q", "dfn", "abbr", "data", "time",
        "code", "var", "samp", "kbd", "sub", "sup", "i", "b", "u", "mark", "ruby",
        "rt", "rp", "bdi", "bdo", "span", "br", "wbr", "ins", "del",

        // Embedded content
        "picture", "source", "img", "iframe", "embed", "object", "param", "video",
        "audio", "track", "map", "area", "canvas", "math", "svg",

        // Table content
        "table", "caption", "colgroup", "col", "tbody", "thead", "tfoot", "tr", "td",
        "th",

        // Forms
        "form", "fieldset", "legend", "label", "input", "button", "select",
        "datalist", "optgroup", "option", "textarea", "output", "progress", "meter",

        // Interactive elements
        "details", "summary", "dialog", "menu", "menuitem",

        // Scripting
        "template", "script", "noscript",

        // Deprecated tags
        "applet", "acronym", "bgsound", "dir", "frame", "frameset", "noframes",
        "isindex", "keygen", "listing", "marquee", "menuitem", "nextid", "plaintext",
        "rb", "rtc", "strike", "xmp", "big", "blink", "center", "font", "spacer",
        "tt", "basefont", "u1", "u2", "u3", "nobr", "noframes", "noembed",
        "layer", "ilayer", "nolayer", "blink", "b", "i", "tt", "small", "big",
        "object", "param", "applet", "s", "strike", "u", "spacer", "acronym",
        "menuitem", "listing", "plaintext", "xmp",

        // Others you may encounter in specific contexts
        "caption", "colgroup", "col", "tbody", "thead", "tfoot", "tr", "td", "th"
    };

    internal static bool IsHtmlTag(this string tagName)
    {
        var isHtmlTag = HtmlTags.Contains(tagName.ToLower());

        return isHtmlTag;
    }

    internal static string GetTagName(this string markupContent)
    {
        // Extract the tag name from the markup content
        var match = Regex.Match(markupContent, @"<\s*(\w+)[^>]*>");

        var tagName = match.Success ? match.Groups[1].Value : string.Empty;

        return tagName;
    }
}
