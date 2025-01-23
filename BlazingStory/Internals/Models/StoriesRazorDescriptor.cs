using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

internal class StoriesRazorDescriptor
{
    [DynamicallyAccessedMembers(All)]
    internal readonly Type TypeOfStoriesRazor;

    internal readonly StoriesAttribute StoriesAttribute;

    public StoriesRazorDescriptor([DynamicallyAccessedMembers(All)] Type typeOfStoriesRazor, StoriesAttribute storiesAttribute)
    {
        this.TypeOfStoriesRazor = typeOfStoriesRazor;
        this.StoriesAttribute = storiesAttribute;
    }
}
