using BlazingStory.Internals.Models;

namespace BlazingStory.Test.Internals.Models;

internal class StoryTest
{
    [Test]
    public void NavigationPath_Test()
    {
        var story = new Story("Pages/Authentication and Authorization", "Sign In", new(), null!);
        story.NavigationPath.Is("pages-authentication-and-authorization--sign-in");
    }
}
