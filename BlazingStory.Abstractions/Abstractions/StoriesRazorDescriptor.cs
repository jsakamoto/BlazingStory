using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Abstractions;

/// <summary>
/// Holds the type and attribute information for a Stories Razor component.
/// </summary>
public class StoriesRazorDescriptor
{
    /// <summary>
    /// The type of the Stories Razor component.
    /// </summary>
    [DynamicallyAccessedMembers(All)]
    public readonly Type TypeOfStoriesRazor;

    /// <summary>
    /// The <see cref="Types.StoriesAttribute"/> applied to the Stories Razor component.
    /// </summary>
    public readonly StoriesAttribute StoriesAttribute;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoriesRazorDescriptor"/> class.
    /// </summary>
    /// <param name="typeOfStoriesRazor">The type of the Stories Razor component.</param>
    /// <param name="storiesAttribute">The attribute applied to the Stories Razor component.</param>
    public StoriesRazorDescriptor([DynamicallyAccessedMembers(All)] Type typeOfStoriesRazor, StoriesAttribute storiesAttribute)
    {
        this.TypeOfStoriesRazor = typeOfStoriesRazor;
        this.StoriesAttribute = storiesAttribute;
    }
}
