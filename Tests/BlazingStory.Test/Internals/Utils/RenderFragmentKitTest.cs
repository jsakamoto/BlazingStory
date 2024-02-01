using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

public class RenderFragmentKitTest
{
    [Test]
    public void ToString_For_RenderFragmentT_Test()
    {
        // Given
        RenderFragment<DateTime> renderFragment = (arg) => (builder) => builder.AddContent(0, "Dolor sit errata");

        // When
        var str = RenderFragmentKit.ToString(renderFragment);

        // Then
        str.Is("Dolor sit errata");
    }

    [Test]
    public void TryToString_For_RenderFragmentT_Test()
    {
        // Given
        RenderFragment<string> renderFragment = (arg) => (builder) => builder.AddContent(0, "Ipsum feudist est");

        // When
        var result = RenderFragmentKit.TryToString(renderFragment, out var str);

        // Then
        result.IsTrue();
        str.Is("Ipsum feudist est");
    }
}
