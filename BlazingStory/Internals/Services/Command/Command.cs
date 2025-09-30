using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Command;

public class Command
{
    public HotKeyCombo? HotKey { get => this._HotKey; set { if (this._HotKey == value) return; this._HotKey = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    public bool? Flag { get => this._Flag; set { if (this._Flag == value) return; this._Flag = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    internal readonly string? Title;
    
    internal readonly string? LinkUrl;

    private HotKeyCombo? _HotKey;

    private readonly HotKeyCombo? _DefaultHotKey;

    private bool? _Flag;

    private readonly Dictionary<Guid, ValueTaskCallback<Command>> _Subscribers = new();

    internal event EventHandler? StateChanged;

    internal Command(string? title = null, bool? flag = null) : this(default, title, flag) { }

    public Command(HotKeyCombo? hotKey, string? title = null, bool? flag = null, string? linkUrl = null)
    {
        this._HotKey = hotKey;
        this._DefaultHotKey = hotKey;
        this.Title = title;
        this.LinkUrl = linkUrl;
        this._Flag = flag;
    }

    /// <summary>Returns the key name of the hot key, like "⌃ ⇧ F1".</summary>
    internal string GetHotKeyName() => this._HotKey?.ToString() ?? string.Empty;

    /// <summary>Returns the title text of the command, like "Command [⌃ ⇧ F1]".</summary>
    internal string? GetTitleText() => string.IsNullOrEmpty(this.GetHotKeyName()) ? this.Title : $"{this.Title} [{this.GetHotKeyName()}]";

    internal IDisposable Subscribe(ValueTaskCallback callBack)
    {
        return this.Subscribe(async _ =>
        {
            await callBack();
            await StateHasChanged(callBack);
        });
    }

    internal IDisposable Subscribe(ValueTaskCallback<Command> callBack)
    {
        var key = Guid.NewGuid();
        this._Subscribers.Add(key, callBack);
        return new Disposer(() => { this._Subscribers.Remove(key); });
    }

    internal async ValueTask InvokeAsync()
    {
        var tasks = this._Subscribers.Select(s => this.InvokeCallbackAsync(s.Value)).ToArray();
        foreach (var task in tasks) { await task; }
    }

    internal void ToggleFlag()
    {
        this.Flag = !this.Flag;
    }

    private async ValueTask InvokeCallbackAsync(ValueTaskCallback<Command> callBack)
    {
        await callBack.Invoke(this);
        await StateHasChanged(callBack);
    }

    private static async ValueTask StateHasChanged(Delegate callBack)
    {
        if (callBack.Target is IHandleEvent handleEvent) { await handleEvent.HandleEventAsync(EventCallbackWorkItem.Empty, null); }
    }

    internal void ResetHotKey() => this.HotKey = this._DefaultHotKey;
}
