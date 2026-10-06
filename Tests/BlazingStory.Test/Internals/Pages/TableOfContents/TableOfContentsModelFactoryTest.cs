using BlazingStory.Internals.Pages.TableOfContents;

namespace BlazingStory.Test.Internals.Pages.TableOfContents;

internal class TableOfContentsModelFactoryTest
{
    [Test]
    public void Create_Should_Return_Empty_When_Headings_Are_Empty_Test()
    {
        TableOfContentsModelFactory.Create(Array.Empty<TableOfContentsSourceHeading>()).Count.Is(0);
    }

    [Test]
    public void Create_Should_Filter_By_Default_HeadingLevelRange_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("H1", 1),
            new TableOfContentsSourceHeading("H2", 2),
            new TableOfContentsSourceHeading("H3", 3),
            new TableOfContentsSourceHeading("H4", 4),
            new TableOfContentsSourceHeading("H5", 5),
        });

        items.SelectMany(EnumAll).Select(i => $"{i.Text}:{i.Level}")
            .Is("H1:1", "H2:2", "H3:3", "H4:4");
    }

    [Test]
    public void Create_Should_Ignore_Headings_Outside_Included_Range_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("Too Deep", 5),
            new TableOfContentsSourceHeading("Too Shallow", 0),
        });

        items.Count.Is(0);
    }

    [Test]
    public void Create_Should_Include_All_Headings_When_Range_Is_H1_To_H6_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("H1", 1),
            new TableOfContentsSourceHeading("H2", 2),
            new TableOfContentsSourceHeading("H3", 3),
            new TableOfContentsSourceHeading("H4", 4),
            new TableOfContentsSourceHeading("H5", 5),
            new TableOfContentsSourceHeading("H6", 6),
        }, 1, 6);

        items.SelectMany(EnumAll).Select(i => $"{i.Text}:{i.Level}")
            .Is("H1:1", "H2:2", "H3:3", "H4:4", "H5:5", "H6:6");
    }

    [Test]
    public void Create_Should_Apply_Custom_HeadingLevelRange_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("H1", 1),
            new TableOfContentsSourceHeading("H2", 2),
            new TableOfContentsSourceHeading("H3", 3),
            new TableOfContentsSourceHeading("H4", 4),
            new TableOfContentsSourceHeading("H5", 5),
        }, 3, 5);

        items.SelectMany(EnumAll).Select(i => $"{i.Text}:{i.Level}")
            .Is("H3:3", "H4:4", "H5:5");
    }

    [Test]
    public void Create_Should_Build_Hierarchy_From_HeadingLevels_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("A", 2),
            new TableOfContentsSourceHeading("A-1", 3),
            new TableOfContentsSourceHeading("A-1-x", 4),
            new TableOfContentsSourceHeading("A-2", 3),
            new TableOfContentsSourceHeading("B", 2),
        });

        items.Count.Is(2);
        items[0].Text.Is("A");
        items[0].Children.Select(c => c.Text).Is("A-1", "A-2");
        items[0].Children[0].Children.Select(c => c.Text).Is("A-1-x");
        items[1].Text.Is("B");
    }

    [Test]
    public void Create_Should_Generate_UniqueIds_When_HeadingText_Duplicates_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("Introduction", 2),
            new TableOfContentsSourceHeading("Introduction", 2),
            new TableOfContentsSourceHeading("introduction", 2),
        });

        items.Select(i => i.Id).Is("introduction", "introduction-2", "introduction-3");
    }

    [Test]
    public void Create_Should_Use_ProvidedId_And_Still_EnsureUniqueness_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("Section A", 2, "custom-id"),
            new TableOfContentsSourceHeading("Section B", 2, "custom-id"),
        });

        items.Select(i => i.Id).Is("custom-id", "custom-id-2");
    }

    [Test]
    public void Create_Should_Fall_Back_To_Text_When_ProvidedId_Is_Whitespace_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("Fallback Heading", 2, "   "),
        });

        items.Select(i => i.Id).Is("fallback-heading");
    }

    [Test]
    public void Create_Should_Fall_Back_To_Section_When_HeadingText_Is_Whitespace_Test()
    {
        var items = TableOfContentsModelFactory.Create(new[]
        {
            new TableOfContentsSourceHeading("   ", 2),
        });

        items.Select(i => i.Id).Is("section");
    }

    [Test]
    public void Slugify_Should_Normalize_Text_Test()
    {
        TableOfContentsModelFactory.Slugify("  Hello, World!  ").Is("hello-world");
        TableOfContentsModelFactory.Slugify("Café au lait").Is("cafe-au-lait");
        TableOfContentsModelFactory.Slugify("---").Is("section");
    }

    private static IEnumerable<TableOfContentsItem> EnumAll(TableOfContentsItem root)
    {
        yield return root;
        foreach (var item in root.Children.SelectMany(EnumAll))
        {
            yield return item;
        }
    }
}