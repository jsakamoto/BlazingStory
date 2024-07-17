using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar;

public partial class SettingsMenu : ComponentBase
{
    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private IReadOnlyDictionary<bool, IEnumerable<Command>>? _CommandSegments;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        var commandService = this.Services.GetRequiredService<CommandService>();
        var topCommandsTypes = new[] {
            CommandType.AboutYourBlazingStory,
            CommandType.ReleaseNotes,
            CommandType.KeyboardShortcuts,
        };
        this._CommandSegments = commandService.Commands
            .GroupBy(cmd => topCommandsTypes.Contains(cmd.Type))
            .ToDictionary(g => g.Key, g => g.Select(a => a.Command).ToArray().AsEnumerable());
    }

    #endregion Protected Methods
}
