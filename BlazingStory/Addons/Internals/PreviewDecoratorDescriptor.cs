namespace BlazingStory.Addons.Internals;

internal class PreviewDecoratorDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly Type ComponentType;

    internal PreviewDecoratorDescriptor(Type previewDecoratorComponentType)
    {
        this.ComponentType = previewDecoratorComponentType;
    }
}
