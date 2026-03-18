using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.ToolKit.JSInterop;

/// <summary>
/// A wrapper around a lazily-loaded JavaScript ES module that provides typed invocation helpers.
/// </summary>
public class JSModule : IAsyncDisposable
{
    private IJSObjectReference? _Module;

    private readonly Func<IJSRuntime> _GetJSRuntime;

    private readonly string _ModulePath;

    private readonly string _UpdateToken;

    /// <summary>
    /// Initializes a new instance of <see cref="JSModule"/>.
    /// </summary>
    /// <param name="jSRuntime">A factory that returns the <see cref="IJSRuntime"/> instance.</param>
    /// <param name="modulePath">The path to the JavaScript module file.</param>
    /// <param name="updateToken">A cache-busting token appended as a query string parameter when importing the module.</param>
    public JSModule(Func<IJSRuntime> jSRuntime, string modulePath, string updateToken)
    {
        this._GetJSRuntime = jSRuntime;
        this._ModulePath = modulePath;
        this._UpdateToken = updateToken;
    }

    private async ValueTask<IJSObjectReference> GetModuleAsync()
    {
        if (this._Module == null)
        {
            var jsRuntime = this._GetJSRuntime();
            this._Module = await jsRuntime.ImportAsync(this._ModulePath, this._UpdateToken);
        }
        return this._Module;
    }

    /// <summary>
    /// Ensures that the required JavaScript module is loaded and ready for use.
    /// </summary>
    public async ValueTask EnsureModuleAsync() => await this.GetModuleAsync();

    /// <summary>
    /// Invokes a void JavaScript function on the module without throwing if the JS runtime is disconnected.
    /// </summary>
    /// <param name="identifier">The JavaScript function name to invoke.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    public ValueTask InvokeVoidIfConnectedAsync(string identifier, params object?[]? args) => this._Module.InvokeVoidIfConnectedAsync(identifier, args);

    /// <summary>
    /// Invokes a void JavaScript function on the module, loading the module first if needed.
    /// </summary>
    /// <param name="identifier">The JavaScript function name to invoke.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        var module = await this.GetModuleAsync();
        await module.InvokeVoidAsync(identifier, args);
    }

    /// <summary>
    /// Invokes a JavaScript function on the module and returns the result, loading the module first if needed.
    /// </summary>
    /// <param name="identifier">The JavaScript function name to invoke.</param>
    /// <param name="args">JSON-serializable arguments.</param>
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
