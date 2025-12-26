using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Extensions;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Services;

internal class JSModule : IAsyncDisposable
{
    private IJSObjectReference? _Module;

    private readonly Func<IJSRuntime> _GetJSRuntime;

    private readonly string _ModulePath;

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
            this._Module = await jsRuntime.ImportAsync(this._ModulePath);
        }
        return this._Module;
    }

    /// <summary>
    /// Ensures that the required JavaScript module is loaded and ready for use.
    /// </summary>
    public async ValueTask EnsureModuleAsync() => await this.GetModuleAsync();

    public ValueTask InvokeVoidIfConnectedAsync(string identifier, params object?[]? args) => this._Module.InvokeVoidIfConnectedAsync(identifier, args);

    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        var module = await this.GetModuleAsync();
        await module.InvokeVoidAsync(identifier, args);
    }

    public async ValueTask<T> InvokeAsync<[DynamicallyAccessedMembers(PublicConstructors | PublicProperties | PublicFields)] T>(string identifier, params object?[]? args)
    {
        var module = await this.GetModuleAsync();
        return await module.InvokeAsync<T>(identifier, args);
    }

    public async ValueTask DisposeAsync()
    {
        await this._Module.DisposeIfConnectedAsync();
    }
}
