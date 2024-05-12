using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Utils;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandService : IAsyncDisposable
{
    private readonly HotKeys _HotKeys;

    private readonly HotKeysContext _HotKeysContext;

    private readonly ILogger<CommandService> _Logger;

    private string CommandStateKeyName => this.GetType().Name + "." + nameof(this.Commands);

    internal readonly CommandSet<CommandType> Commands;

    public CommandService(HotKeys hotKeys, HelperScript helperScript, ILogger<CommandService> logger)
    {
        this.Commands = new CommandSet<CommandType>(this.CommandStateKeyName, helperScript, logger);
        this._HotKeys = hotKeys;
        this._Logger = logger;
        this._HotKeysContext = this._HotKeys.CreateContext();
        this._HotKeys.KeyDown += this.HotKeys_OnKeyDown;
    }

    public Command? this[CommandType type] => this.Commands[type];

    public async Task InvokeAsync(CommandType type)
    {
        if (this.Commands[type] is not Command command) return;
        await command.InvokeAsync();
    }

    public IDisposable Subscribe(CommandType type, ValueTaskCallback callBack)
    {
        if (this.Commands[type] is not Command command) throw new KeyNotFoundException();
        return command.Subscribe(callBack);
    }

    private void HotKeys_OnKeyDown(object? sender, HotKeyDownEventArgs args)
    {
        if (args.SrcElementTagName is "TEXTAREA" or "INPUT") return;
        var commad = this.Commands
            .Select(entry => entry.Command)
            .FirstOrDefault(cmd => cmd.HotKey != null && cmd.HotKey.Code == args.Code && cmd.HotKey.Modifiers == args.Modifiers);
        if (commad == null) return;

        // TODO: args.PreventDefault works only on Blazor WebAssembly.
        args.PreventDefault = true;

        commad.InvokeAsync().AndLogException(this._Logger);
    }

    public async ValueTask DisposeAsync()
    {
        this.Commands.Dispose();
        this._HotKeys.KeyDown -= this.HotKeys_OnKeyDown;
        await this._HotKeysContext.DisposeAsync();
    }
}
