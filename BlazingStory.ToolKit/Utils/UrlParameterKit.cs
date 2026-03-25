namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// Utility class for handling URL parameters.
/// </summary>
public static class UriParameterKit
{
    /// <summary>
    /// Encodes a dictionary of key-value pairs into a semicolon-delimited, URL-escaped string.
    /// </summary>
    /// <param name="keyValues">The key-value pairs to encode.</param>
    public static string EncodeKeyValues<TValue>(IReadOnlyDictionary<string, TValue>? keyValues)
    {
        if (keyValues == null) return "";
        return string.Join(';', keyValues.Select(kv => Uri.EscapeDataString(kv.Key) + ":" + Uri.EscapeDataString(kv.Value?.ToString() ?? "")));
    }

    /// <summary>
    /// Decodes a semicolon-delimited, URL-escaped string into a dictionary of key-value pairs.
    /// </summary>
    /// <param name="text">The encoded string to decode.</param>
    public static IReadOnlyDictionary<string, string> DecodeKeyValues(string? text)
    {
        return (text ?? "").Split(';')
            .Where(chunk => !string.IsNullOrEmpty(chunk))
            .Select(chunk => chunk.Split(':'))
            .ToDictionary(
                chunk => Uri.UnescapeDataString(chunk[0]),
                chunk => Uri.UnescapeDataString(chunk[1]));
    }

    /// <summary>
    /// Appends the given parameters as a query string to the given URI.
    /// </summary>
    /// <param name="uri">The base URI.</param>
    /// <param name="parameters">The query parameters to append.</param>
    public static string GetUri(string uri, IReadOnlyDictionary<string, object?>? parameters)
    {
        if (parameters == null) return uri;
#pragma warning disable SYSLIB0013 // Type or member is obsolete
        var searchText = string.Join('&', parameters.Select(kv => Uri.EscapeUriString(kv.Key) + "=" + Uri.EscapeUriString(kv.Value?.ToString() ?? "")));
#pragma warning restore SYSLIB0013 // Type or member is obsolete
        if (string.IsNullOrEmpty(searchText)) return uri;

        return uri + "?" + searchText;
    }
}
