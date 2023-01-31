using System.Collections;
using System.Collections.Specialized;
using BlazingStory.Internals.Extensions;
using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Services.Command;

internal class CommandSet<TKey> : IDisposable, IEnumerable<(TKey Type, Command Command)>
    where TKey : struct, Enum
{
    private readonly string _StorageKey;

    private readonly HelperScript _HelperScript;

    private readonly ILogger _Logger;

    private readonly OrderedDictionary _Commands = new();

    private bool _Initialized = false;

    public Command? this[TKey type] => this._Commands[(object)type] as Command;

    internal CommandSet(string storageKey, HelperScript helperScript, ILogger logger)
    {
        this._StorageKey = storageKey;
        this._HelperScript = helperScript;
        this._Logger = logger;
    }

    internal async ValueTask EnsureInitializedAsync(Func<IEnumerable<(TKey Type, Command Command)>> getCommandEntries)
    {
        if (this._Initialized) return;
        this._Initialized = true;

        var commandStates = await this._HelperScript.LoadObjectFromLocalStorageAsync(this._StorageKey, new Dictionary<TKey, CommandState>());
        foreach (var cmdEntry in getCommandEntries())
        {
            if (commandStates.TryGetValue(cmdEntry.Type, out var state)) state.Apply(cmdEntry.Command);
            cmdEntry.Command.StateChanged += this.Command_StateChanged;
            this._Commands.Add(cmdEntry.Type, cmdEntry.Command);
        }
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
