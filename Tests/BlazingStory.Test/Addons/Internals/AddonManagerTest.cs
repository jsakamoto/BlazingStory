using BlazingStory.Addons;
using BlazingStory.Addons.Internals;
using BlazingStory.Test._Fixtures.Addons;
using BlazingStory.ToolKit.Icons;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Addons.Internals;

internal class AddonManagerTest
{
    [Test]
    public void GetSettingsPages_OrderedByOrder_Test()
    {
        // Given
        using var addonManager = new AddonManager();
        var builder = (IAddonBuilder)addonManager;
        builder.AddSettingsPage<CustomSettingsPage>(order: 300, route: "page-c", icon: SvgIconType.Gear, menuCaption: "Page C", tabCaption: "C");
        builder.AddSettingsPage<CustomSettingsPage>(order: 100, route: "page-a", icon: SvgIconType.Gear, menuCaption: "Page A", tabCaption: "A");
        builder.AddSettingsPage<CustomSettingsPage>(order: 200, route: "page-b", icon: SvgIconType.Gear, menuCaption: "Page B", tabCaption: "B");

        // When
        var settingsPages = addonManager.GetSettingsPages();

        // Then
        settingsPages.Select(page => page.Route).Is("page-a", "page-b", "page-c");
    }

    [Test]
    public void AddSettingsPage_Test()
    {
        // Given
        using var addonManager = new AddonManager();
        var builder = (IAddonBuilder)addonManager;

        // When
        builder.AddSettingsPage<CustomSettingsPage>(order: 100, route: "foo", icon: SvgIconType.Gear, menuCaption: "Foo menu", tabCaption: "Foo tab",
            hotKeyEntryName: "FooEntry", defaultHotKeyModifiers: ModCode.Ctrl, defaultHotKeyCode: Code.F2);

        // Then
        var settingsPage = addonManager.GetSettingsPages().Single();
        settingsPage.Order.Is(100);
        settingsPage.Route.Is("foo");
        settingsPage.Icon.Is(SvgIconType.Gear);
        settingsPage.MenuCaption.Is("Foo menu");
        settingsPage.TabCaption.Is("Foo tab");
        settingsPage.HotKeyEntryName.Is("FooEntry");
        settingsPage.DefaultHotKeyModifiers.Is(ModCode.Ctrl);
        settingsPage.DefaultHotKeyCode.Is(Code.F2);
        settingsPage.ComponentType.Is(typeof(CustomSettingsPage));
    }

    [Test]
    public void CommandKey_Test()
    {
        // Given
        using var addonManager = new AddonManager();
        var builder = (IAddonBuilder)addonManager;
        builder.AddSettingsPage<CustomSettingsPage>(order: 100, route: "foo", icon: SvgIconType.Gear, menuCaption: "Foo", tabCaption: "Foo");
        builder.AddSettingsPage<CustomSettingsPage>(order: 200, route: "bar", icon: SvgIconType.Gear, menuCaption: "Bar", tabCaption: "Bar", hotKeyEntryName: "BarEntry");

        // When
        var commandKeys = addonManager.GetSettingsPages().Select(page => page.CommandKey);

        // Then
        commandKeys.Is("settings/foo", "BarEntry");
    }
}
