using System.Text.Json;
using BlazingStory.Internals.Services.Command;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Addons;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Test.Internals.Services.Command;

internal class CommandServiceTest
{
    /// <summary>
    /// The command keys before the settings pages became pluggable. (This is used to reproduce the saved JSON text.)
    /// </summary>
    private enum LegacyCommandType
    {
        AboutYourBlazingStory,
        ReleaseNotes,
        KeyboardShortcuts,
        SideBarVisible,
    }

    [Test]
    public async Task SettingsPage_Commands_Test()
    {
        // Given
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(builder => builder.Addons.Register<CustomSettingsPageAddon>());

        // When
        var commandService = ctx.Services.GetRequiredService<CommandService>();

        // Then
        commandService.Commands.Take(5).Select(cmd => $"{cmd.Key} | {cmd.Command.Title} | {cmd.Command.GetHotKeyName()}").Is(
            "settings/about | About your Blazing Story | ",
            "settings/release-notes | Release notes | ",
            "CustomSettings | Custom settings | alt ⌃ F2",
            "KeyboardShortcuts | Keyboard shortcuts | ⌃ ⇧ ,",
            "SideBarVisible | Show sidebar | alt S");
    }

    [Test]
    public async Task Load_Saved_HotKeys_Test()
    {
        // Given: the hot keys that were saved before the settings pages became pluggable
        var savedCommandStates = JsonSerializer.Serialize(new Dictionary<LegacyCommandType, CommandState>
        {
            [LegacyCommandType.KeyboardShortcuts] = new() { KeyMod = ModCode.Ctrl | ModCode.Alt, KeyCode = Code.K },
            [LegacyCommandType.SideBarVisible] = new() { KeyMod = ModCode.Alt, KeyCode = Code.B, Flag = false },
        }, new JsonSerializerOptions { IncludeFields = true });

        // When
        await using var ctx = await SettingsPagesTestHelper.CreateContextAsync(savedCommandStates: savedCommandStates);

        // Then
        var commandService = ctx.Services.GetRequiredService<CommandService>();
        commandService.Commands["KeyboardShortcuts"]!.GetHotKeyName().Is("alt ⌃ K");
        commandService[CommandType.SideBarVisible]!.GetHotKeyName().Is("alt B");
        commandService[CommandType.SideBarVisible]!.Flag.Is(false);
    }
}
