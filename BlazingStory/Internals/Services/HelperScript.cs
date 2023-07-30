using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services;

internal class HelperScript : IAsyncDisposable
{
    private readonly JSModule _JSModule;

    private readonly JsonSerializerOptions JsonSerializerOptions = new() { IncludeFields = true };

    public HelperScript(IJSRuntime jSRuntime)
    {
        this._JSModule = new(() => jSRuntime, "helper.min.js");
    }

    internal async ValueTask InvokeVoidAsync(string id, params object?[]? args)
    {
        await this._JSModule.InvokeVoidAsync(id, args);
    }

    internal async ValueTask<T> InvokeAsync<T>(string id, params object?[]? args)
    {
        return await this._JSModule.InvokeAsync<T>(id, args);
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

    internal async ValueTask<string> GetLocalStorageItemAsync(string key, string defaultValue) => await this.InvokeAsync<string?>("getLocalStorageItem", key) ?? defaultValue;

    public async ValueTask<T> GetLocalStorageItemAsync<T>(string key, T defaultValue) where T : IParsable<T>
    {
        var stringValue = await this.GetLocalStorageItemAsync(key);
        return T.TryParse(stringValue, null, out var value) ? value : defaultValue;
    }

    internal ValueTask SetupKeyDownSenderAsync() => this.InvokeVoidAsync("setupKeyDownSender");

    internal ValueTask SetupKeyDownReceiverAsync() => this.InvokeVoidAsync("setupKeyDownReceiver");

    public async ValueTask DisposeAsync()
    {
        await this._JSModule.DisposeAsync();
    }
}
