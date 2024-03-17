using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Services.XmlDocComment;

/// <summary>
/// Provides XML document comment of types for Blazor WebAssembly apps.
/// </summary>
internal class XmlDocCommentForWasm : XmlDocCommentBase
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

    protected override async ValueTask<XDocument?> GetXmlDocCommentXDocAsync(Type type)
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
}
