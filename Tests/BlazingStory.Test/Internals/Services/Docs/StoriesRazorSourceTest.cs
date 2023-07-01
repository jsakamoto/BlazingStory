using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Test._Fixtures;
using BlazingStory.Types;
using BlazingStoryApp1.Stories;
using Castle.Core.Internal;
using static BlazingStory.Test._Fixtures.TestHelper;

namespace BlazingStory.Test.Internals.Services.Docs;

internal class StoriesRazorSourceTest
{
    [Test]
    public async Task GetSourceCodeAsync_Rating_RateControl_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Rating_stories);
        var storyAttribute = typeofStoriesRazor.GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, "Rate Control", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        sourceCode.Is("<Rating @attributes=\"context.Args\" />");
    }

    [Test]
    public async Task GetSourceCodeAsync_Button_Danger_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Button_stories);
        var storyAttribute = typeofStoriesRazor.GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, "Danger", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        var expected = string.Join('\n', """
            <h1>
                <span style="display:inline-block; padding:12px 24px; margin:24px 12px;">
                    This is Button
                </span>
            </h1>

            <Button @attributes="context.Args" />
            """.Split('\n').Select(s => s.TrimEnd('\r')));
        sourceCode.Is(expected);
    }
}
