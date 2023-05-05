using Microsoft.JSInterop;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Utility class for handling URL parameters.
/// </summary>
internal static class UriParameterKit
{
    internal static string EncodeKeyValues(IReadOnlyDictionary<string, object?>? keyValues)
    {
        if (keyValues == null) return "";
        return string.Join(';', keyValues.Select(kv => Uri.EscapeDataString(kv.Key) + ":" + Uri.EscapeDataString(kv.Value?.ToString() ?? "")));
    }

    internal static IReadOnlyDictionary<string, string> DecodeKeyValues(string? text)
    {
        return (text ?? "").Split(';')
            .Where(chunk => !string.IsNullOrEmpty(chunk))
            .Select(chunk => chunk.Split(':'))
            .ToDictionary(
                chunk => Uri.UnescapeDataString(chunk[0]),
                chunk => Uri.UnescapeDataString(chunk[1]));
    }

    internal static string GetUri(string uri, IReadOnlyDictionary<string, object?>? parameters)
    {
        if (parameters == null) return uri;
#pragma warning disable SYSLIB0013 // Type or member is obsolete
        var searchText = string.Join('&', parameters.Select(kv => Uri.EscapeUriString(kv.Key) + "=" + Uri.EscapeUriString(kv.Value?.ToString() ?? "")));
#pragma warning restore SYSLIB0013 // Type or member is obsolete
        if (string.IsNullOrEmpty(searchText)) return uri;

        return uri + "?" + searchText;
    }

    /// <summary>
    /// Get the query string that ensures the loading of the latest static assets.<br/>
    /// This method will return a string like "?v=1.0.0-preview.2.3" if the browser is online. Otherwise, "".<br/>
    /// (This method depends on the "BlazingStory.isOnLine" JavaScript function, which is defined in the "/wwwroot/BlazingStory.lib.module.js")
    /// </summary>
    /// <param name="jSRuntime">The <see cref="IJSRuntime"/> instance to retrieve browser's on-line status</param>
    internal static string GetUpdateToken(IJSRuntime jSRuntime)
    {
        var jsInProcRuntime = jSRuntime as IJSInProcessRuntime;
        if (jsInProcRuntime == null) return "";

        var isOnLine = jsInProcRuntime.Invoke<bool>("BlazingStory.isOnLine");
        if (!isOnLine) return "";

        return "?v=" + Uri.EscapeDataString(VersionUtility.GetVersionText());
    }
}
