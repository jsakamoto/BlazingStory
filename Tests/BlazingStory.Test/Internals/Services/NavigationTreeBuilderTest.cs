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
        var root = builder.Build(storyContainers, null);

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
                new("Examples/Button", "Default", new(), _EmptyFragment),
                new("Examples/Button", "Primary", new(), _EmptyFragment),
            }},
            new(typeof(Select_stories), "Examples/Select")
            {
                Stories = {
                new("Examples/Select", "Select", new(), _EmptyFragment),
            }}
        };

        // When
        var root = builder.Build(storyContainers, "examples-button--primary");

        // Then
        root.Type.Is(NavigationTreeItemType.Container);
        root.SubItems.Count.Is(1);

        var examplesNode = root.SubItems[0];
        examplesNode.Type.Is(NavigationTreeItemType.Container);
        examplesNode.Caption.Is("Examples");
        examplesNode.SubItems.Count.Is(2);
        examplesNode.Expanded.IsTrue();

        var buttonNode = examplesNode.SubItems[0];
        buttonNode.Type.Is(NavigationTreeItemType.StoryCollection);
        buttonNode.Caption.Is("Button");
        buttonNode.SubItems.Count.Is(2);
        buttonNode.Expanded.IsTrue();

        var defaultButtonNode = buttonNode.SubItems[0];
        defaultButtonNode.Type.Is(NavigationTreeItemType.Story);
        defaultButtonNode.Caption.Is("Default");
        defaultButtonNode.NavigationPath.Is("examples-button--default");
        defaultButtonNode.SubItems.Count.Is(0);

        var primaryButtonNode = buttonNode.SubItems[1];
        primaryButtonNode.Type.Is(NavigationTreeItemType.Story);
        primaryButtonNode.Caption.Is("Primary");
        primaryButtonNode.NavigationPath.Is("examples-button--primary");
        primaryButtonNode.SubItems.Count.Is(0);

        var selectNode = examplesNode.SubItems[1];
        selectNode.Type.Is(NavigationTreeItemType.StoryCollection);
        selectNode.Caption.Is("Select");
        selectNode.SubItems.Count.Is(1);
        selectNode.Expanded.IsFalse();

        var selectSelectNode = selectNode.SubItems[0];
        selectSelectNode.Type.Is(NavigationTreeItemType.Story);
        selectSelectNode.Caption.Is("Select");
        selectSelectNode.NavigationPath.Is("examples-select--select");
        selectSelectNode.SubItems.Count.Is(0);
    }

    [Test]
    public void Build_SubHeadings_Should_Be_Expanded_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button_stories), "Components/Button"){ Stories = {
                new("Components/Button", "Default", new(), _EmptyFragment),
                new("Components/Button", "Primary", new(), _EmptyFragment),
            }},
            new(typeof(Select_stories), "Pages/Authentication"){ Stories = {
                new("Pages/Authentication", "Sign In", new(), _EmptyFragment),
                new("Pages/Authentication", "Sign Out", new(), _EmptyFragment),
            }},
        };

        // When
        var root = builder.Build(storyContainers, null);

        // Then
        var componentsNode = root.SubItems[0];
        componentsNode.Expanded.IsTrue();
        var buttonNode = componentsNode.SubItems[0];
        buttonNode.Expanded.IsFalse();

        var pagesNode = root.SubItems[1];
        pagesNode.Expanded.IsTrue();
        var authNode = pagesNode.SubItems[0];
        authNode.Expanded.IsFalse();
    }
}
