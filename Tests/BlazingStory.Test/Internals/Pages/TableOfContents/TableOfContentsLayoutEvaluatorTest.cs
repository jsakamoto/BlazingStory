using BlazingStory.Internals.Pages.TableOfContents;
using BlazingStory.Types;

namespace BlazingStory.Test.Internals.Pages.TableOfContents;

internal class TableOfContentsLayoutEvaluatorTest
{
    [Test]
    public void Evaluate_Should_Disable_All_When_NotMarkdownPage_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.LeftSidebar, isMarkdownCustomPage: false, hasItems: true);

        state.RenderTop.IsFalse();
        state.RenderLeftSidebar.IsFalse();
        state.RenderRightSidebar.IsFalse();
        state.EnableScrollSpy.IsFalse();
    }

    [Test]
    public void Evaluate_Should_Disable_All_When_NoItems_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.RightSidebar, isMarkdownCustomPage: true, hasItems: false);

        state.RenderTop.IsFalse();
        state.RenderLeftSidebar.IsFalse();
        state.RenderRightSidebar.IsFalse();
        state.EnableScrollSpy.IsFalse();
    }

    [Test]
    public void Evaluate_TopPlacement_Should_RenderTop_And_DisableScrollSpy_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.Top, isMarkdownCustomPage: true, hasItems: true);

        state.RenderTop.IsTrue();
        state.RenderLeftSidebar.IsFalse();
        state.RenderRightSidebar.IsFalse();
        state.EnableScrollSpy.IsFalse();
    }

    [Test]
    public void Evaluate_LeftSidebar_Should_RenderLeft_And_EnableScrollSpy_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.LeftSidebar, isMarkdownCustomPage: true, hasItems: true);

        state.RenderTop.IsFalse();
        state.RenderLeftSidebar.IsTrue();
        state.RenderRightSidebar.IsFalse();
        state.EnableScrollSpy.IsTrue();
    }

    [Test]
    public void Evaluate_RightSidebar_Should_RenderRight_And_EnableScrollSpy_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.RightSidebar, isMarkdownCustomPage: true, hasItems: true);

        state.RenderTop.IsFalse();
        state.RenderLeftSidebar.IsFalse();
        state.RenderRightSidebar.IsTrue();
        state.EnableScrollSpy.IsTrue();
    }

    [Test]
    public void Evaluate_NonePlacement_Should_Disable_All_Test()
    {
        var state = TableOfContentsLayoutEvaluator.Evaluate(TableOfContentsPlacement.None, isMarkdownCustomPage: true, hasItems: true);

        state.RenderTop.IsFalse();
        state.RenderLeftSidebar.IsFalse();
        state.RenderRightSidebar.IsFalse();
        state.EnableScrollSpy.IsFalse();
    }
}