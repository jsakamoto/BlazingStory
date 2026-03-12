using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

internal class PanelDescriptor
{
    internal readonly Guid Id = Guid.NewGuid();

    internal readonly int Order;

    internal readonly Func<ViewMode, bool> Match;

    internal string Name => this.ComponentType?.Name ?? "(null)";

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    public PanelDescriptor(int order, Func<ViewMode, bool> match, [DynamicallyAccessedMembers(All)] Type panelComponentType)
    {
        this.Order = order;
        this.Match = match;
        this.ComponentType = panelComponentType;
    }
}