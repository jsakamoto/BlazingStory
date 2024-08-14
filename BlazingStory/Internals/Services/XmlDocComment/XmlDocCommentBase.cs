using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.XmlDocComment;

internal abstract class XmlDocCommentBase : IXmlDocComment
{
    /// <summary>
    /// Get summary text of a property from XML document comment file.
    /// </summary>
    /// <param name="ownerType">
    /// Type of the property owner.
    /// </param>
    /// <param name="propertyName">
    /// Name of the property.
    /// </param>
    public async ValueTask<MarkupString> GetSummaryOfPropertyAsync(Type ownerType, string propertyName)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(ownerType);

        if (xdocComment == null)
        {
            return default;
        }

        var memberName = $"P:{ownerType.Namespace}.{ownerType.Name}.{propertyName}";

        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary))
            .FirstOrDefault();
    }

    /// <summary>
    /// Get summary text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">
    /// Type for getting summary text.
    /// </param>
    /// <returns>
    /// The <see cref="MarkupString" />. <br /> If the remarks text is not found, returns default..
    /// </returns>
    public async ValueTask<MarkupString> GetSummaryOfTypeAsync(Type componentType)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);

        if (xdocComment == null)
        {
            return default;
        }

        var componentOpenType = TypeUtility.GetOpenType(componentType);

        var memberName = $"T:{componentOpenType.FullName}";

        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary))
            .FirstOrDefault();
    }

    /// <summary>
    /// Get remarks text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">
    /// Type for getting remarks text.
    /// </param>
    /// <returns>
    /// The <see cref="MarkupString" />. <br /> If the remarks text is not found, returns default..
    /// </returns>
    public async ValueTask<MarkupString> GetRemarksOfTypeAsync(Type componentType)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);

        if (xdocComment == null)
        {
            return default;
        }

        var memberName = $"T:{componentType.FullName}";

        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("remarks"))
            .Select(remarks => GetInnerText(remarks))
            .FirstOrDefault();
    }

    protected abstract ValueTask<XDocument?> GetXmlDocCommentXDocAsync(Type type);

    /// <summary>
    /// Get inner text of a XML document comment element. <br /> (e.g. <c>See also &lt;see
    /// cref="F:Foo.Bar.Fizz.Buzz"/&gt;</c> =&gt; <c>See also "Fizz.Buzz".</c>))
    /// </summary>
    private static MarkupString GetInnerText(XElement element)
    {
        static string encode(string? text) => HtmlEncoder.Default.Encode(text ?? "");

        static string getAttrText(XElement element, string attrName)
        {
            var attrValue = element.Attribute(attrName)?.Value ?? "";

            return "\"" + encode(string.Join('.', attrValue.Split('.').TakeLast(2))) + "\"";
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
                            getAttrText(e, "cref"),
                        "paramref" => getAttrText(e, "name"),
                        "typeparamref" => getAttrText(e, "name"),
                        _ => encode(e.Value)
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
