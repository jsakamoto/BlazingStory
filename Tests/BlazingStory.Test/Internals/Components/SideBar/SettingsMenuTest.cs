using BlazingStory.Internals.Components.SideBar;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Addons;
using Bunit;

namespace BlazingStory.Test.Internals.Components.SideBar;

internal class SettingsMenuTest
{
    private static IEnumerable<string> GetMenuItems(IRenderedComponent<SettingsMenu> cut)
    {
        return cut.Find(".popup-menu-body").Children.Select(element => element.ClassList.Contains("menu-item-divider")
            ? "----"
            : element.QuerySelector(".command-title")!.TextContent + " | " + element.QuerySelector(".icon-area use")?.GetAttribute("href"));
    }

    [Test]
    public async Task Render_MenuItems_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync();
        var cut = ctx.Render<SettingsMenu>();

        // When
        cut.Find(".popup-menu-trigger-content").Click();

        // Then
        GetMenuItems(cut).Take(4).Is(
            "About your Blazing Story | #icon--circleinfo",
            "Release notes | #icon--releasenotes",
            "Keyboard shortcuts | #icon--keyboardshortcuts",
            "----");
        GetMenuItems(cut).TakeLast(2).Is(
            "----",
            "Documentation | #icon--document");
    }

    [Test]
    public async Task Render_MenuItems_with_CustomAddon_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(builder => builder.Addons.Register<CustomSettingsPageAddon>());
        var cut = ctx.Render<SettingsMenu>();

        // When
        cut.Find(".popup-menu-trigger-content").Click();

        // Then
        GetMenuItems(cut).Take(5).Is(
            "About your Blazing Story | #icon--circleinfo",
            "Release notes | #icon--releasenotes",
            "Custom settings | #icon--gear",
            "Keyboard shortcuts | #icon--keyboardshortcuts",
            "----");
    }
}
