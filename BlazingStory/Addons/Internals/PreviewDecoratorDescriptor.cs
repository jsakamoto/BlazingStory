using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

internal class PreviewDecoratorDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    internal PreviewDecoratorDescriptor([DynamicallyAccessedMembers(All)] Type previewDecoratorComponentType)
    {
        this.ComponentType = previewDecoratorComponentType;
    }
}
