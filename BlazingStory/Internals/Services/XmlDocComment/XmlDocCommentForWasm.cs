using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Services.XmlDocComment;

/// <summary>
/// Provides XML document comment of types for Blazor WebAssembly apps.
/// </summary>
internal class XmlDocCommentForWasm : IXmlDocComment
{
    private readonly HttpClient _HttpClient;

    private readonly ILogger<XmlDocCommentForWasm> _Logger;

    private readonly SemaphoreSlim _Syncer = new SemaphoreSlim(1);

    private class XmlDocCommentCacheEntity
    {
        public DateTime TimestampUTC = DateTime.UtcNow;

        public readonly XDocument? XmlDoc;

        public XmlDocCommentCacheEntity(XDocument? xmlDoc) { this.XmlDoc = xmlDoc; }
    }

    /// <summary>
    /// Cache period in seconds.
    /// </summary>
    private const double CachePeriodSec = 180.0;

    private readonly Dictionary<string, XmlDocCommentCacheEntity> _XmlDocCommentCache = new();

    /// <summary>
    /// initialize new instance of <see cref="XmlDocCommentForWasm"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    public XmlDocCommentForWasm(HttpClient httpClient, ILogger<XmlDocCommentForWasm> logger)
    {
        this._HttpClient = httpClient;
        this._Logger = logger;
    }

    /// <summary>
    /// Get summary text of a property from XML document comment file.
    /// </summary>
    /// <param name="ownerType">Type of the property owner.</param>
    /// <param name="propertyName">Name of the property.</param>
    public async ValueTask<MarkupString> GetSummaryOfPropertyAsync(Type ownerType, string propertyName)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(ownerType);
        if (xdocComment == null) return default;

        var memberName = $"P:{ownerType.FullName}.{propertyName}";
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
    /// <param name="componentType">Type for getting summary text.</param>
    public async ValueTask<MarkupString> GetSummaryOfTypeAsync(Type componentType)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);
        if (xdocComment == null) return default;

        var memberName = $"T:{componentType.FullName}";
        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary))
            .FirstOrDefault();
    }

    private async ValueTask<XDocument?> GetXmlDocCommentXDocAsync(Type type)
    {
        await this._Syncer.WaitAsync();
        try
        {
            var assemblyName = type.Assembly.GetName().Name;
            if (string.IsNullOrEmpty(assemblyName)) return null;

            var xdocComment = default(XDocument);
            if (this._XmlDocCommentCache.TryGetValue(assemblyName, out var cacheEntity) && (DateTime.UtcNow - cacheEntity.TimestampUTC).TotalSeconds < CachePeriodSec)
            {
                cacheEntity.TimestampUTC = DateTime.UtcNow;
                xdocComment = cacheEntity.XmlDoc;
            }
            else
            {
                try
                {
                    var xdocUrl = $"./_framework/{assemblyName}.xml";
                    var xdocContent = await this._HttpClient.GetStringAsync(xdocUrl);
                    xdocComment = XDocument.Parse(xdocContent);
                }
                catch (Exception ex)
                {
                    this._Logger.LogError(ex, ex.Message);
                    xdocComment = null;
                }

                this._XmlDocCommentCache[assemblyName] = new(xdocComment);
            }
            return xdocComment;
        }
        finally { this._Syncer.Release(); }
    }


    /// <summary>
    /// Get inner text of a XML document comment element.<br/>
    /// (e.g. <c>See also &lt;see cref="F:Foo.Bar.Fizz.Buzz"/&gt;</c> =&gt; <c>See also "Fizz.Buzz".</c>))
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
