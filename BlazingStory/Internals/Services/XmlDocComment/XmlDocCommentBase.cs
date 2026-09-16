using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using BlazingStory.ToolKit.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.XmlDocComment;

internal abstract class XmlDocCommentBase : IXmlDocComment
{
    /// <summary>
    /// Get summary text of a property from XML document comment file.
    /// </summary>
    /// <param name="ownerType">Type of the property owner.</param>
    /// <param name="propertyName">Name of the property.</param>
    public async ValueTask<MarkupString> GetSummaryOfPropertyAsync(Type ownerType, string propertyName)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(ownerType);
        if (xdocComment == null) return default;

        var memberName = $"P:{ownerType.Namespace}.{ownerType.Name}.{propertyName}";
        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary, ownerType))
            .FirstOrDefault();
    }

    /// <summary>
    /// Get summary text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">Type for getting summary text.</param>
    public async ValueTask<MarkupString> GetSummaryOfTypeAsync(Type componentType)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);
        if (xdocComment == null) return default;

        var componentOpenType = TypeUtility.GetOpenType(componentType);

        var memberName = $"T:{componentOpenType.FullName}";
        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary, componentType))
            .FirstOrDefault();
    }

    protected abstract ValueTask<XDocument?> GetXmlDocCommentXDocAsync(Type type);

    /// <summary>
    /// Get inner text of a XML document comment element.<br/>
    /// (e.g. <c>See also &lt;see cref="F:Foo.Bar.Fizz.Buzz"/&gt;</c> =&gt; <c>See also "Fizz.Buzz".</c>))
    /// </summary>
    /// <param name="element">The XML document comment element.</param>
    /// <param name="contextType">The type that owns the XML document comment, used for resolving "cref" references.</param>
    private static MarkupString GetInnerText(XElement element, Type? contextType)
    {
        var innerText = ConcatNodes(element, contextType);
        innerText = Regex.Replace(innerText, "^(\\s|&#xD;|&#xA;)*", "");
        innerText = Regex.Replace(innerText, "(\\s|&#xD;|&#xA;)*$", "");
        return (MarkupString)innerText;
    }

    /// <summary>
    /// Concatenate the text of the child nodes of a XML document comment element.<br/>
    /// An element that has no special meaning is rewritten into the text of its own child nodes, so that the
    /// elements nested inside it - such as a "see" inside a "para" - are not dropped.
    /// </summary>
    /// <param name="element">The XML document comment element.</param>
    /// <param name="contextType">The type that owns the XML document comment, used for resolving "cref" references.</param>
    private static string ConcatNodes(XElement element, Type? contextType)
    {
        static string encode(string? text) => HtmlEncoder.Default.Encode(text ?? "");

        static string quote(string? text) => "\"" + encode(text) + "\"";

        string seeText(XElement e) =>
            e.Attribute("href") is { } href ? $"<a href=\"{href.Value}\" target=\"_blank\">{e.Value}</a>" :
            e.Attribute("langword") is { } langword ? encode(langword.Value) :
            quote(CrefText.GetDisplayText(e.Attribute("cref")?.Value ?? "", contextType));

        return string.Concat(element
            .Nodes()
            .Select(node => node switch
            {
                XElement e => e.NodeType switch
                {
                    XmlNodeType.Element => e.Name.LocalName switch
                    {
                        "see" or "seealso" => seeText(e),
                        "paramref" or "typeparamref" => quote(e.Attribute("name")?.Value),
                        _ => ConcatNodes(e, contextType)
                    },
                    _ => encode(e.Value)
                },
                _ => encode(node.ToString())
            })
        );
    }
}
