using System.Text.Json;

namespace BlazingStory.Test._Fixtures;

internal record DotNetSDKVersion(string Version, string RollForward, bool AllowPrerelease)
{
    /// <summary>
    /// Returns the global.json text for this SDK version.
    /// </summary>
    public string ToGlobalJsonText()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Serialize(new { SDK = this }, options);
    }
}
