using System.Collections;
using System.Collections.Specialized;
using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Utils;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandSet<TKey> : IDisposable, IEnumerable<(TKey Type, Command Command)>
    where TKey : struct, Enum
{
    private readonly string _StorageKey;

    private readonly HotKeysContext _HotKeysContext;

    private readonly HelperScript _HelperScript;

    private readonly ILogger _Logger;

    private readonly OrderedDictionary _Commands = new();

    private bool _Initialized = false;

    public Command? this[TKey type] => this._Commands[(object)type] as Command;

    internal CommandSet(string storageKey, HotKeysContext hotKeysContext, HelperScript helperScript, ILogger logger)
    {
        this._StorageKey = storageKey;
        this._HotKeysContext = hotKeysContext;
        this._HelperScript = helperScript;
        this._Logger = logger;
    }

    internal async ValueTask EnsureInitializedAsync(Func<IEnumerable<(TKey Type, Command Command)>> getCommandEntries)
    {
        if (this._Initialized) return;
        this._Initialized = true;

        var commandStates = await this._HelperScript.LoadObjectFromLocalStorageAsync(this._StorageKey, new Dictionary<TKey, CommandState>());
        foreach (var (type, command) in getCommandEntries())
        {
            if (commandStates.TryGetValue(type, out var state)) state.Apply(command);
            command.StateChanged += this.Command_StateChanged;
            this._Commands.Add(type, command);

            if (command.HotKey != null) this._HotKeysContext.Add(command.HotKey.Modifiers, command.HotKey.Code, command.InvokeAsync);
        }
    }

    public IDisposable Subscribe(TKey type, ValueTaskCallback callBack)
    {
        if (this[type] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    public IDisposable Subscribe(TKey type, ValueTaskCallback<Command> callBack)
    {
        if (this[type] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    private void Command_StateChanged(object? sender, EventArgs e)
    {
        var commandStates = this._Commands.Keys
            .Cast<TKey>()
            .ToDictionary(key => key, key => new CommandState(this[key]!));
        this._HelperScript
            .SaveObjectToLocalStorageAsync(this._StorageKey, commandStates)
            .AndLogException(this._Logger);
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public IEnumerator<(TKey Type, Command Command)> GetEnumerator()
    {
        return this._Commands.Keys.Cast<TKey>().Select(key => (key, this[key]!)).GetEnumerator();
    }

    public void Dispose()
    {
        foreach (var cmd in this._Commands.Values.Cast<Command>())
        {
            cmd.StateChanged -= this.Command_StateChanged;
        }
    }
}
