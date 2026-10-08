using BlazingStory.Addons;
using BlazingStory.ToolKit.Icons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test._Fixtures.Addons;

/// <summary>
/// An addon that adds a custom settings page, like an app's own addon does.
/// </summary>
internal class CustomSettingsPageAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddSettingsPage<CustomSettingsPage>(order: 250, route: "custom", icon: SvgIconType.Gear, menuCaption: "Custom settings", tabCaption: "Custom",
            hotKeyEntryName: "CustomSettings", defaultHotKeyModifiers: ModCode.Ctrl | ModCode.Alt, defaultHotKeyCode: Code.F2);
    }
}

internal class CustomSettingsPage : ComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "custom-settings-page");
        builder.AddContent(2, "This is a custom settings page");
        builder.CloseElement();
    }
}
