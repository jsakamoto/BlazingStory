using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Extensions;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Services;

internal class JSModule : IAsyncDisposable
{
    #region Private Fields

    private readonly Func<IJSRuntime?> _GetJSRuntime;
    private readonly string _ModulePath;
    private IJSObjectReference? _Module;

    #endregion Private Fields

    #region Internal Constructors

    internal JSModule(Func<IJSRuntime?> jSRuntime, string modulePath)
    {
        this._GetJSRuntime = jSRuntime;
        this._ModulePath = modulePath;
    }

    #endregion Internal Constructors

    #region Public Methods

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

    #endregion Public Methods

    #region Private Methods

    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        if (this._Module == null)
        {
            var jsRuntime = this._GetJSRuntime();

            if (jsRuntime == null)
            {
                throw new InvalidOperationException("The JSRuntime is not available.");
            }

            this._Module = await jsRuntime.ImportAsync(this._ModulePath);
        }

        return this._Module;
    }

    #endregion Private Methods
}
