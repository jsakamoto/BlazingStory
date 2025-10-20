using System;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Dummies;
using BlazingStoryApp1.Stories;
using RazorClassLib1.Components.Button;
using RazorClassLib1.Components.Rating;
using RazorClassLib1.Components.Select;
using static BlazingStory.Test._Fixtures.TestHelper;
using static BlazingStory.Types.NavigationTreeOrderBuilder;
using BlazingStory.Test._Fixtures;

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
        var root = builder.Build(storyContainers, [], [], null);

        // Then
        root.Type.Is(NavigationItemType.Container);
        root.SubItems.Count.Is(0);
    }

    [Test]
    public async Task Build_Test()
    {
        // Given
        await using var host = new TestHost();
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button), null, new(typeof(Button_stories), new("Examples/Button")), host.Services) { Stories = {
                CreateStory<Button>("Examples/Button", "Default"),
                CreateStory<Button>("Examples/Button", "Primary"),
            }},
            new(typeof(Button), null, new(typeof(Select_stories), new("Examples/Select")), host.Services) { Stories = {
                CreateStory<Button>("Examples/Select", "Select"),
            }}
        };

        // When
        var root = builder.Build(storyContainers, [], [], expandedNavigationPath: "/story/examples-button--primary");

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
        buttonNode.Type.Is(NavigationItemType.Component);
        buttonNode.Caption.Is("Button");
        buttonNode.SubItems.Count.Is(3);
        buttonNode.Expanded.IsTrue();
        buttonNode.PathSegments.Is("Examples");

        var docsOfButtonNode = buttonNode.SubItems[0];
        docsOfButtonNode.Type.Is(NavigationItemType.Docs);
        docsOfButtonNode.Caption.Is("Docs");
        docsOfButtonNode.NavigationPath.Is("/docs/examples-button--docs");
        docsOfButtonNode.SubItems.Count.Is(0);
        docsOfButtonNode.PathSegments.Is("Examples", "Button");

        var defaultButtonNode = buttonNode.SubItems[1];
        defaultButtonNode.Type.Is(NavigationItemType.Story);
        defaultButtonNode.Caption.Is("Default");
        defaultButtonNode.NavigationPath.Is("/story/examples-button--default");
        defaultButtonNode.SubItems.Count.Is(0);
        defaultButtonNode.PathSegments.Is("Examples", "Button");

        var primaryButtonNode = buttonNode.SubItems[2];
        primaryButtonNode.Type.Is(NavigationItemType.Story);
        primaryButtonNode.Caption.Is("Primary");
        primaryButtonNode.NavigationPath.Is("/story/examples-button--primary");
        primaryButtonNode.SubItems.Count.Is(0);
        primaryButtonNode.PathSegments.Is("Examples", "Button");

        var selectNode = examplesNode.SubItems[1];
        selectNode.Type.Is(NavigationItemType.Component);
        selectNode.Caption.Is("Select");
        selectNode.SubItems.Count.Is(2);
        selectNode.Expanded.IsFalse();
        selectNode.PathSegments.Is("Examples");

        var docsOfSelectNode = selectNode.SubItems[0];
        docsOfSelectNode.Type.Is(NavigationItemType.Docs);
        docsOfSelectNode.Caption.Is("Docs");
        docsOfSelectNode.NavigationPath.Is("/docs/examples-select--docs");
        docsOfSelectNode.SubItems.Count.Is(0);
        docsOfSelectNode.PathSegments.Is("Examples", "Select");

        var selectSelectNode = selectNode.SubItems[1];
        selectSelectNode.Type.Is(NavigationItemType.Story);
        selectSelectNode.Caption.Is("Select");
        selectSelectNode.NavigationPath.Is("/story/examples-select--select");
        selectSelectNode.SubItems.Count.Is(0);
        selectSelectNode.PathSegments.Is("Examples", "Select");
    }

    [Test]
    public async Task Build_SubHeadings_Should_Be_Expanded_Test()
    {
        // Given
        await using var host = new TestHost();
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button), null, new(typeof(Button_stories), new("Components/Button")), host.Services){ Stories = {
                CreateStory<Button>("Components/Button", "Default"),
                CreateStory<Button>("Components/Button", "Primary"),
            }},
            new(typeof(Button), null, new(typeof(Select_stories), new("Pages/Authentication")), host.Services){ Stories = {
                CreateStory<Button>("Pages/Authentication", "Sign In"),
                CreateStory<Button>("Pages/Authentication", "Sign Out"),
            }},
        };

        // When
        var root = builder.Build(storyContainers, [], [], null);

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

    [Test]
    public async Task Build_TopLevel_Is_Component_Test()
    {
        // Given
        await using var host = new TestHost();
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button), null, new(typeof(Button_stories), new("Button")), host.Services){ Stories = {
                CreateStory<Button>("Button", "Default"),
                CreateStory<Button>("Button", "Primary"),
            }}
        };

        // When
        var root = builder.Build(storyContainers, [], [], null);

        // Then
        root.SubItems.Count.Is(1);
        root.SubItems[0].Dump().Is("Component | Button | 3 | Expanded");
        root.SubItems[0].SubItems[0].Dump().Is("Docs | Docs | 0");
        root.SubItems[0].SubItems[1].Dump().Is("Story | Default | 0");
        root.SubItems[0].SubItems[2].Dump().Is("Story | Primary | 0");
    }

    [Test]
    public async Task Build_TopLevel_Is_CustomPage_Test()
    {
        // Given
        await using var host = new TestHost();
        var builder = new NavigationTreeBuilder();
        var storyContainers = new StoryContainer[] {
            new(typeof(Button), null, new(typeof(Button_stories), new("Components/Button")), host.Services){ Stories = {
                CreateStory<Button>("Button", "Default"),
                CreateStory<Button>("Button", "Primary"),
            }}
        };
        var customPageContainers = new CustomPageContainer[] {
            new(new(typeof(DummyPage), new("Welcome"))),
        };

        // When
        var root = builder.Build(storyContainers, customPageContainers, [], null);

        // Then
        root.SubItems.Count.Is(2);
        root.SubItems[0].Dump().Is("CustomPage | Welcome | 0 | Expanded");
        root.SubItems[1].Dump().Is("Container | Components | 1 | Expanded");
        root.SubItems[1].SubItems[0].Dump().Is("Component | Button | 3");
        root.SubItems[1].SubItems[0].SubItems[0].Dump().Is("Docs | Docs | 0");
        root.SubItems[1].SubItems[0].SubItems[1].Dump().Is("Story | Default | 0");
        root.SubItems[1].SubItems[0].SubItems[2].Dump().Is("Story | Primary | 0");
    }

    [Test]
    public async Task Build_and_its_Ordering_Test()
    {
        // Given
        await using var host = new TestHost();
        var storyContainers = new StoryContainer[] {
            new(typeof(Rating), null, new(typeof(Select_stories), new("UI Components/Atoms/Rating")), host.Services) { Stories = {
                CreateStory<Rating>("UI Components/Atoms/Rating", "Default"),
            }},
            new(typeof(Select), null, new(typeof(Select_stories), new("Examples/Select")), host.Services) { Stories = {
                CreateStory<Select>("Examples/Select", "Single Select"),
                CreateStory<Select>("Examples/Select", "Multiple Select"),
            }},
            new(typeof(Button), null, new(typeof(Button_stories), new("Examples/Button")), host.Services) { Stories = {
                CreateStory<Button>("Examples/Button", "Default"),
                CreateStory<Button>("Examples/Button", "Primary"),
                CreateStory<Button>("Examples/Button", "Danger"),
            }},
        };
        var customPageContainers = new CustomPageContainer[] {
            new(new(typeof(DummyPage), new("Examples/Welcome"))),
            new(new(typeof(DummyPage), new("Examples/Sample of Markdown"))),
            new(new(typeof(DummyPage), new("UI Components/Atoms/Rating/Use Cases"))),
            new(new(typeof(DummyPage), new("UI Components/Atoms/Rating/Overview"))),
            new(new(typeof(DummyPage), new("For Your Team"))),
        };

        // When
        var builder = new NavigationTreeBuilder();
        var root = builder.Build(storyContainers, customPageContainers, [], expandedNavigationPath: null);

        // Then: The 1st level nodes were alphabetically sorted, but custom pages are prioritized.
        root.SubItems.Captions().Is("For Your Team", "Examples", "UI Components"); 

        // In the "Examples" node, components and custom pages nodes were sorted in alphabetical order, but custom pages were always listed before components.
        root.SubItems[1].SubItems.Captions().Is("Sample of Markdown", "Welcome", "Button", "Select");

        root.SubItems[1].SubItems[2].SubItems.Captions().Is("Docs", "Default", "Primary", "Danger"); // Stories were kept in the original order.
        root.SubItems[1].SubItems[3].SubItems.Captions().Is("Docs", "Single Select", "Multiple Select");

        root.SubItems[2].SubItems[0].SubItems[0].SubItems.Captions().Is("Overview", "Use Cases", "Docs", "Default");
    }

    [Test]
    public async Task Build_and_Custom_Ordering_Test()
    {
        // Given
        await using var host = new TestHost();
        var storyContainers = new StoryContainer[] {
            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Components/Slider")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Components/Slider", "Default"),
            }},

            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Components/Layouts/Header")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Components/Layouts/Header", "Default"),
            }},
            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Components/Layouts/Footer")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Components/Layouts/Footer", "Default"),
            }},

            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Components/Button")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Components/Button", "Default"),
                CreateStory<DummyComponent>("Components/Button", "Small"),
                CreateStory<DummyComponent>("Components/Button", "Large"),
            }},

            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Templates/SignInForm")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Templates/SignInForm", "Default"),
            }},
            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Templates/ConfirmDialog")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Templates/ConfirmDialog", "Default"),
            }},

            new(typeof(DummyComponent), null, new(typeof(DummyStories), new("Others/Overlay")), host.Services) { Stories = {
                CreateStory<DummyComponent>("Others/Overlay", "Default"),
            }},
        };
        var customPageContainers = new CustomPageContainer[] {
            new(new(typeof(DummyPage), new("Overview"))),
            new(new(typeof(DummyPage), new("Welcome"))),
            new(new(typeof(DummyPage), new("Getting Started"))),
            new(new(typeof(DummyPage), new("Components/Slider/Notice"))),
            new(new(typeof(DummyPage), new("Components/Slider/Warning"))),
            new(new(typeof(DummyPage), new("Components/Slider/Advice"))),
        };

        // When
        var builder = new NavigationTreeBuilder();
        var customOrdering = N[
            "Components",
            N[
                "Layouts",
                N[
                    "Header",
                    "Footer"
                ],
                "Slider",
                N[
                    "Warning"
                ]
            ],
            "Welcome",
            "Templates"];
        var root = builder.Build(storyContainers, customPageContainers, customOrdering, expandedNavigationPath: null);

        // Then
        root.SubItems.Captions().Is("Components", "Welcome", "Templates", "Getting Started", "Overview", "Others"); // The 1st level nodes were sorted in the custom order.
        root.SubItems[0].SubItems.Captions().Is("Layouts", "Slider", "Button"); // The 2nd level nodes were also sorted in the custom order.
        root.SubItems[0].SubItems[0].SubItems.Captions().Is("Header", "Footer");
        root.SubItems[0].SubItems[0].SubItems[0].SubItems.Captions().Is("Docs", "Default");
        root.SubItems[0].SubItems[0].SubItems[1].SubItems.Captions().Is("Docs", "Default");

        root.SubItems[0].SubItems[1].SubItems.Captions().Is("Warning", "Advice", "Notice", "Docs", "Default"); // The custom-ordered page node should be brought up first

        root.SubItems[0].SubItems[2].SubItems.Captions().Is("Docs", "Default", "Small", "Large");

        root.SubItems[2].SubItems.Captions().Is("ConfirmDialog", "SignInForm"); // In the "Templates" node, components nodes were sorted in alphabetical order because they were not specified in the custom order.
        root.SubItems[2].SubItems[0].SubItems.Captions().Is("Docs", "Default");
        root.SubItems[2].SubItems[1].SubItems.Captions().Is("Docs", "Default");

        root.SubItems[5].SubItems.Captions().Is("Overlay");
        root.SubItems[5].SubItems[0].SubItems.Captions().Is("Docs", "Default");
    }
}
