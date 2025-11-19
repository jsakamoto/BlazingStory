using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStoryApp1.Stories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLib1.Components.Button;
using RazorClassLib1.Components.Select;

namespace BlazingStory.Test._Fixtures;

internal static class TestHelper
{
    internal static readonly RenderFragment<BlazingStory.Types.StoryContext> EmptyFragment = ctx => ((RenderTreeBuilder _) => { });

    internal static StoriesRazorDescriptor Descriptor(string title)
    {
        return new(typeof(object), new(title));
    }

    internal static class StoryContext
    {
        internal static BlazingStory.Types.StoryContext CreateEmpty() => new(Enumerable.Empty<ComponentParameter>());
    }

    internal static Story CreateStory<TComponent>(string title = "", string name = "")
    {
        var parameters = ParameterExtractor.GetParametersFromComponentType(typeof(TComponent), XmlDocComment.Dummy);
        var context = new BlazingStory.Types.StoryContext(parameters);
        return new Story(Descriptor(title), typeof(TComponent), name, context, null, null, EmptyFragment, null);
    }

    internal static IEnumerable<StoryContainer> GetExampleStories1(IServiceProvider services) => [
        new(typeof(Button), null, new(typeof(Button_stories), new("Examples/Button")), services) { Stories = {
            CreateStory<Button>(title: "Examples/Button", name: "Default Button"),
            CreateStory<Button>(title: "Examples/Button", name: "Primary Button"),
        }},
        new(typeof(Select), null, new(typeof(Select_stories), new("Examples/Select")), services) { Stories = {
            CreateStory<Button>(title: "Examples/Select", name: "Select"),
        }}
    ];
}
