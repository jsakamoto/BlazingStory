using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons;

/// <summary>
/// Provides methods for registering addon components such as toolbar content, panels, and preview decorators.
/// </summary>
public interface IAddonBuilder
{
    /// <summary>
    /// Registers a toolbar content component shown when the current view mode matches.
    /// </summary>
    /// <param name="order">The display order of the toolbar content relative to others.</param>
    /// <param name="match">A predicate that determines whether this content is shown for a given view mode.</param>
    void AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match);

    /// <summary>
    /// Registers a panel component shown in the addon panel area when the current view mode matches.
    /// </summary>
    /// <param name="order">The display order of the panel relative to others.</param>
    /// <param name="match">A predicate that determines whether this panel is shown for a given view mode.</param>
    void AddPanel<[DynamicallyAccessedMembers(All)] TPanelComponent>(int order, Func<ViewMode, bool> match);

    /// <summary>
    /// Registers a preview decorator component that wraps the story preview.
    /// </summary>
    void AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>();
}
