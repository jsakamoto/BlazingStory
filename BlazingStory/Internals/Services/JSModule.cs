using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Utils;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services;

internal class JSModule : IAsyncDisposable
{
    private IJSObjectReference? _Module;

    private readonly Func<IJSRuntime> _GetJSRuntime;

    private readonly string _ModulePath;

    private const string _DefaultBasePath = "./_content/BlazingStory/";

    internal JSModule(Func<IJSRuntime> jSRuntime, string modulePath)
    {
        this._GetJSRuntime = jSRuntime;
        this._ModulePath = modulePath;
    }

    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        if (this._Module == null)
        {
            var jsRuntime = this._GetJSRuntime();
            var updateToken = UriParameterKit.GetUpdateToken(jsRuntime);
            this._Module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", _DefaultBasePath + this._ModulePath + updateToken);
        }
        return this._Module;
    }

    public async ValueTask InvokeVoidIfConnectedAsync(string identifier, params object?[]? args)
    {
        try { await this.InvokeVoidAsync(identifier, args); }
        catch (JSDisconnectedException) { }
    }

    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        var module = await this.GetModuleAsync();
        await module.InvokeVoidAsync(identifier, args);
    }

    public async ValueTask<T> InvokeAsync<T>(string identifier, params object?[]? args)
    {
        var module = await this.GetModuleAsync();
        return await module.InvokeAsync<T>(identifier, args);
    }

    public async ValueTask DisposeAsync()
    {
        await this._Module.DisposeIfConnectedAsync();
    }
}
