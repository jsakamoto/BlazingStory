using System.Xml.Linq;

namespace BlazingStory.Internals.Services;

internal class XmlDocComment
{
    public static string GetSummaryOfProperty(Type ownerType, string propertyName)
    {
        if (ownerType.Assembly.Location == "") return "";

        var xdocPath = Path.ChangeExtension(new Uri(ownerType.Assembly.Location).LocalPath, ".xml");
        if (!File.Exists(xdocPath)) return "";

        var memberName = $"P:{ownerType.FullName}.{propertyName}";

        return XDocument.Load(xdocPath)
            .Descendants("member")
            .Where(member => member.Attribute("name")?.Value == memberName)
            .SelectMany(member => member.Descendants("summary"))
            .Select(summary => summary.Value.Trim())
            .FirstOrDefault() ?? "";
    }
}
