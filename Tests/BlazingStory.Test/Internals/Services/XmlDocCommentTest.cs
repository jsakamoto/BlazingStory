using BlazingStory.Internals.Services;
using RazorClassLib1.Components.Button;

namespace BlazingStory.Test.Internals.Services;

internal class XmlDocCommentTest
{
    [Test]
    public void GetSummaryOfProperty_Test()
    {
        var summary = XmlDocComment.GetSummaryOfProperty(typeof(Button), nameof(Button.Text));
        summary.Is("Set a text that is button caption.");
    }
}
