using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

public class TypeUtilityTest
{
    [Test]
    public void TryConvertType_RenderFragment_Test()
    {
        TypeUtility.TryConvertType(typeof(RenderFragment), "Hello, World.", out var result).IsTrue();

        var renderFragment = result.IsInstanceOf<RenderFragment>();

        using var ctx = new Bunit.TestContext();
        using var cut = ctx.Render(renderFragment);
        cut.Markup.Is("Hello, World.");
    }
}
