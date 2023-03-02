﻿using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Test._Fixtures;
using BlazingStoryApp1.Stories;

namespace BlazingStory.Test.Internals.Services.Navigation;

internal class NavigationTreeBuilderTest
{
    [Test]
    public void Build_from_EmptyBook_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = Array.Empty<StoryContainer>();

        // When
        var root = builder.Build(storyContainers, null);

        // Then
        root.Type.Is(NavigationItemType.Container);
        root.SubItems.Count.Is(0);
    }

    [Test]
    public void Build_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button_stories), "Examples/Button"){ Stories = {
                new("Examples/Button", "Default", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
                new("Examples/Button", "Primary", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }},
            new(typeof(Select_stories), "Examples/Select")
            {
                Stories = {
                new("Examples/Select", "Select", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }}
        };

        // When
        var root = builder.Build(storyContainers, "examples-button--primary");

        // Then
        root.Type.Is(NavigationItemType.Container);
        root.SubItems.Count.Is(1);

        var examplesNode = root.SubItems[0];
        examplesNode.Type.Is(NavigationItemType.Container);
        examplesNode.Caption.Is("Examples");
        examplesNode.SubItems.Count.Is(2);
        examplesNode.Expanded.IsTrue();
        examplesNode.PathSegments.Count().Is(0);

        var buttonNode = examplesNode.SubItems[0];
        buttonNode.Type.Is(NavigationItemType.StoryCollection);
        buttonNode.Caption.Is("Button");
        buttonNode.SubItems.Count.Is(2);
        buttonNode.Expanded.IsTrue();
        buttonNode.PathSegments.Is("Examples");

        var defaultButtonNode = buttonNode.SubItems[0];
        defaultButtonNode.Type.Is(NavigationItemType.Story);
        defaultButtonNode.Caption.Is("Default");
        defaultButtonNode.NavigationPath.Is("examples-button--default");
        defaultButtonNode.SubItems.Count.Is(0);
        defaultButtonNode.PathSegments.Is("Examples", "Button");

        var primaryButtonNode = buttonNode.SubItems[1];
        primaryButtonNode.Type.Is(NavigationItemType.Story);
        primaryButtonNode.Caption.Is("Primary");
        primaryButtonNode.NavigationPath.Is("examples-button--primary");
        primaryButtonNode.SubItems.Count.Is(0);
        primaryButtonNode.PathSegments.Is("Examples", "Button");

        var selectNode = examplesNode.SubItems[1];
        selectNode.Type.Is(NavigationItemType.StoryCollection);
        selectNode.Caption.Is("Select");
        selectNode.SubItems.Count.Is(1);
        selectNode.Expanded.IsFalse();
        selectNode.PathSegments.Is("Examples");

        var selectSelectNode = selectNode.SubItems[0];
        selectSelectNode.Type.Is(NavigationItemType.Story);
        selectSelectNode.Caption.Is("Select");
        selectSelectNode.NavigationPath.Is("examples-select--select");
        selectSelectNode.SubItems.Count.Is(0);
        selectSelectNode.PathSegments.Is("Examples", "Select");
    }

    [Test]
    public void Build_SubHeadings_Should_Be_Expanded_Test()
    {
        // Given
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button_stories), "Components/Button"){ Stories = {
                new("Components/Button", "Default", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
                new("Components/Button", "Primary", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }},
            new(typeof(Select_stories), "Pages/Authentication"){ Stories = {
                new("Pages/Authentication", "Sign In", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
                new("Pages/Authentication", "Sign Out", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
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