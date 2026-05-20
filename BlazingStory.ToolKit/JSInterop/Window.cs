using System.ComponentModel;
using BlazingStory.ToolKit.Internals.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BlazingStory.ToolKit.JSInterop;

/// <summary>
/// Provides interop with the browser window object, supporting message posting and subscription.
/// </summary>
public sealed class Window : IAsyncDisposable
{
    private readonly JSModule _JSModule;

    private readonly DotNetObjectReference<Window> _DotNetRef;

    private bool _Disposed;

    private ILogger<Window> _Logger;

    private IJSObjectReference? _MessageListener;

    /// <summary>Initializes a new instance of <see cref="Window"/>.</summary>
    /// <param name="jSRuntime">The JS runtime used to invoke JavaScript functions.</param>
    /// <param name="logger">The logger for recording errors.</param>
    public Window(IJSRuntime jSRuntime, ILogger<Window> logger)
    {
        this._Logger = logger;
        this._DotNetRef = DotNetObjectReference.Create(this);
        this._JSModule = JSModuleFactory.Create(() => jSRuntime, "js/window.js");
    }

    private class MessageSubscripton : IDisposable
    {
        private readonly Window _Owner;

        public readonly Func<string?, Task> Action;

        public MessageSubscripton(Window owner, Func<string?, Task> action)
        {
            this._Owner = owner;
            this.Action = action;
        }

        public void Dispose()
        {
            if (this._Owner._Disposed) return;
            lock (this._Owner._MessageSubscriptions) this._Owner._MessageSubscriptions.Remove(this);
        }
    }

    private readonly List<MessageSubscripton> _MessageSubscriptions = [];

    /// <summary>Posts a message to the window element identified by the given CSS selector.</summary>
    /// <param name="selector">"parent" to target a parent window, or CSS selector to target a specific iframe element.</param>
    /// <param name="message">The message content to post.</param>
    public async ValueTask PostMessageAsync(string selector, string? message)
    {
        await this._JSModule.InvokeVoidAsync("postMessage", selector, message);
    }

    /// <summary>Subscribes to window messages and invokes the callback when a message is received.</summary>
    /// <param name="callback">The action to invoke when a message arrives.</param>
    public async ValueTask<IDisposable> SubscribeMessageAsync(Func<string?, Task> callback)
    {
        await this.EnsureMessageListenerAsync();
        lock (this._MessageSubscriptions)
        {
            var subscription = new MessageSubscripton(this, callback);
            this._MessageSubscriptions.Add(subscription);
            return subscription;
        }
    }

    private async ValueTask EnsureMessageListenerAsync()
    {
        if (this._MessageListener is not null) return;
        this._MessageListener = await this._JSModule.InvokeAsync<IJSObjectReference>("setupMessageListener", this._DotNetRef);
    }

    /// <summary>Called from JavaScript when a window message is received; dispatches it to all subscribers.</summary>
    /// <param name="message">The received message content.</param>
    [JSInvokable(nameof(OnMessageReceived)), EditorBrowsable(EditorBrowsableState.Never)]
    public async Task OnMessageReceived(string? message)
    {
        static IEnumerable<Task> invokeAllAction(IEnumerable<MessageSubscripton> subscriptions, string? message)
        {
            lock (subscriptions) return subscriptions.Select(s => s.Action(message)).ToArray();
        }

        foreach (var task in invokeAllAction(this._MessageSubscriptions, message))
        {
            try { await task; }
            catch (Exception ex) { this._Logger.LogError(ex, "An error occurred while processing a window message."); }
        }
    }

    /// <summary>Releases all resources and unregisters the JavaScript message listener.</summary>
    public async ValueTask DisposeAsync()
    {
        this._Disposed = true;
        lock (this._MessageSubscriptions) this._MessageSubscriptions.Clear();
        await this._MessageListener.DisposeIfConnectedAsync("dispose");
        await this._JSModule.DisposeAsync();
        this._DotNetRef.Dispose();
    }
}
