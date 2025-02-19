using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class CustomRazorDetectorTest
{
    [Test]
    public void Detect_from_Empty_Test()
    {
        var store = new CustomStore();

        CustomRazorDetector.DetectAndRegisterToStore([], store);

        store.CustomContainers.Count().Is(0);
    }

    [Test]
    public void Detect_Test()
    {
        var customStore = new CustomStore();
        var app1Assembly = typeof(BlazingStoryApp1.App).Assembly;
        var app2Assembly = typeof(BlazingStoryApp2.App).Assembly;

        CustomRazorDetector.DetectAndRegisterToStore([app1Assembly, app2Assembly], customStore);

        customStore.CustomContainers.Count().Is(2);
        customStore.CustomContainers.Select(c => $"{c.Title},{c.NavigationPath}")
            .Is("Examples/XGetting Started,examples-xgetting-started",
                "Examples/UI/Welcome,examples-ui-welcome");
    }
}
