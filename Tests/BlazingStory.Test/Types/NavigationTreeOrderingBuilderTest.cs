using static BlazingStory.Types.NavigationTreeOrderBuilder;
using static BlazingStory.Types.NavigationTreeOrderEntry;

namespace BlazingStory.Test.Types;

public class NavigationTreeOrderingBuilderTest
{
    [Test]
    public void Build_Test()
    {
        // When
        var ordering = N[
            "Components",
            N[
                "Layouts",
                N[
                    "Header",
                    "Footer"
                ],
                "Slider",
                "Button"
            ],
            "Templates",
            "Others"];

        // Then
        ordering.Length.Is(4);
        ordering[0].Type.Is(NodeType.Item);
        ordering[0].Title.Is("Components");

        ordering[1].Type.Is(NodeType.SubItems);
        ordering[1].SubItems.Count.Is(4);
        ordering[1].SubItems[0].Type.Is(NodeType.Item);
        ordering[1].SubItems[0].Title.Is("Layouts");

        ordering[1].SubItems[1].Type.Is(NodeType.SubItems);
        ordering[1].SubItems[1].SubItems.Count.Is(2);

        ordering[1].SubItems[1].SubItems[0].Type.Is(NodeType.Item);
        ordering[1].SubItems[1].SubItems[0].Title.Is("Header");

        ordering[1].SubItems[1].SubItems[1].Type.Is(NodeType.Item);
        ordering[1].SubItems[1].SubItems[1].Title.Is("Footer");

        ordering[1].SubItems[2].Type.Is(NodeType.Item);
        ordering[1].SubItems[2].Title.Is("Slider");

        ordering[1].SubItems[3].Type.Is(NodeType.Item);
        ordering[1].SubItems[3].Title.Is("Button");

        ordering[2].Type.Is(NodeType.Item);
        ordering[2].Title.Is("Templates");

        ordering[3].Type.Is(NodeType.Item);
        ordering[3].Title.Is("Others");
    }
}
