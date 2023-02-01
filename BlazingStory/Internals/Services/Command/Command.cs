using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Command;

public class Command
{
    public HotKeyCombo? HotKey { get => this._HotKey; set { if (this._HotKey == value) return; this._HotKey = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    public bool? Flag { get => this._Flag; set { if (this._Flag == value) return; this._Flag = value; this.StateChanged?.Invoke(this, EventArgs.Empty); } }

    internal readonly string? Title;

    private HotKeyCombo? _HotKey;

    private bool? _Flag;

    internal string? GetTitleText() => string.IsNullOrEmpty(this._HotKey?.Code.ToString()) ? this.Title : $"{this.Title} [{this.GetHotKeyName()}]";

    private readonly Dictionary<Guid, ValueTaskCallback<Command>> _Subscribers = new();

    internal event EventHandler? StateChanged;

    internal Command(string? title = null, bool? flag = null) : this(default, title, flag) { }

    public Command(HotKeyCombo? hotKey, string? title = null, bool? flag = null)
    {
        this._HotKey = hotKey;
        this.Title = title;
        this._Flag = flag;
    }

    internal string GetHotKeyName()
    {
        var hotKeyName = (string?)this.HotKey?.Code;
        return hotKeyName?.StartsWith("Key") == true ? hotKeyName[3..] : hotKeyName ?? "";
    }

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
}
