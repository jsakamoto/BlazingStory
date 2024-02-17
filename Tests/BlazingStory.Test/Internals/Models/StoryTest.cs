using BlazingStory.Internals.Models;
using static BlazingStory.Test._Fixtures.TestHelper;

namespace BlazingStory.Test.Internals.Models;

internal class StoryTest
{
    [Test]
    public void NavigationPath_Test()
    {
        var story = new Story(Descriptor("Pages/Authentication and Authorization"), typeof(object), "Sign In", new([]), null, null, EmptyFragment);
        story.NavigationPath.Is("pages-authentication-and-authorization--sign-in");
    }
}
