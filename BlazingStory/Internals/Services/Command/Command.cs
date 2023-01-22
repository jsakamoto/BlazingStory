using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;
public class Command
{
    internal readonly CommandType Type;

    internal Code HotKey;

    private string? _TitleHolder;

    private readonly Dictionary<Guid, AsyncCallback> _Subscribers = new();

    internal string? Title => this._TitleHolder == null ? null : string.Format(this._TitleHolder, this.GetHotKeyName());

    internal Command(CommandType type, string? title = null) : this(type, new Code(""), title) { }

    internal Command(CommandType type, Code hotKey, string? title = null)
    {
        this.Type = type;
        this.HotKey = hotKey;
        this._TitleHolder = title;
    }

    private string GetHotKeyName()
    {
        var hotKeyName = (string)this.HotKey;
        return hotKeyName.StartsWith("Key") ? hotKeyName.Substring(3) : hotKeyName;
    }

    internal IDisposable Subscribe(AsyncCallback callBack)
    {
        var key = Guid.NewGuid();
        this._Subscribers.Add(key, callBack);
        return new Disposer(() => { this._Subscribers.Remove(key); });
    }

    internal async ValueTask InvokeAsync()
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
