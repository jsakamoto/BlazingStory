using System.Xml;
using System.Xml.Linq;
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

        public readonly XDocument XmlDoc;

        public XmlDocCommentCacheEntity(XDocument xmlDoc) { this.XmlDoc = xmlDoc; }
    }

    /// <summary>
    /// Cache period in seconds.
    /// </summary>
    private const double CachePeriodSec = 1.0;

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
    public async ValueTask<string> GetSummaryOfPropertyAsync(Type ownerType, string propertyName)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(ownerType);
        if (xdocComment == null) return "";

        var memberName = $"P:{ownerType.FullName}.{propertyName}";
        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary))
            .FirstOrDefault() ?? "";
    }

    /// <summary>
    /// Get summary text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">Type for getting summary text.</param>
    public async ValueTask<string> GetSummaryOfTypeAsync(Type componentType)
    {
        var xdocComment = await this.GetXmlDocCommentXDocAsync(componentType);
        if (xdocComment == null) return "";

        var memberName = $"T:{componentType.FullName}";
        return xdocComment
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => GetInnerText(summary))
            .FirstOrDefault() ?? "";
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
                var xdocUrl = $"./_framework/{assemblyName}.xml";
                var xdocContent = "";
                try { xdocContent = await this._HttpClient.GetStringAsync(xdocUrl); }
                catch (Exception ex)
                {
                    this._Logger.LogError(ex, ex.Message);
                    return null;
                }

                xdocComment = XDocument.Parse(xdocContent);

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
    private static string GetInnerText(XElement element)
    {
        static string getAttrText(XElement element, string attrName)
        {
            var attrValue = element.Attribute(attrName)?.Value ?? "";
            return "\"" + string.Join('.', attrValue.Split('.').TakeLast(2)) + "\"";
        }

        return string.Concat(element
            .Nodes()
            .Select(node => node switch
            {
                XElement e => e.NodeType switch
                {
                    XmlNodeType.Element => e.Name.LocalName switch
                    {
                        "see" => getAttrText(e, "cref"),
                        "paramref" => getAttrText(e, "name"),
                        "typeparamref" => getAttrText(e, "name"),
                        _ => e.Value
                    },
                    _ => e.Value
                },
                _ => node.ToString()
            })
        ).Trim();
    }


    //public static string GetSummaryOfProperty(Type ownerType, string propertyName)
    //{
    //    if (ownerType.Assembly.Location == "") return "";

    //    var xdocPath = Path.ChangeExtension(new Uri(ownerType.Assembly.Location).LocalPath, ".xml");
    //    if (!File.Exists(xdocPath)) return "";

    //    var memberName = $"P:{ownerType.FullName}.{propertyName}";

    //    return XDocument.Load(xdocPath)
    //        .Descendants("member")
    //        .Where(member => member.Attribute("name")?.Value == memberName)
    //        .SelectMany(member => member.Descendants("summary"))
    //        .Select(summary => summary.Value.Trim())
    //        .FirstOrDefault() ?? "";
    //}
}
