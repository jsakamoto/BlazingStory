using Microsoft.JSInterop;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// Wraps bUnit's JSRuntime to support "GetValueAsync", which HotKeys2 uses on .NET 10 or later, but bUnit's JSRuntime doesn't implement.
/// </summary>
internal class BunitJSRuntimeWrapper : IJSRuntime
{
    private readonly IJSRuntime _JSRuntime;

    public BunitJSRuntimeWrapper(IJSRuntime jsRuntime)
    {
        this._JSRuntime = jsRuntime;
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => this._JSRuntime.InvokeAsync<TValue>(identifier, args);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => this._JSRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);

#if NET10_0_OR_GREATER
    public ValueTask<TValue> GetValueAsync<TValue>(string identifier) => ValueTask.FromResult(default(TValue)!);

    public ValueTask<TValue> GetValueAsync<TValue>(string identifier, CancellationToken cancellationToken) => ValueTask.FromResult(default(TValue)!);
#endif
}
