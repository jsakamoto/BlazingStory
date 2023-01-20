using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services;

internal class HelperScript : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _Module;

    private readonly JsonSerializerOptions JsonSerializerOptions = new() { IncludeFields = true };

    internal HelperScript(IJSRuntime jSRuntime)
    {
        this._Module = new(async () => await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazingStory/helper.min.js"));
    }

    public async ValueTask SaveObjectToLocalStorageAsync<T>(string key, T obj)
    {
        var json = JsonSerializer.Serialize(obj, this.JsonSerializerOptions);
        await this.SetLocalStorageItemAsync(key, json);
    }

    public async ValueTask<T> LoadObjectFromLocalStorageAsync<T>(string key, T defaultObject)
    {
        var json = await this.GetLocalStorageItemAsync(key);
        if (string.IsNullOrEmpty(json)) return defaultObject;
        return JsonSerializer.Deserialize<T?>(json, this.JsonSerializerOptions) ?? defaultObject;
    }

    public async ValueTask SetLocalStorageItemAsync<T>(string key, T? value)
    {
        var module = await this._Module.Value;
        await module.InvokeVoidAsync("setLocalStorageItem", key, value?.ToString() ?? "");
    }

    public async ValueTask<string?> GetLocalStorageItemAsync(string key)
    {
        var module = await this._Module.Value;
        return await module.InvokeAsync<string?>("getLocalStorageItem", key);
    }

    public async ValueTask<T> GetLocalStorageItemAsync<T>(string key, T defaultValue) where T : IParsable<T>
    {
        var stringValue = await this.GetLocalStorageItemAsync(key);
        return T.TryParse(stringValue, null, out var value) ? value : defaultValue;
    }

    public async ValueTask DisposeAsync()
    {
        if (this._Module.IsValueCreated) return;
        var module = await this._Module.Value;
        try { await module.DisposeAsync(); } catch (JSDisconnectedException) { }
    }
}
