using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Test._Fixtures;
using BlazingStory.Types;
using BlazingStoryApp1.Stories;
using Castle.Core.Internal;
using RazorClassLib1.Components.Button;
using RazorClassLib1.Components.Rating;
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
        var story = new Story(descriptor, typeof(Rating), "Rate Control", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

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
        var story = new Story(descriptor, typeof(Button), "Danger", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

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

    [Test]
    public async Task UpdateSourceTextWithArgument_Test()
    {
        // Given
        var sourceText = "<Button @attributes=\"context.Args\" />";
        var story = TestHelper.CreateStory<Button>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Bold), true);
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Color), ButtonColor.Default);

        // When 
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is("<Button Bold=\"true\" Color=\"ButtonColor.Default\" />");
    }
}
