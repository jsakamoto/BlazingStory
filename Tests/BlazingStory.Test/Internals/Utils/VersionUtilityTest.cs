using BlazingStory.Internals.Utils;

namespace BlazingStory.Test.Internals.Utils;

internal class VersionUtilityTest
{
    [Test]
    public void GetFormattedVersionText_without_Sufix_Test()
    {
        var assembly = typeof(ClassLibVer1230.Class1).Assembly;
        assembly.GetFormattedVersionText().Is("1.2.3");
    }

    [Test]
    public void GetFormattedVersionText_with_Sufix_Test()
    {
        var assembly = typeof(ClassLibVer1200Prev34.Class1).Assembly;
        assembly.GetFormattedVersionText().Is("1.2 Preview 3.4");
    }
}
