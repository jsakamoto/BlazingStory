using BlazingStory.Internals.Extensions;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandService : IDisposable
{
    private readonly HotKeys _HotKeys;

    private readonly HotKeysContext _HotKeysContext;

    private readonly ILogger<CommandService> _Logger;

    private readonly Dictionary<CommandType, Command> _Commands = new();

    public CommandService(HotKeys hotKeys, ILogger<CommandService> logger)
    {
        this._HotKeys = hotKeys;
        this._Logger = logger;
        this._HotKeysContext = this._HotKeys.CreateContext();
        this._HotKeys.KeyDown += this.HotKeys_OnKeyDown;
    }

    public void AddCommands(params Command[] commands)
    {
        foreach (var cmd in commands)
        {
            this._Commands.Add(cmd.Type, cmd);
        }
    }

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
        var commad = this._Commands.FirstOrDefault(item => item.Value.HotKey == args.Code).Value;
        if (commad == null) return;
        commad.InvokeAsync().AndLogException(this._Logger);
    }

    public void Dispose()
    {
        this._HotKeys.KeyDown -= this.HotKeys_OnKeyDown;
        this._HotKeysContext.Dispose();
    }
}
