using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandState
{
    public CommandType Type;
    public string? HotKey;
    public bool Flag;

    public CommandState()
    {
    }

    public CommandState(Command command)
    {
        this.Type = command.Type;
        this.HotKey = command.HotKey;
        this.Flag = command.Flag;
    }

    internal void Apply(Command command)
    {
        command.HotKey = new Code(this.HotKey ?? "");
        command.Flag = this.Flag;
    }
}
