using BlazingStory.Internals.Services;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Internals.Services;

public class HotKeyComboTest
{
    [Test]
    public void EnumKeyTexts_Test()
    {
        new HotKeyCombo(Code.A).EnumKeyTexts().Is("A");
        new HotKeyCombo(ModCode.Ctrl, Code.Num0).EnumKeyTexts().Is("⌃", "0");
        new HotKeyCombo(ModCode.Alt, Code.Equal).EnumKeyTexts().Is("alt", "=");
        new HotKeyCombo(ModCode.Shift, Code.Enter).EnumKeyTexts().Is("⇧", "Enter");
        new HotKeyCombo(ModCode.Alt | ModCode.Ctrl | ModCode.Shift, Code.F1)
            .EnumKeyTexts()
            .Is("alt", "⌃", "⇧", "F1");
    }

    [Test]
    public void ToString_Test()
    {
        new HotKeyCombo(Code.A).ToString().Is("A");
        new HotKeyCombo(ModCode.Ctrl, Code.Num0).ToString().Is("⌃ 0");
        new HotKeyCombo(ModCode.Alt, Code.Equal).ToString().Is("alt =");
        new HotKeyCombo(ModCode.Shift, Code.Enter).ToString().Is("⇧ Enter");
        new HotKeyCombo(ModCode.Alt | ModCode.Ctrl | ModCode.Shift, Code.F1)
            .ToString()
            .Is("alt ⌃ ⇧ F1");
    }
}
