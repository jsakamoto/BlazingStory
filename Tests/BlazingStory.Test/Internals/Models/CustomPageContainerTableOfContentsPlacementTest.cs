using BlazingStory.Internals.Models;
using BlazingStory.Types;

namespace BlazingStory.Test.Internals.Models;

internal class CustomPageContainerTableOfContentsPlacementTest
{
    private sealed class NoOverridePage { }

    private sealed class TopOverridePage { }

    [Test]
    public void Constructor_Should_Throw_When_Descriptor_Is_Null_Test()
    {
        Action action = () => new CustomPageContainer(null!);
        Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void Constructor_Should_Set_Title_And_NavigationPath_From_Attribute_Test()
    {
        var attribute = new CustomPageAttribute("Test/Title Path");
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.Title.Is("Test/Title Path");
        container.NavigationPath.Is("test-title-path");
    }

    [Test]
    public void Constructor_Should_Throw_When_Title_Is_Null_Test()
    {
        var attribute = new CustomPageAttribute(null!);
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);

        Action action = () => new CustomPageContainer(descriptor);
        Assert.Throws<ArgumentNullException>(action);
    }

    [Test]
    public void ResolveTableOfContentsPlacement_Should_Use_GlobalDefault_When_PageOverride_Missing_Test()
    {
        var attribute = new CustomPageAttribute("Test/No Override");
        attribute.TableOfContentsPlacement.Is(TableOfContentsPlacement.Inherit);
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsPlacement(TableOfContentsPlacement.RightSidebar).Is(TableOfContentsPlacement.RightSidebar);
    }

    [Test]
    public void ResolveTableOfContentsPlacement_Should_Prioritize_PageOverride_Over_GlobalDefault_Test()
    {
        var attribute = new CustomPageAttribute("Test/Top Override")
        {
            TableOfContentsPlacement = TableOfContentsPlacement.Top,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(TopOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsPlacement(TableOfContentsPlacement.LeftSidebar).Is(TableOfContentsPlacement.Top);
    }

    [Test]
    public void ResolveTableOfContentsPlacement_Should_Use_None_When_GlobalDefault_Is_Null_Test()
    {
        var attribute = new CustomPageAttribute("Test/No Override")
        {
            TableOfContentsPlacement = TableOfContentsPlacement.Inherit,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsPlacement(null).Is(TableOfContentsPlacement.None);
    }

    [Test]
    public void ResolveTableOfContentsPlacement_Should_Normalize_Invalid_GlobalDefault_To_None_Test()
    {
        var attribute = new CustomPageAttribute("Test/No Override")
        {
            TableOfContentsPlacement = TableOfContentsPlacement.Inherit,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsPlacement((TableOfContentsPlacement)999).Is(TableOfContentsPlacement.None);
    }

    [Test]
    public void ResolveTableOfContentsPlacement_Should_Normalize_Invalid_PageOverride_To_None_Test()
    {
        var attribute = new CustomPageAttribute("Test/Invalid Override")
        {
            TableOfContentsPlacement = (TableOfContentsPlacement)999,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(TopOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsPlacement(TableOfContentsPlacement.RightSidebar).Is(TableOfContentsPlacement.None);
    }

    [Test]
    public void ResolveTableOfContentsHeadingLevelRange_Should_Use_GlobalDefaults_When_PageOverrides_Missing_Test()
    {
        var attribute = new CustomPageAttribute("Test/No Range Override");
        var descriptor = new CustomPageRazorDescriptor(typeof(NoOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsMinHeadingLevel(1).Is(1);
        container.ResolveTableOfContentsMaxHeadingLevel(4).Is(4);
    }

    [Test]
    public void ResolveTableOfContentsHeadingLevelRange_Should_Prioritize_PageOverrides_Over_GlobalDefaults_Test()
    {
        var attribute = new CustomPageAttribute("Test/Range Override")
        {
            TableOfContentsMinHeadingLevel = 2,
            TableOfContentsMaxHeadingLevel = 6,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(TopOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsMinHeadingLevel(1).Is(2);
        container.ResolveTableOfContentsMaxHeadingLevel(4).Is(6);
    }

    [Test]
    public void ResolveTableOfContentsHeadingLevelRange_Should_Normalize_Values_To_H1_To_H6_Test()
    {
        var attribute = new CustomPageAttribute("Test/Range Normalize")
        {
            TableOfContentsMinHeadingLevel = -2,
            TableOfContentsMaxHeadingLevel = 99,
        };
        var descriptor = new CustomPageRazorDescriptor(typeof(TopOverridePage), attribute);
        var container = new CustomPageContainer(descriptor);

        container.ResolveTableOfContentsMinHeadingLevel(1).Is(1);
        container.ResolveTableOfContentsMaxHeadingLevel(4).Is(6);
    }
}