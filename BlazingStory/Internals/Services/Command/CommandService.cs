using BlazingStory.Internals.Utils;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandService : IAsyncDisposable
{
    internal readonly CommandSet<CommandType> Commands;
    private readonly HotKeysContext _HotKeysContext;
    private readonly ILogger<CommandService> _Logger;

    public CommandService(HotKeys hotKeys, HelperScript helperScript, ILogger<CommandService> logger)
    {
        this._Logger = logger;
        this._HotKeysContext = hotKeys.CreateContext();
        this.Commands = new CommandSet<CommandType>(this.CommandStateKeyName, this._HotKeysContext, helperScript, logger);
    }

    private string CommandStateKeyName => this.GetType().Name + "." + nameof(this.Commands);
    public Command? this[CommandType type] => this.Commands[type];

    public async Task InvokeAsync(CommandType type)
    {
        if (this.Commands[type] is not Command command)
        {
            return;
        }

        await command.InvokeAsync();
    }

    public IDisposable Subscribe(CommandType type, ValueTaskCallback callBack)
    {
        if (this.Commands[type] is not Command command)
        {
            throw new KeyNotFoundException();
        }

        return command.Subscribe(callBack);
    }

    public async ValueTask DisposeAsync()
    {
        this.Commands.Dispose();
        await this._HotKeysContext.DisposeAsync();
    }
}
