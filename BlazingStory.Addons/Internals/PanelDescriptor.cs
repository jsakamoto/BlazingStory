using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

/// <summary>
/// Describes a registered addon panel component, including its order, visibility predicate, and component type.
/// </summary>
internal class PanelDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly int Order;

    internal readonly Func<ViewMode, bool> Match;

    internal string Name => this.ComponentType?.Name ?? "(null)";

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    /// <summary>
    /// Initializes a new instance of <see cref="PanelDescriptor"/>.
    /// </summary>
    /// <param name="order">The display order relative to other panels.</param>
    /// <param name="match">A predicate that determines whether this panel is visible for a given view mode.</param>
    /// <param name="panelComponentType">The Blazor component type used to render the panel.</param>
    public PanelDescriptor(int order, Func<ViewMode, bool> match, [DynamicallyAccessedMembers(All)] Type panelComponentType)
    {
        this.Order = order;
        this.Match = match;
        this.ComponentType = panelComponentType;
    }
}