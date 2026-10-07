using System.Diagnostics.CodeAnalysis;
using BlazingStory.ToolKit.Icons;
using Toolbelt.Blazor.HotKeys2;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons;

/// <summary>
/// Provides methods for registering addon components such as toolbar content, panels, preview decorators, and settings pages.
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

    /// <summary>
    /// Registers a settings page component shown at <c>?path=/settings/{route}</c>.<br/>
    /// The page also appears as a tab on the settings page and as an item in the "Shortcuts" menu.
    /// </summary>
    /// <param name="order">The display order of the tab and the menu item relative to others.</param>
    /// <param name="route">The route parameter after <c>?path=/settings/</c>, like <c>"about"</c>.</param>
    /// <param name="icon">The icon of the item in the "Shortcuts" menu.</param>
    /// <param name="menuCaption">The caption of the item in the "Shortcuts" menu. This is also used as the command title.</param>
    /// <param name="tabCaption">The caption of the tab on the settings page.</param>
    /// <param name="hotKeyEntryName">The name used to save the hot key of this page. If specified, the page is listed in the "Keyboard shortcuts" panel. If <see langword="null"/>, the page has no keyboard shortcut entry.</param>
    /// <param name="defaultHotKeyModifiers">The modifier keys of the default hot key. Only used when <paramref name="hotKeyEntryName"/> is specified.</param>
    /// <param name="defaultHotKeyCode">The key code of the default hot key. Only used when <paramref name="hotKeyEntryName"/> is specified.</param>
    void AddSettingsPage<[DynamicallyAccessedMembers(All)] TSettingsPageComponent>(
        int order,
        string route,
        SvgIconType icon,
        string menuCaption,
        string tabCaption,
        string? hotKeyEntryName = null,
        ModCode defaultHotKeyModifiers = ModCode.None,
        Code? defaultHotKeyCode = null);
}
