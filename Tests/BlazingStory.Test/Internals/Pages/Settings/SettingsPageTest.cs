using BlazingStory.Internals.Models;
using BlazingStory.Internals.Pages.Settings;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Addons;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Test.Internals.Pages.Settings;

internal class SettingsPageTest
{
    [Test]
    public async Task Render_Tabs_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync();

        // When
        var cut = ctx.Render<SettingsPage>(p => p.Add(x => x.RouteData, new QueryRouteData("settings", "shortcuts")));

        // Then
        cut.FindAll(".tab-button").Select(tab => tab.TextContent.Trim()).Is("About", "Release notes", "Keyboard shortcuts");
        cut.FindAll(".tab-button.active").Select(tab => tab.TextContent.Trim()).Is("Keyboard shortcuts");
    }

    [Test]
    public async Task Render_Tabs_with_CustomAddon_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(builder => builder.Addons.Register<CustomSettingsPageAddon>());

        // When
        var cut = ctx.Render<SettingsPage>(p => p.Add(x => x.RouteData, new QueryRouteData("settings", "custom")));

        // Then
        cut.FindAll(".tab-button").Select(tab => tab.TextContent.Trim()).Is("About", "Release notes", "Custom", "Keyboard shortcuts");
        cut.FindAll(".tab-button.active").Select(tab => tab.TextContent.Trim()).Is("Custom");
        cut.Find(".custom-settings-page").TextContent.Is("This is a custom settings page");
    }

    [Test]
    public async Task Click_Tab_Navigates_to_SettingsPage_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(builder => builder.Addons.Register<CustomSettingsPageAddon>());
        var cut = ctx.Render<SettingsPage>(p => p.Add(x => x.RouteData, new QueryRouteData("settings", "about")));

        // When
        cut.FindAll(".tab-button").Single(tab => tab.TextContent.Trim() == "Custom").Click();

        // Then
        var navigationManager = ctx.Services.GetRequiredService<NavigationManager>();
        navigationManager.Uri.Is("http://localhost/?path=/settings/custom");
    }
}
