using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Utils;

public static class HelperExtension
{
    public static string GenerateUniqueId()
    {
        var guidBytes = Guid.NewGuid().ToByteArray();
        var base64Guid = Convert.ToBase64String(guidBytes);
        var sanitizedGuid = Regex.Replace(base64Guid, @"[/+=]", "-");
        return sanitizedGuid.Substring(0, 10).Replace("-", "_");
    }

    public static string GenerateUniqueId(this string? id)
    {
        var guidBytes = Guid.NewGuid().ToByteArray();
        var base64Guid = Convert.ToBase64String(guidBytes);
        var sanitizedGuid = Regex.Replace(base64Guid, @"[/+=]", "-");
        var uniqueId = sanitizedGuid.Substring(0, 10).Replace("-", "_");
        var returnResponse = string.IsNullOrWhiteSpace(id) ? uniqueId : $"{uniqueId}_{id.Trim()}";

        return returnResponse;
    }
}
