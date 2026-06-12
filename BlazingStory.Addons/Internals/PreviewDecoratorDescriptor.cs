using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

/// <summary>
/// Describes a registered preview decorator component that wraps the story canvas preview.
/// </summary>
internal class PreviewDecoratorDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    /// <summary>
    /// Initializes a new instance of <see cref="PreviewDecoratorDescriptor"/>.
    /// </summary>
    /// <param name="previewDecoratorComponentType">The Blazor component type used as the preview decorator.</param>
    internal PreviewDecoratorDescriptor([DynamicallyAccessedMembers(All)] Type previewDecoratorComponentType)
    {
        this.ComponentType = previewDecoratorComponentType;
    }
}
