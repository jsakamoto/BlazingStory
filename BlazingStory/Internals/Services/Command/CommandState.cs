using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services.Command;

internal class CommandState
{
    public ModCode KeyMod;
    public string? KeyCode;
    public bool? Flag;

    public CommandState()
    {
    }

    public CommandState(Command command)
    {
        this.KeyMod = command.HotKey?.Modifiers ?? ModCode.None;
        this.KeyCode = command.HotKey?.Code;
        this.Flag = command.Flag;
    }

    internal void Apply(Command command)
    {
        command.HotKey = new HotKeyCombo(this.KeyMod, new(this.KeyCode ?? ""));
        command.Flag = this.Flag;
    }
}
