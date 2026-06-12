using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.ToolKit.JSInterop;

/// <summary>
/// Extension methods for <see cref="IJSRuntime"/> and <see cref="IJSObjectReference"/>.
/// </summary>
public static class IJSExtensions
{
    private static readonly JsonSerializerOptions _JsonSerializerOptions = new() { IncludeFields = true };

    /// <summary>
    /// Import a JavaScript module from the specified path.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="modulePath">The path to the JavaScript module to import, like "./Component.razor.js".</param>
    /// <param name="updateToken">A cache-busting token appended as a query string parameter.</param>
    public static async ValueTask<IJSObjectReference> ImportAsync(this IJSRuntime jsRuntime, string modulePath, string updateToken)
    {
        updateToken = jsRuntime.GetUpdateToken(updateToken);
        return await jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath + updateToken);
    }

    /// <summary>
    /// Invoke a JavaScript function with the specified identifier.<br/>
    /// This method will not throw an exception if the <see cref="IJSObjectReference"/> is disconnected.
    /// </summary>
    /// <param name="value">The <see cref="IJSObjectReference"/> instance.</param>
    /// <param name="identifier">An identifier for the function to invoke.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    public static async ValueTask InvokeVoidIfConnectedAsync(this IJSObjectReference? value, string identifier, params object?[]? args)
    {
        try { if (value != null) await value.InvokeVoidAsync(identifier, args); }
        catch (JSDisconnectedException) { }
    }

    /// <summary>
    /// Gets a string value from local storage by key, or null if not found.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    public static async ValueTask<string?> GetLocalStorageItemAsync(this IJSRuntime jsRuntime, string key)
    {
        return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
    }

    /// <summary>
    /// Gets a string value from local storage by key, returning <paramref name="defaultValue"/> if not found.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    /// <param name="defaultValue">The value to return when the key is absent.</param>
    public static async ValueTask<string> GetLocalStorageItemAsync(this IJSRuntime jsRuntime, string key, string defaultValue)
    {
        return await jsRuntime.GetLocalStorageItemAsync(key) ?? defaultValue;
    }

    /// <summary>
    /// Stores a value in local storage under the given key.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    /// <param name="value">The value to store.</param>
    public static async ValueTask SetLocalStorageItemAsync<T>(this IJSRuntime jsRuntime, string key, T? value)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value?.ToString() ?? "");
    }

    /// <summary>
    /// Serializes an object to JSON and saves it in local storage under the given key.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    /// <param name="obj">The object to serialize and store.</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public static async ValueTask SaveObjectToLocalStorageAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(this IJSRuntime jsRuntime, string key, T obj)
    {
        var json = JsonSerializer.Serialize(obj, _JsonSerializerOptions);
        await jsRuntime.SetLocalStorageItemAsync(key, json);
    }

    /// <summary>
    /// Loads and deserializes a JSON object from local storage, returning <paramref name="defaultObject"/> if absent or unparseable.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    /// <param name="defaultObject">The fallback value when the key is absent or deserialization fails.</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public static async ValueTask<T> LoadObjectFromLocalStorageAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(this IJSRuntime jsRuntime, string key, T defaultObject)
    {
        var json = await jsRuntime.GetLocalStorageItemAsync(key);
        if (string.IsNullOrEmpty(json)) return defaultObject;
        return JsonSerializer.Deserialize<T?>(json, _JsonSerializerOptions) ?? defaultObject;
    }

    /// <summary>
    /// Gets a local storage value parsed as <typeparamref name="T"/>, returning <paramref name="defaultValue"/> if absent or unparseable.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="key">The local storage key.</param>
    /// <param name="defaultValue">The fallback value when the key is absent or parsing fails.</param>
    public static async ValueTask<T> GetLocalStorageItemAsync<T>(this IJSRuntime jsRuntime, string key, T defaultValue) where T : IParsable<T>
    {
        var stringValue = await jsRuntime.GetLocalStorageItemAsync(key);
        return T.TryParse(stringValue, null, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Dispose this <see cref="IJSObjectReference"/> object.<br/>
    /// If <paramref name="methodToCallBeforeDispose"/> is not null, it will be invoked before disposing the object.<br/>
    /// This method will not throw an exception if the <see cref="IJSObjectReference"/> is disconnected.
    /// </summary>
    /// <param name="value">The <see cref="IJSObjectReference"/> instance.</param>
    /// <param name="methodToCallBeforeDispose">If specified, this method will be invoked before disposing the object.</param>
    public static async ValueTask DisposeIfConnectedAsync(this IJSObjectReference? value, string? methodToCallBeforeDispose = null)
    {
        if (value == null) return;
        try
        {
            if (methodToCallBeforeDispose != null) await value.InvokeVoidAsync(methodToCallBeforeDispose);
            await value.DisposeAsync();
        }
        catch (JSDisconnectedException) { }
    }

    /// <summary>
    /// Get the query string that ensures the loading of the latest static assets.<br/>
    /// This method will return a string like "?v=1.0.0-preview.2.3" if the browser is online. Otherwise, "".<br/>
    /// (This method depends on the "Toolbelt.Blazor.getProperty" JavaScript function, which is provided by Toolbelt.Blazor.GetProperty.Script NuGet package)
    /// </summary>
    /// <param name="jSRuntime">The <see cref="IJSRuntime"/> instance to retrieve browser's on-line status</param>
    /// <param name="updateToken">The update token to be included in the query string. This is usually a version number or a timestamp that changes whenever the static assets are updated.</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public static string GetUpdateToken(this IJSRuntime jSRuntime, string updateToken)
    {
        if (jSRuntime is IJSInProcessRuntime jsInProcRuntime)
        {
#if NET10_0_OR_GREATER
            var isOnLine = jsInProcRuntime.GetValue<bool>("navigator.onLine");
#else
            var isOnLine = jsInProcRuntime.Invoke<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");
#endif
            if (!isOnLine) return "";
        }

        return "?v=" + updateToken;
    }
}
