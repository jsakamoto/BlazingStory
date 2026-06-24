using BlazingStory.Configurations;
using BlazingStory.Types;

namespace BlazingStory.Test.Configurations;

internal class BlazingStoryOptionsTableOfContentsTest
{
    [Test]
    public void CustomPageTableOfContentsPlacement_Default_Should_Be_None_Test()
    {
        var options = new BlazingStoryOptions();

        options.CustomPageTableOfContentsPlacement.Is((TableOfContentsPlacement?)TableOfContentsPlacement.None);
    }

    [Test]
    public void CustomPageTableOfContentsPlacement_Should_Accept_Null_Test()
    {
        var options = new BlazingStoryOptions
        {
            CustomPageTableOfContentsPlacement = null,
        };

        options.CustomPageTableOfContentsPlacement.IsNull();
    }

    [Test]
    public void CustomPageAttribute_TableOfContentsPlacement_Default_Should_Be_Inherit_Test()
    {
        var attribute = new CustomPageAttribute("Test/Page");

        attribute.TableOfContentsPlacement.Is(TableOfContentsPlacement.Inherit);
    }

    [Test]
    public void CustomPageAttribute_TableOfContentsPlacement_Should_Accept_Explicit_None_Test()
    {
        var attribute = new CustomPageAttribute("Test/Page")
        {
            TableOfContentsPlacement = TableOfContentsPlacement.None,
        };

        attribute.TableOfContentsPlacement.Is(TableOfContentsPlacement.None);
    }

    [Test]
    public void CustomPageTableOfContentsHeadingLevelRange_Default_Should_Be_H1_To_H4_Test()
    {
        var options = new BlazingStoryOptions();

        options.CustomPageTableOfContentsMinHeadingLevel.Is(1);
        options.CustomPageTableOfContentsMaxHeadingLevel.Is(4);
    }

    [Test]
    public void CustomPageAttribute_TableOfContentsHeadingLevelRange_Default_Should_Be_Null_Test()
    {
        var attribute = new CustomPageAttribute("Test/Page");

        attribute.TableOfContentsMinHeadingLevel.IsNull();
        attribute.TableOfContentsMaxHeadingLevel.IsNull();
    }

    [Test]
    public void CustomPageAttribute_TableOfContentsHeadingLevelRange_Should_Accept_Explicit_Values_Test()
    {
        var attribute = new CustomPageAttribute("Test/Page")
        {
            TableOfContentsMinHeadingLevel = 2,
            TableOfContentsMaxHeadingLevel = 6,
        };

        attribute.TableOfContentsMinHeadingLevel.Is(2);
        attribute.TableOfContentsMaxHeadingLevel.Is(6);
    }
}