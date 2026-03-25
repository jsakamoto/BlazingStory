using BlazingStory.Internals.Services;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Internals.Services.Command;

public class CommandTest
{
    [Test]
    public void GetTitleText_with_Hotkey_Test()
    {
        var command = new BlazingStory.Internals.Services.Command.Command(new HotKeyCombo(ModCode.Ctrl, Code.Num0), "Lorem ipsum");
        command.GetTitleText().Is("Lorem ipsum [⌃ 0]");
    }

    [Test]
    public void GetTitleText_without_Hotkey_Test()
    {
        var command = new BlazingStory.Internals.Services.Command.Command("Diam sadipscing");
        command.GetTitleText().Is("Diam sadipscing");
    }
}
