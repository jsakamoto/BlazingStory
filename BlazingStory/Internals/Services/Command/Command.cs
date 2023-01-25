using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

public class Command
{
    public readonly CommandType Type;

    public Code HotKey { get => this._HotKey; set { if (this._HotKey == value) return; this._HotKey = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    public bool? Flag { get => this._Flag; set { if (this._Flag == value) return; this._Flag = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    internal readonly string? Title;

    private Code _HotKey;

    private bool? _Flag;

    internal string? GetTitleText() => this._HotKey.ToString() == "" ? this.Title : $"{this.Title} [{this.GetHotKeyName()}]";

    private readonly Dictionary<Guid, AsyncCallback> _Subscribers = new();

    internal event EventHandler? StateChanged;

    internal Command(CommandType type, string? title = null, bool? flag = null) : this(type, new Code(""), title, flag) { }

    public Command(CommandType type, Code hotKey, string? title = null, bool? flag = null)
    {
        this.Type = type;
        this._HotKey = hotKey;
        this.Title = title;
        this._Flag = flag;
    }

    internal string GetHotKeyName()
    {
        var hotKeyName = (string?)this.HotKey;
        return hotKeyName?.StartsWith("Key") == true ? hotKeyName[3..] : hotKeyName ?? "";
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
        if (callback.Target is IHandleEvent handleEvent) { await handleEvent.HandleEventAsync(EventCallbackWorkItem.Empty, null); }
    }
}
