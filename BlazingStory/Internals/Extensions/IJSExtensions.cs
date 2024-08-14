using BlazingStory.Internals.Utils;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Extensions;

internal static class IJSExtensions
{
    private const string _DefaultBasePath = "./_content/BlazingStory/";

    /// <summary>
    /// Import a JavScript module from the specified path.
    /// </summary>
    /// <param name="jsRuntime">
    /// The <see cref="IJSRuntime" /> instance.
    /// </param>
    /// <param name="modulePath">
    /// The path to the JavaScript module to import, like "./Component.razor.js".
    /// </param>
    /// <returns>
    /// </returns>
    public static async ValueTask<IJSObjectReference> ImportAsync(this IJSRuntime jsRuntime, string modulePath)
    {
        var updateToken = UriParameterKit.GetUpdateToken(jsRuntime);

        return await jsRuntime.InvokeAsync<IJSObjectReference>("import", _DefaultBasePath + modulePath + updateToken);
    }

    /// <summary>
    /// Invoke a JavaScript function with the specified identifier. <br /> This method will not
    /// throw an exception if the <see cref="IJSObjectReference" /> is disconnected.
    /// </summary>
    /// <param name="value">
    /// The <see cref="IJSObjectReference" /> instance.
    /// </param>
    /// <param name="identifier">
    /// An identifier for the function to invoke.
    /// </param>
    /// <param name="args">
    /// JSON-serializable arguments.
    /// </param>
    public static async ValueTask InvokeVoidIfConnectedAsync(this IJSObjectReference? value, string identifier, params object?[]? args)
    {
        try
        {
            if (value != null)
            {
                await value.InvokeVoidAsync(identifier, args);
            }
        }
        catch (JSDisconnectedException)
        {
        }
    }

    /// <summary>
    /// Dispose this <see cref="IJSObjectReference" /> object. <br /> If <paramref
    /// name="methodToCallBeforeDispose" /> is not null, it will be invoked before disposing the
    /// object. <br /> This method will not throw an exception if the <see cref="IJSObjectReference"
    /// /> is disconnected.
    /// </summary>
    /// <param name="value">
    /// The <see cref="IJSObjectReference" /> instance.
    /// </param>
    /// <param name="methodToCallBeforeDispose">
    /// If specified, this method will be invoked before disposing the object.
    /// </param>
    public static async ValueTask DisposeIfConnectedAsync(this IJSObjectReference? value, string? methodToCallBeforeDispose = null)
    {
        if (value == null)
        {
            return;
        }

        try
        {
            if (methodToCallBeforeDispose != null)
            {
                await value.InvokeVoidAsync(methodToCallBeforeDispose);
            }

            await value.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }
}
