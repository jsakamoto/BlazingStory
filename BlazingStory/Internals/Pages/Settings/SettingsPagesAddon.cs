using BlazingStory.Addons;
using BlazingStory.Internals.Pages.Settings.Panels;
using BlazingStory.ToolKit.Icons;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Pages.Settings;

/// <summary>
/// Registers the built-in settings pages ("About", "Release notes", and "Keyboard shortcuts") with the addon builder.
/// </summary>
internal class SettingsPagesAddon : IAddon
{
    /// <summary>
    /// Registers the built-in settings pages with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register settings pages.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddSettingsPage<AboutPanel>(order: 100, route: "about", icon: SvgIconType.CircleInfo, menuCaption: "About your Blazing Story", tabCaption: "About");
        builder.AddSettingsPage<ReleaseNotesPanel>(order: 200, route: "release-notes", icon: SvgIconType.ReleaseNotes, menuCaption: "Release notes", tabCaption: "Release notes");
        builder.AddSettingsPage<KeyboardShortcutsPanel>(order: 300, route: "shortcuts", icon: SvgIconType.KeyboardShortcuts, menuCaption: "Keyboard shortcuts", tabCaption: "Keyboard shortcuts",
            hotKeyEntryName: "KeyboardShortcuts", defaultHotKeyModifiers: ModCode.Ctrl | ModCode.Shift, defaultHotKeyCode: Code.Comma);
    }
}
