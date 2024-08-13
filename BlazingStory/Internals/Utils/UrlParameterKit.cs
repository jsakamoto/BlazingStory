﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Utility class for handling URL parameters.
/// </summary>
internal static class UriParameterKit
{
    internal static string EncodeKeyValues(IReadOnlyDictionary<string, object?>? keyValues)
    {
        if (keyValues == null)
        {
            return "";
        }

        return string.Join(';', keyValues.Select(kv => Uri.EscapeDataString(kv.Key) + ":" + Uri.EscapeDataString(kv.Value?.ToString() ?? "")));
    }

    internal static IReadOnlyDictionary<string, string> DecodeKeyValues(this string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new Dictionary<string, string>();
        }

        var result = text.Split(';')
            .Where(chunk => !string.IsNullOrEmpty(chunk))
            .Select(chunk => chunk.Split(':'))
            .ToDictionary(
                chunk => Uri.UnescapeDataString(chunk[0]),
                chunk => Uri.UnescapeDataString(chunk[1]));

        return result;
    }

    internal static string GetUri(string uri, IReadOnlyDictionary<string, object?>? parameters)
    {
        if (parameters == null)
        {
            return uri;
        }

#pragma warning disable SYSLIB0013 // Type or member is obsolete
        var searchText = string.Join('&', parameters.Select(kv => Uri.EscapeUriString(kv.Key) + "=" + Uri.EscapeUriString(kv.Value?.ToString() ?? "")));
#pragma warning restore SYSLIB0013 // Type or member is obsolete

        if (string.IsNullOrEmpty(searchText))
        {
            return uri;
        }

        return uri + "?" + searchText;
    }

    /// <summary>
    /// Get the query string that ensures the loading of the latest static assets. <br /> This
    /// method will return a string like "?v=1.0.0-preview.2.3" if the browser is online. Otherwise,
    /// "". <br /> (This method depends on the "Toolbelt.Blazor.getProperty" JavaScript function,
    /// which is provided by Toolbelt.Blazor.GetProperty.Script NuGet package)
    /// </summary>
    /// <param name="jSRuntime">
    /// The <see cref="IJSRuntime" /> instance to retrieve browser's on-line status
    /// </param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    internal static string GetUpdateToken(IJSRuntime? jSRuntime)
    {
        if (jSRuntime == null)
        {
            return "";
        }

        var jsInProcRuntime = jSRuntime as IJSInProcessRuntime;

        if (jsInProcRuntime == null)
        {
            return "";
        }

        var isOnLine = jsInProcRuntime.Invoke<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");

        if (!isOnLine)
        {
            return "";
        }

        return "?v=" + VersionInfo.GetEscapedVersionText();
    }
}
