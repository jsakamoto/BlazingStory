using BlazingStory.Types;

namespace BlazingStory.Internals.Models;

internal class StoriesRazorDescriptor
{
    internal readonly Type TypeOfStoriesRazor;
    internal readonly StoriesAttribute StoriesAttribute;

    public StoriesRazorDescriptor(Type typeOfStoriesRazor, StoriesAttribute storiesAttribute)
    {
        this.TypeOfStoriesRazor = typeOfStoriesRazor;
        this.StoriesAttribute = storiesAttribute;
    }
}
