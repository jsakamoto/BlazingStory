using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

internal class PreviewDecoratorDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)]
    internal readonly Type ComponentType;

    internal PreviewDecoratorDescriptor([DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] Type previewDecoratorComponentType)
    {
        this.ComponentType = previewDecoratorComponentType;
    }
}
