using System.Collections.Specialized;
using BlazingStory.Internals.Extensions;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandService : IDisposable
{
    private readonly HotKeys _HotKeys;

    private readonly HelperScript _HelperScript;

    private readonly HotKeysContext _HotKeysContext;

    private readonly ILogger<CommandService> _Logger;

    private readonly OrderedDictionary _Commands = new();

    private string CommandStateKeyName => this.GetType().Name + "." + nameof(this._Commands);

    private bool _Initialized = false;

    public IEnumerable<(CommandType Type, Command Command)> Commands => this._Commands.Keys.Cast<CommandType>().Select(key => (key, this[key]!));

    public CommandService(HotKeys hotKeys, HelperScript helperScript, ILogger<CommandService> logger)
    {
        this._HotKeys = hotKeys;
        this._HelperScript = helperScript;
        this._Logger = logger;
        this._HotKeysContext = this._HotKeys.CreateContext();
        this._HotKeys.KeyDown += this.HotKeys_OnKeyDown;
    }

    public async ValueTask EnsureCommandsAsync(Func<IEnumerable<(CommandType Type, Command Command)>> getCommandEntries)
    {
        if (this._Initialized) return;
        this._Initialized = true;

        var commandStates = await this._HelperScript.LoadObjectFromLocalStorageAsync(this.CommandStateKeyName, new Dictionary<CommandType, CommandState>());
        foreach (var cmdEntry in getCommandEntries())
        {
            if (commandStates.TryGetValue(cmdEntry.Type, out var state)) state.Apply(cmdEntry.Command);
            cmdEntry.Command.StateChanged += this.Command_StateChanged;
            this._Commands.Add(cmdEntry.Type, cmdEntry.Command);
        }
    }

    public Command? this[CommandType type] => this._Commands[(object)type] as Command;

    public async Task InvokeAsync(CommandType type)
    {
        if (this._Commands[type] is not Command command) return;
        await command.InvokeAsync();
    }

    public IDisposable Subscribe(CommandType type, AsyncCallback callBack)
    {
        if (this._Commands[type] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    private void HotKeys_OnKeyDown(object? sender, HotKeyDownEventArgs args)
    {
        if (args.SrcElementTagName is "TEXTAREA" or "INPUT") return;
        var commad = this._Commands.Values.Cast<Command>()
            .FirstOrDefault(cmd => cmd.HotKey != null && cmd.HotKey.Code == args.Code && cmd.HotKey.Modifiers == args.Modifiers);
        if (commad == null) return;
        commad.InvokeAsync().AndLogException(this._Logger);
    }

    private void Command_StateChanged(object? sender, EventArgs e)
    {
        var commandStates = this._Commands.Keys
            .Cast<CommandType>()
            .ToDictionary(key => key, key => new CommandState(this[key]!));
        this._HelperScript
            .SaveObjectToLocalStorageAsync(this.CommandStateKeyName, commandStates)
            .AndLogException(this._Logger);
    }

    public void Dispose()
    {
        foreach (var cmd in this._Commands.Values.Cast<Command>())
        {
            cmd.StateChanged -= this.Command_StateChanged;
        }
        this._HotKeys.KeyDown -= this.HotKeys_OnKeyDown;
        this._HotKeysContext.Dispose();
    }
}
