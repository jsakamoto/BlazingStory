using BlazingStory.Internals.Models;
using BlazingStory.Test._Fixtures;
using BlazingStory.Types;

namespace BlazingStory.Test.Internals.Models;

internal class StoryTest
{
    [Test]
    public void NavigationPath_Test()
    {
        var storyContext = new StoryContext(Enumerable.Empty<ComponentParameter>());
        var story = new Story(TestHelper.Descriptor("Pages/Authentication and Authorization"), "Sign In", storyContext, null!);
        story.NavigationPath.Is("pages-authentication-and-authorization--sign-in");
    }
}
