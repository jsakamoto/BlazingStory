using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazingStory.Internals.Utils;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Services;

internal class HelperScript : IAsyncDisposable
{
    private readonly IJSRuntime _JSRuntime;

    private readonly JSModule _JSModule;

    private readonly JsonSerializerOptions JsonSerializerOptions = new() { IncludeFields = true };

    public HelperScript(IJSRuntime jSRuntime)
    {
        this._JSRuntime = jSRuntime;
        this._JSModule = JSModuleFactory.Create(() => jSRuntime, "js/helper.min.js");
    }

    internal async ValueTask InvokeVoidAsync(string id, params object?[]? args)
    {
        await this._JSModule.InvokeVoidAsync(id, args);
    }

    internal async ValueTask<T> InvokeAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(string id, params object?[]? args)
    {
        return await this._JSModule.InvokeAsync<T>(id, args);
    }

    internal ValueTask CopyTextToClipboardAsync(string text) => this.InvokeVoidAsync("copyTextToClipboard", text);

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    internal async ValueTask SaveObjectToLocalStorageAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(string key, T obj)
    {
        var json = JsonSerializer.Serialize(obj, this.JsonSerializerOptions);
        await this._JSRuntime.SetLocalStorageItemAsync(key, json);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    internal async ValueTask<T> LoadObjectFromLocalStorageAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicFields | PublicProperties)] T>(string key, T defaultObject)
    {
        var json = await this._JSRuntime.GetLocalStorageItemAsync(key);
        if (string.IsNullOrEmpty(json)) return defaultObject;
        return JsonSerializer.Deserialize<T?>(json, this.JsonSerializerOptions) ?? defaultObject;
    }

    public async ValueTask<T> GetLocalStorageItemAsync<T>(string key, T defaultValue) where T : IParsable<T>
    {
        var stringValue = await this._JSRuntime.GetLocalStorageItemAsync(key);
        return T.TryParse(stringValue, null, out var value) ? value : defaultValue;
    }

    internal ValueTask SetupKeyDownReceiverAsync() => this.InvokeVoidAsync("setupMessageReceiverFromIFrame");

    public async ValueTask DisposeAsync()
    {
        await this._JSModule.DisposeAsync();
    }
}
