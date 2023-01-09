using System.Reflection;
using BlazingStory.Internals.Components;
using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Components;

internal class StoriesDetectorTest
{
    [Test]
    public void Detect_from_Empty_Test()
    {
        var storiesStore = new StoriesStore();
        using var ctx = new Bunit.TestContext();
        var cut = ctx.RenderComponent<StoriesDetector>(builder => builder
            .Add(_ => _.Assemblies, Enumerable.Empty<Assembly>())
            .Add(_ => _.StoriesStore, storiesStore));

        storiesStore.StoryContainers.Count().Is(0);
    }

    [Test]
    public void Detect_Test()
    {
        var storiesStore = new StoriesStore();

        using var ctx = new Bunit.TestContext();
        var cut = ctx.RenderComponent<StoriesDetector>(builder => builder
            .Add(_ => _.Assemblies, new[] {
                typeof(BlazingStoryApp1.App).Assembly,
                typeof(BlazingStoryApp2.App).Assembly,
            })
            .Add(_ => _.StoriesStore, storiesStore));

        storiesStore.StoryContainers.Count().Is(4);
    }
}
