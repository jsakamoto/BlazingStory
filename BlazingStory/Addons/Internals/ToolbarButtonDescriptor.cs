namespace BlazingStory.Addons.Internals;

internal class ToolbarButtonDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly int Order;

    internal readonly Func<ViewMode, bool> Match;

    internal readonly Type ComponentType;

    internal readonly GlobalArguments Globals = new();

    internal readonly Dictionary<string, object?> ComponentParameter = new();

    internal ToolbarButtonDescriptor(int order, Func<ViewMode, bool> match, Type toolbarButtonComponentType)
    {
        this.Order = order;
        this.Match = match;
        this.ComponentType = toolbarButtonComponentType;
        this.ComponentParameter["Globals"] = this.Globals;
    }
}
