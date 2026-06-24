using BlazingStory.Addons.BuiltIns.Panel.Accessibility;
using BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;
using BlazingStory.ToolKit.Icons;
using Bunit;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Accessibility;

public class AxeResultViewTest
{
    [TestCase(false, "false")]
    [TestCase(true, "true")]
    public void AxeResultView_Render_HeaderIsButtonWithAriaExpanded_Test(bool expanded, string expectedAriaExpanded)
    {
        // Given
        using var ctx = new BunitContext();
        ctx.ComponentFactories.AddStub<AxeNodesView>();
        ctx.ComponentFactories.AddStub<SvgIcon>();
        var result = new Result { Id = "select-name", Impact = "critical" };

        // When
        var cut = ctx.Render<AxeResultView>(parameters => parameters
            .Add(p => p.Result, result)
            .Add(p => p.Expanded, expanded));

        // Then
        var header = cut.Find(".header");
        header.TagName.Is("BUTTON");
        header.GetAttribute("type").Is("button");
        header.GetAttribute("aria-expanded").Is(expectedAriaExpanded);
    }

    [Test]
    public async Task AxeResultView_ClickHeader_InvokesOnClickHeader_Test()
    {
        // Given
        using var ctx = new BunitContext();
        ctx.ComponentFactories.AddStub<AxeNodesView>();
        ctx.ComponentFactories.AddStub<SvgIcon>();
        var clickCount = 0;
        var cut = ctx.Render<AxeResultView>(parameters => parameters
            .Add(p => p.Result, new Result { Id = "select-name" })
            .Add(p => p.OnClickHeader, () => clickCount++));

        // When
        await cut.Find(".header").ClickAsync(new());

        // Then
        clickCount.Is(1);
    }
}
