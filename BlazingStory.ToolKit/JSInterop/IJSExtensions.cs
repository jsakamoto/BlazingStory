using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace BlazingStory.ToolKit.JSInterop;

public static class IJSExtensions
{
    /// <summary>
    /// Import a JavScript module from the specified path.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    /// <param name="modulePath">The path to the JavaScript module to import, like "./Component.razor.js".</param>
    /// <param name="updateToken"></param>
    /// <returns></returns>
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

    public static async ValueTask<string?> GetLocalStorageItemAsync(this IJSRuntime jsRuntime, string key)
    {
        return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
    }

    public static async ValueTask<string> GetLocalStorageItemAsync(this IJSRuntime jsRuntime, string key, string defaultValue)
    {
        return await jsRuntime.GetLocalStorageItemAsync(key) ?? defaultValue;
    }

    public static async ValueTask SetLocalStorageItemAsync<T>(this IJSRuntime jsRuntime, string key, T? value)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value?.ToString() ?? "");
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
    private static string GetUpdateToken(this IJSRuntime jSRuntime, string updateToken)
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
