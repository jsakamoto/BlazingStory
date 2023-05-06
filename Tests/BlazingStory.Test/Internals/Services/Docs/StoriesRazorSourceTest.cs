using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Test._Fixtures;
using BlazingStory.Types;
using BlazingStoryApp1.Stories;
using Castle.Core.Internal;

namespace BlazingStory.Test.Internals.Services.Docs;

internal class StoriesRazorSourceTest
{
    [Test]
    public async Task GetSourceCodeAsync_Button_Primary_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Button_stories);
        var storyAttribute = typeof(Button_stories).GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, "Primary", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        sourceCode.Is(
            "<Button Text=\"Primary\" Color=\"ButtonColor.Primary\" @attributes=\"context.Args\" />"
        );
    }
    [Test]
    public async Task GetSourceCodeAsync_Button_Danger_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Button_stories);
        var storyAttribute = typeof(Button_stories).GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, "Danger", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        sourceCode.Is(
            """
            <h1>
                <span style="display:inline-block; padding:12px 24px; margin:24px 12px;">
                    This is Button
                </span>
            </h1>
            <Button @attributes="context.Args" />
            <h1>
                <span style="display:inline-block; padding:12px 24px; margin:24px 12px;">
                    This is Button
                </span>
            </h1>
            <Button @attributes="context.Args" />
            """
        );
    }
}
