using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class CustomRazorDetectorTest
{
    [Test]
    public void Detect_from_Empty_Test()
    {
        var store = new CustomPageStore();

        CustomPageRazorDetector.DetectAndRegisterToStore([], store);

        store.CustomPageContainers.Count().Is(0);
    }

    [Test]
    public void Detect_Test()
    {
        var store = new CustomPageStore();
        var app1Assembly = typeof(BlazingStoryApp1.App).Assembly;
        var app2Assembly = typeof(BlazingStoryApp2.App).Assembly;

        CustomPageRazorDetector.DetectAndRegisterToStore([app1Assembly, app2Assembly], store);

        store.CustomPageContainers.Select(c => $"{c.Title},{c.NavigationPath}")
            .Order()
            .Is("Examples/Blazing Story,examples-blazing-story",
                "Examples/Getting Started,examples-getting-started",
                "Examples/Sample of Markdown,examples-sample-of-markdown");
    }
}
