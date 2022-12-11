using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class StoryContainerTypesDetectorTest
{
    [Test]
    public void Detect_from_Null_Test()
    {
        var detector = new StoryContainerTypesDetector();
        var storyContainers = detector.DetectContainers(null);

        storyContainers.Count().Is(0);
    }

    [Test]
    public void Detect_Test()
    {
        var detector = new StoryContainerTypesDetector();
        var storyContainers = detector.DetectContainers(new[] {
            typeof(BlazingStoryApp1.App).Assembly,
            typeof(BlazingStoryApp2.App).Assembly,
        });

        storyContainers
            .Select(c => $"{c.Title},{c.ComponentType.FullName}")
            .Is("Examples/Button,BlazingStoryApp1.Stories.Button_stories",
                "Examples/Select,BlazingStoryApp1.Stories.Select_stories",
                "Examples/Card,BlazingStoryApp2.Stories.Card_stories");
    }
}
