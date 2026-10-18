using BlazingStory.Addons.Internals;
using BlazingStory.Internals.Pages.Settings;
using BlazingStory.Internals.Pages.Settings.Panels;
using BlazingStory.ToolKit.Icons;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Internals.Pages.Settings;

internal class SettingsPagesAddonTest
{
    [Test]
    public void Initialize_Test()
    {
        // Given
        var addonStore = new AddonStore();
        addonStore.Register<SettingsPagesAddon>();
        using var addonManager = new AddonManager();

        // When
        addonManager.Initialize(addonStore);

        // Then
        var settingsPages = addonManager.GetSettingsPages().ToArray();
        settingsPages.Select(page => (page.Order, page.Route, page.Icon, page.MenuCaption, page.TabCaption, page.HotKeyEntryName, page.ComponentType)).Is(
            (100, "about", SvgIconType.CircleInfo, "About your Blazing Story", "About", null, typeof(AboutPanel)),
            (200, "release-notes", SvgIconType.ReleaseNotes, "Release notes", "Release notes", null, typeof(ReleaseNotesPanel)),
            (300, "shortcuts", SvgIconType.KeyboardShortcuts, "Keyboard shortcuts", "Keyboard shortcuts", "KeyboardShortcuts", typeof(KeyboardShortcutsPanel)));

        settingsPages[2].DefaultHotKeyModifiers.Is(ModCode.Ctrl | ModCode.Shift);
        settingsPages[2].DefaultHotKeyCode.Is(Code.Comma);
    }
}
