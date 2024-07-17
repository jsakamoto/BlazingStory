using BlazingStory.Types;

namespace BlazingStory.Internals.Models;

internal class StoriesRazorDescriptor
{
    #region Internal Fields

    internal readonly Type TypeOfStoriesRazor;

    internal readonly StoriesAttribute StoriesAttribute;

    #endregion Internal Fields

    #region Public Constructors

    public StoriesRazorDescriptor(Type typeOfStoriesRazor, StoriesAttribute storiesAttribute)
    {
        this.TypeOfStoriesRazor = typeOfStoriesRazor;
        this.StoriesAttribute = storiesAttribute;
    }

    #endregion Public Constructors
}
