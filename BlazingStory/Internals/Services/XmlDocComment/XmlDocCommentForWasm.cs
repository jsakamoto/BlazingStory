using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Services.XmlDocComment;

internal class XmlDocCommentForWasm : IXmlDocComment
{
    private readonly HttpClient _HttpClient;

    private readonly ILogger<XmlDocCommentForWasm> _Logger;

    public XmlDocCommentForWasm(HttpClient httpClient, ILogger<XmlDocCommentForWasm> logger)
    {
        this._HttpClient = httpClient;
        this._Logger = logger;
    }

    public async ValueTask<string> GetSummaryOfPropertyAsync(Type ownerType, string propertyName)
    {
        var assemblyName = ownerType.Assembly.GetName().Name;
        Console.WriteLine($"M-1: [{assemblyName}]");
        if (string.IsNullOrEmpty(assemblyName)) return "";

        Console.WriteLine("M-2");
        var xdocUrl = $"./_framework/{assemblyName}.xml";
        Console.WriteLine("M-3: " + xdocUrl);
        var xdocContent = "";
        try { xdocContent = await this._HttpClient.GetStringAsync(xdocUrl); }
        catch (Exception ex)
        {
            Console.WriteLine("M-ERR");
            this._Logger.LogError(ex, ex.Message);
            return "";
        }

        Console.WriteLine("M-4");
        var memberName = $"P:{ownerType.FullName}.{propertyName}";

        return XDocument.Parse(xdocContent)
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => summary.Value.Trim())
            .FirstOrDefault() ?? "";
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
