using System.Diagnostics.CodeAnalysis;
using BlazingStory.ToolKit.Icons;
using Toolbelt.Blazor.HotKeys2;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

/// <summary>
/// Describes a registered settings page component, including its order, route, captions, icon, and hot key settings.
/// </summary>
internal class SettingsPageDescriptor
{
    internal readonly int Order;

    internal readonly string Route;

    internal readonly SvgIconType Icon;

    internal readonly string MenuCaption;

    internal readonly string TabCaption;

    internal readonly string? HotKeyEntryName;

    internal readonly ModCode DefaultHotKeyModifiers;

    internal readonly Code? DefaultHotKeyCode;

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ComponentType;

    /// <summary>
    /// Gets the key of the command that opens this settings page.<br/>
    /// This is <see cref="HotKeyEntryName"/> if specified; otherwise, a key built from the route, like "settings/about".
    /// </summary>
    internal string CommandKey => this.HotKeyEntryName ?? $"settings/{this.Route}";

    /// <summary>
    /// Initializes a new instance of <see cref="SettingsPageDescriptor"/>.
    /// </summary>
    /// <param name="order">The display order relative to other settings pages.</param>
    /// <param name="route">The route parameter after "?path=/settings/".</param>
    /// <param name="icon">The icon of the item in the "Shortcuts" menu.</param>
    /// <param name="menuCaption">The caption of the item in the "Shortcuts" menu.</param>
    /// <param name="tabCaption">The caption of the tab on the settings page.</param>
    /// <param name="hotKeyEntryName">The name used to save the hot key, or <see langword="null"/> if the page has no keyboard shortcut entry.</param>
    /// <param name="defaultHotKeyModifiers">The modifier keys of the default hot key.</param>
    /// <param name="defaultHotKeyCode">The key code of the default hot key.</param>
    /// <param name="settingsPageComponentType">The Blazor component type used to render the settings page.</param>
    public SettingsPageDescriptor(int order, string route, SvgIconType icon, string menuCaption, string tabCaption, string? hotKeyEntryName, ModCode defaultHotKeyModifiers, Code? defaultHotKeyCode, [DynamicallyAccessedMembers(All)] Type settingsPageComponentType)
    {
        this.Order = order;
        this.Route = route;
        this.Icon = icon;
        this.MenuCaption = menuCaption;
        this.TabCaption = tabCaption;
        this.HotKeyEntryName = hotKeyEntryName;
        this.DefaultHotKeyModifiers = defaultHotKeyModifiers;
        this.DefaultHotKeyCode = defaultHotKeyCode;
        this.ComponentType = settingsPageComponentType;
    }
}
