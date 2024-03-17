using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Services.XmlDocComment;

/// <summary>
/// Provides XML document comment of types for Blazor Server apps.
/// </summary>
internal class XmlDocCommentForServer : XmlDocCommentBase
{
    private readonly ILogger<XmlDocCommentForServer> _Logger;

    public XmlDocCommentForServer(ILogger<XmlDocCommentForServer> logger)
    {
        this._Logger = logger;
    }

    protected override ValueTask<XDocument?> GetXmlDocCommentXDocAsync(Type type)
    {
        var assemblyName = type.Assembly.GetName().Name;
        if (string.IsNullOrEmpty(assemblyName)) return ValueTask.FromResult(default(XDocument));

        try
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var xdocPath = Path.Combine(baseDir, assemblyName + ".xml");
            var xdocComment = XDocument.Load(xdocPath);

            return ValueTask.FromResult<XDocument?>(xdocComment);
        }
        catch (Exception ex)
        {
            this._Logger.LogError(ex, ex.Message);
            return ValueTask.FromResult(default(XDocument));
        }
    }
}
