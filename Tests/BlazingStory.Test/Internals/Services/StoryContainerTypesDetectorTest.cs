using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class StoriesRazorDetectorTest
{
    [Test]
    public void Detect_from_Null_Test()
    {
        var typeOfStoriesRazors = StoriesRazorDetector.Detect(null);

        typeOfStoriesRazors.Count().Is(0);
    }

    [Test]
    public void Detect_Test()
    {
        var typeOfStoriesRazors = StoriesRazorDetector.Detect([
            typeof(BlazingStoryApp1.App).Assembly,
            typeof(BlazingStoryApp2.App).Assembly,
        ]);

        typeOfStoriesRazors
            .Select(c => $"{c.StoriesAttribute.Title},{c.TypeOfStoriesRazor.FullName}")
            .Is("Examples/UI/Button,BlazingStoryApp1.Stories.Button_stories",
                "Examples/UI/DumpParameter,BlazingStoryApp1.Stories.DumpParameter_stories",
                "Lorem/Ipsum/Header,BlazingStoryApp1.Stories.LoremIpsum_stories",
                "Examples/UI/Rating,BlazingStoryApp1.Stories.Rating_stories",
                "Examples/Select,BlazingStoryApp1.Stories.Select_stories",
                "Examples/UI/TextInput,BlazingStoryApp1.Stories.TextInput_stories",
                "Examples/Card,BlazingStoryApp2.Stories.Card_stories");
    }
}
