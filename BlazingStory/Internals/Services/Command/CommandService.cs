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

    private readonly Dictionary<CommandType, Command> _Commands = new();

    private string CommandStateKeyName => this.GetType().Name + "." + nameof(this._Commands);

    public CommandService(HotKeys hotKeys, HelperScript helperScript, ILogger<CommandService> logger)
    {
        this._HotKeys = hotKeys;
        this._HelperScript = helperScript;
        this._Logger = logger;
        this._HotKeysContext = this._HotKeys.CreateContext();
        this._HotKeys.KeyDown += this.HotKeys_OnKeyDown;
    }

    public async ValueTask AddCommandsAsync(params Command[] commands)
    {
        var commandStates = (await this._HelperScript.LoadObjectFromLocalStorageAsync(this.CommandStateKeyName, Array.Empty<CommandState>()))
            .ToDictionary(s => s.Type, s => s);
        foreach (var cmd in commands)
        {
            if (commandStates.TryGetValue(cmd.Type, out var state)) state.Apply(cmd);
            cmd.StateChanged += this.Command_StateChanged;
            this._Commands.Add(cmd.Type, cmd);
        }
    }

    public Command this[CommandType type] => this._Commands[type];

    public async Task InvokeAsync(CommandType type)
    {
        if (!this._Commands.TryGetValue(type, out var command)) return;
        await command.InvokeAsync();
    }

    public IDisposable Subscribe(CommandType type, AsyncCallback callBack)
    {
        if (!this._Commands.TryGetValue(type, out var command)) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    private void HotKeys_OnKeyDown(object? sender, HotKeyDownEventArgs args)
    {
        if (args.SrcElementTagName is "TEXTAREA" or "INPUT") return;
        var commad = this._Commands.Values.FirstOrDefault(cmd => cmd.HotKey == args.Code);
        if (commad == null) return;
        commad.InvokeAsync().AndLogException(this._Logger);
    }

    private void Command_StateChanged(object? sender, EventArgs e)
    {
        this._HelperScript
            .SaveObjectToLocalStorageAsync(this.CommandStateKeyName, this._Commands.Values.Select(cmd => new CommandState(cmd)))
            .AndLogException(this._Logger);
    }

    public void Dispose()
    {
        foreach (var cmd in this._Commands.Values)
        {
            cmd.StateChanged -= this.Command_StateChanged;
        }
        this._HotKeys.KeyDown -= this.HotKeys_OnKeyDown;
        this._HotKeysContext.Dispose();
    }
}
