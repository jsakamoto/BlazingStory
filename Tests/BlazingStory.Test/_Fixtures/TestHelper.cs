using BlazingStory.Internals.Models;
using BlazingStoryApp1.Stories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLib1.Components.Button;

namespace BlazingStory.Test._Fixtures;

internal static class TestHelper
{
    internal static readonly RenderFragment<Types.StoryContext> EmptyFragment = ctx => ((RenderTreeBuilder _) => { });

    internal static StoriesRazorDescriptor Descriptor(string title)
    {
        return new(typeof(object), new(title));
    }

    internal static class StoryContext
    {
        internal static Types.StoryContext CreateEmpty() => new(Enumerable.Empty<ComponentParameter>());
    }

    internal static IEnumerable<StoryContainer> GetExampleStories1(IServiceProvider services) => new StoryContainer[] {
            new(typeof(Button), new(typeof(Button_stories), new("Examples/Button")), services) { Stories = {
                new(Descriptor("Examples/Button"), "Default Button", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
                new(Descriptor("Examples/Button"), "Primary Button", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }},
            new(typeof(Button), new(typeof(Select_stories), new("Examples/Select")), services) { Stories = {
                new(Descriptor("Examples/Select"), "Select", StoryContext.CreateEmpty(), TestHelper.EmptyFragment),
            }}
        };
}
