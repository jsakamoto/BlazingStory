using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;
internal class Command
{
    public readonly CommandType Type;

    public Code HotKey;

    private readonly Dictionary<Guid, AsyncCallback> _Subscribers = new();

    public Command(CommandType type) : this(type, new Code("")) { }

    public Command(CommandType type, Code hotKey)
    {
        this.Type = type;
        this.HotKey = hotKey;
    }

    public IDisposable Subscribe(AsyncCallback callBack)
    {
        var key = Guid.NewGuid();
        this._Subscribers.Add(key, callBack);
        return new Disposer(() => { this._Subscribers.Remove(key); });
    }

    public async ValueTask InvokeAsync()
    {
        var tasks = this._Subscribers.Select(s => InvokeCallbackAsync(s.Value)).ToArray();
        foreach (var task in tasks) { await task; }
    }

    private static async ValueTask InvokeCallbackAsync(AsyncCallback callback)
    {
        await callback.Invoke();
        var handleEvent = callback.Target as IHandleEvent;
        if (handleEvent != null) { await handleEvent.HandleEventAsync(EventCallbackWorkItem.Empty, null); }
    }
}
