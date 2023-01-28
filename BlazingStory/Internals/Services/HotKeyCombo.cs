using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services;

public class HotKeyCombo
{
    public ModCode Modifiers;

    public Code Code;

    public HotKeyCombo(Code code)
    {
        this.Modifiers = ModCode.None;
        this.Code = code;
    }

    public HotKeyCombo(ModCode modifiers, Code code)
    {
        this.Modifiers = modifiers;
        this.Code = code;
    }
}
