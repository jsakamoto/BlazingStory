using BlazingStory.Internals.Utils;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandService : IAsyncDisposable
{
    private readonly ILogger<CommandService> _Logger;

    private string CommandStateKeyName => this.GetType().Name + "." + nameof(this.Commands);

    internal readonly CommandSet<CommandType> Commands;

    public CommandService(HotKeys hotKeys, HelperScript helperScript, ILogger<CommandService> logger)
    {
        this._Logger = logger;
        this.Commands = new CommandSet<CommandType>(this.CommandStateKeyName, hotKeys, helperScript, logger);
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

    public async ValueTask DisposeAsync()
    {
        await this.Commands.DisposeAsync();
    }
}
