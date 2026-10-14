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
        return await this.GetSummaryAsync(ownerType, $"P:{ownerType.Namespace}.{ownerType.Name}.{propertyName}");
    }

    /// <summary>
    /// Get summary text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">Type for getting summary text.</param>
    public async ValueTask<MarkupString> GetSummaryOfTypeAsync(Type componentType)
    {
        var componentOpenType = TypeUtility.GetOpenType(componentType);
        return await this.GetSummaryAsync(componentType, $"T:{componentOpenType.FullName}");
    }

    private async ValueTask<MarkupString> GetSummaryAsync(Type componentType, string memberName)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);
        if (xdocComment == null) return default;

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
    private static MarkupString GetInnerText(XElement element, Type ownerType)
    {
        static string encode(string? text) => HtmlEncoder.Default.Encode(text ?? "");

        string getAttrText(XElement element, string attrName)
        {
            var attrValue = element.Attribute(attrName)?.Value ?? "";
            var text = string.Join('.', Regex.Replace(attrValue, "^.:|\\(.*", "").Split('.').TakeLast(attrValue.StartsWith("T:") ? 1 : 2));
            // The XML file keeps only the arity of a generic type (e.g. "MySelect`2"), so names are known only for the owner type and its bases.
            text = Regex.Replace(text, "(\\w+)`+\\d+", m =>
            {
                for (var type = ownerType; type != null; type = type.BaseType)
                {
                    var openType = TypeUtility.GetOpenType(type);
                    if (openType.Name == m.Value) return $"{m.Groups[1]}<{string.Join(", ", openType.GetGenericArguments().Select(t => t.Name))}>";
                }
                return m.Groups[1].Value;
            });
            return "\"" + encode(text) + "\"";
        }

        var innerText = string.Concat(element
            .Nodes()
            .Select(node => node switch
            {
                XElement e => e.NodeType switch
                {
                    XmlNodeType.Element => e.Name.LocalName switch
                    {
                        "see" => e.Attribute("href") != null ?
                            $"<a href=\"{e.Attribute("href")?.Value}\" target=\"_blank\">{e.Value}</a>" :
                            e.Attribute("langword") is { } langword ? encode(langword.Value) :
                            getAttrText(e, "cref"),
                        "paramref" => getAttrText(e, "name"),
                        "typeparamref" => getAttrText(e, "name"),
                        _ => GetInnerText(e, ownerType).Value
                    },
                    _ => encode(e.Value)
                },
                _ => encode(node.ToString())
            })
        );

        innerText = Regex.Replace(innerText, "^(\\s|&#xD;|&#xA;)*", "");
        innerText = Regex.Replace(innerText, "(\\s|&#xD;|&#xA;)*$", "");
        return (MarkupString)innerText;
    }
}
