using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Abstractions;

public class StoriesRazorDescriptor
{
    [DynamicallyAccessedMembers(All)]
    public readonly Type TypeOfStoriesRazor;

    public readonly StoriesAttribute StoriesAttribute;

    public StoriesRazorDescriptor([DynamicallyAccessedMembers(All)] Type typeOfStoriesRazor, StoriesAttribute storiesAttribute)
    {
        this.TypeOfStoriesRazor = typeOfStoriesRazor;
        this.StoriesAttribute = storiesAttribute;
    }
}
