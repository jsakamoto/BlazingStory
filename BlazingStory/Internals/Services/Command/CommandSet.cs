using System.Collections;
using System.Collections.Specialized;
using BlazingStory.Internals.Utils;
using BlazingStory.ToolKit.Extensions;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandSet : IAsyncDisposable, IEnumerable<(string Key, Command Command)>
{
    private readonly string _StorageKey;

    private readonly HotKeys _HotKeys;

    private readonly IJSRuntime _JSRuntime;

    private readonly ILogger _Logger;

    private readonly OrderedDictionary _Commands = new();

    private bool _Initialized = false;

    private HotKeysContext? _HotKeysContext;

    public Command? this[string key] => this._Commands[key] as Command;

    internal CommandSet(string storageKey, HotKeys hotKeys, IJSRuntime jsRuntime, ILogger logger)
    {
        this._StorageKey = storageKey;
        this._HotKeys = hotKeys;
        this._JSRuntime = jsRuntime;
        this._Logger = logger;
    }

    internal async ValueTask EnsureInitializedAsync(Func<IEnumerable<(string Key, Command Command)>> getCommandEntries)
    {
        if (this._Initialized) return;
        this._Initialized = true;

        var commandStates = await this._JSRuntime.LoadObjectFromLocalStorageAsync(this._StorageKey, new Dictionary<string, CommandState>());
        foreach (var (key, command) in getCommandEntries())
        {
            if (commandStates.TryGetValue(key, out var state)) state.Apply(command);
            command.StateChanged += this.Command_StateChanged;
            this._Commands.Add(key, command);
        }

        await this.ConfigureHotKeys();
    }

    public IDisposable Subscribe(string key, ValueTaskCallback callBack)
    {
        if (this[key] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    public IDisposable Subscribe(string key, ValueTaskCallback<Command> callBack)
    {
        if (this[key] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    private void Command_StateChanged(object? sender, EventArgs e)
    {
        var commandStates = this._Commands.Keys
            .Cast<string>()
            .ToDictionary(key => key, key => new CommandState(this[key]!));
        this._JSRuntime
            .SaveObjectToLocalStorageAsync(this._StorageKey, commandStates)
            .AndLogException(this._Logger);
        this.ConfigureHotKeys()
            .AndLogException(this._Logger);
    }

    private async ValueTask ConfigureHotKeys()
    {
        var previousHotKeysContext = this._HotKeysContext;
        this._HotKeysContext = this._HotKeys.CreateContext();
        foreach (var (_, command) in this)
        {
            if (command.HotKey != null) this._HotKeysContext.Add(command.HotKey.Modifiers, command.HotKey.Code, command.InvokeAsync);
        }
        if (previousHotKeysContext is not null) await previousHotKeysContext.DisposeAsync();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public IEnumerator<(string Key, Command Command)> GetEnumerator()
    {
        return this._Commands.Keys.Cast<string>().Select(key => (key, this[key]!)).GetEnumerator();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var cmd in this._Commands.Values.Cast<Command>())
        {
            cmd.StateChanged -= this.Command_StateChanged;
        }
        if (this._HotKeysContext is not null) await this._HotKeysContext.DisposeAsync();
    }
}
