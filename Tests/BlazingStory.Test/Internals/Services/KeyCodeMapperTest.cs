using BlazingStory.Internals.Services;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Internals.Services;

internal class KeyCodeMapperTest
{
    [Test]
    public void GetKeyFromCode_Test()
    {
        KeyCodeMapper.GetKeyTextFromCode(Code.A).Is("A");
        KeyCodeMapper.GetKeyTextFromCode(Code.Comma).Is(",");
        KeyCodeMapper.GetKeyTextFromCode(Code.Slash).Is("/");
        KeyCodeMapper.GetKeyTextFromCode(Code.F12).Is("F12");
    }
}
