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
    public async Task GetSourceCodeAsync_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Button_stories);
        var storyAttribute = typeof(Button_stories).GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, "Default", TestHelper.StoryContext.CreateEmpty(), TestHelper.EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        sourceCode.Is(
            "<Button Text=\"Default\" Color=\"ButtonColor.Default\" @attributes=\"context.Args\" />"
        );
    }
}
