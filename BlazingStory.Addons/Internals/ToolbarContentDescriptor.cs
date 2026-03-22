using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

/// <summary>
/// Describes a registered toolbar content component, including its order, visibility predicate, component type, and global arguments.
/// </summary>
internal class ToolbarContentDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly int Order;

    internal readonly Func<ViewMode, bool> Match;

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    internal readonly GlobalArguments Globals = new();

    internal readonly Dictionary<string, object?> ComponentParameter = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ToolbarContentDescriptor"/>.
    /// </summary>
    /// <param name="order">The display order relative to other toolbar content.</param>
    /// <param name="match">A predicate that determines whether this content is visible for a given view mode.</param>
    /// <param name="toolbarButtonComponentType">The Blazor component type used to render the toolbar content.</param>
    internal ToolbarContentDescriptor(int order, Func<ViewMode, bool> match, [DynamicallyAccessedMembers(All)] Type toolbarButtonComponentType)
    {
        this.Order = order;
        this.Match = match;
        this.ComponentType = toolbarButtonComponentType;
        this.ComponentParameter["Globals"] = this.Globals;
    }
}
