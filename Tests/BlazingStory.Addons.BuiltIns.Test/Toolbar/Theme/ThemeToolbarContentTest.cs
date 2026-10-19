using BlazingStory.Addons.BuiltIns.Toolbar.Theme;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Test.Toolbar.Theme;

public class ThemeToolbarContentTest
{
    [Test]
    public void ThemeToolbarContent_Render_OnlyOneColorSchemeAvailable_RendersNothing_Test()
    {
        // Given
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // When
        var cut = ctx.Render<CascadingValue<string>>(parameters => parameters
            .Add(p => p.Name, "AvailableColorSchemes")
            .Add(p => p.Value, "Light")
            .AddChildContent<ThemeToolbarContent>());

        // Then
        cut.Markup.Trim().Is("");
    }

    [Test]
    public void ThemeToolbarContent_Render_BothColorSchemesAvailable_RendersSelector_Test()
    {
        // Given
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // When
        var cut = ctx.Render<CascadingValue<string>>(parameters => parameters
            .Add(p => p.Name, "AvailableColorSchemes")
            .Add(p => p.Value, "Both")
            .AddChildContent<ThemeToolbarContent>());

        // Then
        cut.FindAll("[title='Change UI theme']").Count.Is(1);
    }
}
