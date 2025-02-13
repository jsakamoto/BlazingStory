using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services;

/// <summary>
/// Represents a combination of a key and modifier keys.
/// </summary>
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

    /// <summary>
    /// Enumerates the key texts of this combination, like ["⌃", "⇧", "F1"].
    /// </summary>
    public IEnumerable<string> EnumKeyTexts()
    {
        if (this.Modifiers.HasFlag(ModCode.Alt)) yield return "alt";
        if (this.Modifiers.HasFlag(ModCode.Ctrl)) yield return "⌃";
        if (this.Modifiers.HasFlag(ModCode.Shift)) yield return "⇧";
        yield return KeyCodeMapper.GetKeyTextFromCode(this.Code);
    }

    /// <summary>
    /// Returns a string that represents the current object, like "alt ⌃ ⇧ F1".
    /// </summary>
    public override string ToString() => string.Join(' ', this.EnumKeyTexts());
}
