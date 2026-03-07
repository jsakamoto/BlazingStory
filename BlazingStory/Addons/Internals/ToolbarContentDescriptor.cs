using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

internal class ToolbarContentDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly int Order;

    internal readonly Func<ViewMode, bool> Match;

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)]
    internal readonly Type ComponentType;

    internal readonly GlobalArguments Globals = new();

    internal readonly Dictionary<string, object?> ComponentParameter = new();

    internal ToolbarContentDescriptor(int order, Func<ViewMode, bool> match, [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] Type toolbarButtonComponentType)
    {
        this.Order = order;
        this.Match = match;
        this.ComponentType = toolbarButtonComponentType;
        this.ComponentParameter["Globals"] = this.Globals;
    }
}
