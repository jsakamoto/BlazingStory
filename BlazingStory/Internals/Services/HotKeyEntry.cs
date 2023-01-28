using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services;

public class HotKeyEntry
{
    public ModCode Modifiers;

    public Code Code;

    public HotKeyEntry(Code code)
    {
        this.Modifiers = ModCode.None;
        this.Code = code;
    }

    public HotKeyEntry(ModCode modifiers, Code code)
    {
        this.Modifiers = modifiers;
        this.Code = code;
    }
}
