using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services;

internal class HelperScript : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _Module;

    private readonly JsonSerializerOptions JsonSerializerOptions = new() { IncludeFields = true };

    public HelperScript(IJSRuntime jSRuntime)
    {
        this._Module = new(async () => await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazingStory/helper.min.js"));
    }

    internal async ValueTask InvokeVoidAsync(string id, params object?[]? args)
    {
        var module = await this._Module.Value;
        await module.InvokeVoidAsync(id, args);
    }

    internal async ValueTask<T> InvokeAsync<T>(string id, params object?[]? args)
    {
        var module = await this._Module.Value;
        return await module.InvokeAsync<T>(id, args);
    }

    internal ValueTask CopyTextToClipboardAsync(string text) => this.InvokeVoidAsync("copyTextToClipboard", text);

    internal async ValueTask SaveObjectToLocalStorageAsync<T>(string key, T obj)
    {
        var json = JsonSerializer.Serialize(obj, this.JsonSerializerOptions);
        await this.SetLocalStorageItemAsync(key, json);
    }

    internal async ValueTask<T> LoadObjectFromLocalStorageAsync<T>(string key, T defaultObject)
    {
        var json = await this.GetLocalStorageItemAsync(key);
        if (string.IsNullOrEmpty(json)) return defaultObject;
        return JsonSerializer.Deserialize<T?>(json, this.JsonSerializerOptions) ?? defaultObject;
    }

    internal ValueTask SetLocalStorageItemAsync<T>(string key, T? value) => this.InvokeVoidAsync("setLocalStorageItem", key, value?.ToString() ?? "");

    internal ValueTask<string?> GetLocalStorageItemAsync(string key) => this.InvokeAsync<string?>("getLocalStorageItem", key);

    public async ValueTask<T> GetLocalStorageItemAsync<T>(string key, T defaultValue) where T : IParsable<T>
    {
        var stringValue = await this.GetLocalStorageItemAsync(key);
        return T.TryParse(stringValue, null, out var value) ? value : defaultValue;
    }

    internal ValueTask SetupKeyDownSenderAsync() => this.InvokeVoidAsync("setupKeyDownSender");

    internal ValueTask SetupKeyDownReceiverAsync() => this.InvokeVoidAsync("setupKeyDownReceiver");

    public async ValueTask DisposeAsync()
    {
        if (this._Module.IsValueCreated) return;
        var module = await this._Module.Value;
        try { await module.DisposeAsync(); } catch (JSDisconnectedException) { }
    }
}
