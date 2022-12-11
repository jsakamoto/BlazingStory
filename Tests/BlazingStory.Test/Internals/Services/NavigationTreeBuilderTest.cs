using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Types;
using BlazingStoryApp1.Stories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.Test.Internals.Services;

internal class NavigationTreeBuilderTest
{
    private static readonly RenderFragment<StoryContext> _EmptyFragment = ctx => ((RenderTreeBuilder _) => { });

    [Test]
    public void Build_from_EmptyBook_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[0];

        // When
        var root = builder.Build(storyContainers);

        // Then
        root.Type.Is(NavigationTreeItemType.Container);
        root.SubItems.Count.Is(0);
    }

    [Test]
    public void Build_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button_stories), "Examples/Button"){ Stories = {
                new("Default", new(), _EmptyFragment),
                new("Primary", new(), _EmptyFragment),
            }},
            new(typeof(Select_stories), "Examples/Select")
            {
                Stories = {
                new("Select", new(), _EmptyFragment),
            }}
        };

        // When
        var root = builder.Build(storyContainers);

        // Then
        root.Type.Is(NavigationTreeItemType.Container);
        root.SubItems.Count.Is(1);

        var examplesNode = root.SubItems[0];
        examplesNode.Type.Is(NavigationTreeItemType.Container);
        examplesNode.Caption.Is("Examples");
        examplesNode.SubItems.Count.Is(2);

        var buttonNode = examplesNode.SubItems[0];
        buttonNode.Type.Is(NavigationTreeItemType.StoryCollection);
        buttonNode.Caption.Is("Button");
        buttonNode.SubItems.Count.Is(2);

        var defaultButtonNode = buttonNode.SubItems[0];
        defaultButtonNode.Type.Is(NavigationTreeItemType.Story);
        defaultButtonNode.Caption.Is("Default");
        defaultButtonNode.SubItems.Count.Is(0);

        var primaryButtonNode = buttonNode.SubItems[1];
        primaryButtonNode.Type.Is(NavigationTreeItemType.Story);
        primaryButtonNode.Caption.Is("Primary");
        primaryButtonNode.SubItems.Count.Is(0);

        var selectNode = examplesNode.SubItems[1];
        selectNode.Type.Is(NavigationTreeItemType.StoryCollection);
        selectNode.Caption.Is("Select");
        selectNode.SubItems.Count.Is(1);

        var selectSelectNode = selectNode.SubItems[0];
        selectSelectNode.Type.Is(NavigationTreeItemType.Story);
        selectSelectNode.Caption.Is("Select");
        selectSelectNode.SubItems.Count.Is(0);
    }
}
