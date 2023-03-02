using BlazingStory.Internals.Models;
using BlazingStoryApp1.Stories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.Test._Fixtures;

internal class TestHelper
{
    internal static readonly RenderFragment<Types.StoryContext> EmptyFragment = ctx => ((RenderTreeBuilder _) => { });

    internal static class StoryContext
    {
        internal static Types.StoryContext CreateEmpty() => new(Enumerable.Empty<ComponentParameter>());
    }

    internal static readonly IEnumerable<StoryContainer> ExampleStories1 = new StoryContainer[] {
            new(typeof(Button_stories), "Examples/Button") { Stories = {
                new("Examples/Button", "Default Button", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
                new("Examples/Button", "Primary Button", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }},
            new(typeof(Select_stories), "Examples/Select") { Stories = {
                new("Examples/Select", "Select", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }}
        };
}
