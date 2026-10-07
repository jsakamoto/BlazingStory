using BlazingStory.Components;
using BlazingStory.Internals.Pages.Settings.Panels;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Addons;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Pages.Settings.Panels;

internal class KeyboardShortcutsPanelTest
{
    [Test]
    public async Task Render_Commands_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync();
        ctx.RenderTree.Add<CascadingValue<BlazingStoryApp>>(p => p.Add(p => p.Value, new BlazingStoryApp()));

        // When
        var cut = ctx.Render<KeyboardShortcutsPanel>();

        // Then
        cut.FindAll(".command-row .command-name").Select(element => element.TextContent).Is(
            "Keyboard shortcuts",
            "Show sidebar",
            "Show toolbar",
            "Show addons panel",
            "Change addons orientation",
            "Go full screen",
            "Search",
            "Previous component",
            "Next component",
            "Previous story",
            "Next story",
            "Collapse all");
    }

    [Test]
    public async Task Render_Commands_with_CustomAddon_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(builder => builder.Addons.Register<CustomSettingsPageAddon>());
        ctx.RenderTree.Add<CascadingValue<BlazingStoryApp>>(p => p.Add(p => p.Value, new BlazingStoryApp()));

        // When
        var cut = ctx.Render<KeyboardShortcutsPanel>();

        // Then
        cut.FindAll(".command-row .command-name").Select(element => element.TextContent).Take(3).Is(
            "Custom settings",
            "Keyboard shortcuts",
            "Show sidebar");
    }
}
