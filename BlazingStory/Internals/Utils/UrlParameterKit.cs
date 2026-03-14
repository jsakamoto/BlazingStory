namespace BlazingStory.Internals.Utils;

/// <summary>
/// Utility class for handling URL parameters.
/// </summary>
internal static class UriParameterKit
{
    internal static string EncodeKeyValues<TValue>(IReadOnlyDictionary<string, TValue>? keyValues)
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
}
