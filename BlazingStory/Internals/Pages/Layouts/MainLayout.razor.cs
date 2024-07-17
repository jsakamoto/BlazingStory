using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Pages.Layouts;

public partial class MainLayout : LayoutComponentBase
{
    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    [Inject] private NavigationManager? NavigationManager { get; set; }

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    private string SideBarSizeKeyName => this.GetType().Name + "." + nameof(_SideBarSize);

    #endregion Private Properties

    #region Private Fields

    private readonly Subscriptions _Subscriptions = new();
    private bool _Ready = false;

    private HelperScript HelperScript = default!;

    private int _SideBarSize = 210;
    private Command? _SideBarVisibleCommand;

    private Command? _GoFullScreenCommand;
    private SidBarVisibilityStates _SidBarVisibilityState = SidBarVisibilityStates.SidebarShown;

    #endregion Private Fields

    #region Private Enums

    private enum SidBarVisibilityStates
    {
        SidebarShowing,
        SidebarShown,
        SidebarHiding,
        SidebarHidden,
    }

    #endregion Private Enums

    #region Protected Methods

    protected override void OnInitialized()
    {
        this.HelperScript = this.Services.GetRequiredService<HelperScript>();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var commandService = await this.ConfigureCommandsAsync(this.Services);

            this._SideBarVisibleCommand = commandService[CommandType.SideBarVisible]!;
            this._GoFullScreenCommand = commandService[CommandType.GoFullScreen]!;
            this._Subscriptions.Add(this._SideBarVisibleCommand.Subscribe(this.OnToggleSideBarVisible));
            this._Subscriptions.Add(this._GoFullScreenCommand.Subscribe(this.OnToggleFullscreen));
            this._Subscriptions.Add(commandService.Subscribe(CommandType.AboutYourBlazingStory, () => { this.NavigationManager?.NavigateTo("./?path=/settings/about"); return ValueTask.CompletedTask; }));
            this._Subscriptions.Add(commandService.Subscribe(CommandType.ReleaseNotes, () => { this.NavigationManager?.NavigateTo("./?path=/settings/release-notes"); return ValueTask.CompletedTask; }));
            this._Subscriptions.Add(commandService.Subscribe(CommandType.KeyboardShortcuts, () => { this.NavigationManager?.NavigateTo("./?path=/settings/shortcuts"); return ValueTask.CompletedTask; }));
            this._SidBarVisibilityState = this.GetSidebarVisible() ? SidBarVisibilityStates.SidebarShown : SidBarVisibilityStates.SidebarHidden;

            this._Ready = true;
            this.StateHasChanged();

            await this.HelperScript.SetupKeyDownReceiverAsync();
            this._SideBarSize = await this.HelperScript.GetLocalStorageItemAsync(this.SideBarSizeKeyName, this._SideBarSize);
            this.StateHasChanged();
        }
        else
        {
            if (this._SidBarVisibilityState == SidBarVisibilityStates.SidebarHiding)
            {
                this._SidBarVisibilityState = SidBarVisibilityStates.SidebarHidden;
                this.StateHasChanged();
            }
            else if (this._SidBarVisibilityState == SidBarVisibilityStates.SidebarShowing)
            {
                await Task.Delay(100);
                this._SidBarVisibilityState = SidBarVisibilityStates.SidebarShown;
                this.StateHasChanged();
            }
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async ValueTask<CommandService> ConfigureCommandsAsync(IServiceProvider services)
    {
        var commandServce = services.GetRequiredService<CommandService>();
        await commandServce.Commands.EnsureInitializedAsync(() => new (CommandType, Command)[]
        {
            (CommandType.AboutYourBlazingStory, new(default, "About your Blazing Story")),
            (CommandType.ReleaseNotes, new(default, "Release notes")),
            (CommandType.KeyboardShortcuts, new(new(ModCode.Ctrl|ModCode.Shift, Code.Comma), "Keyboard shortcuts")),

            (CommandType.SideBarVisible,new(new(Code.S), "Show sidebar", flag: true)),
            (CommandType.ToolBarVisible,new(new(Code.T), "Show toolbar", flag: true)),
            (CommandType.AddonPanelVisible, new(new(Code.A), "Show addons", flag: true)),
            (CommandType.AddonPanelOrientation, new(new(Code.D), "Change addons orientation")),
            (CommandType.GoFullScreen, new(new(Code.F), "Go full screen", flag: false)),
            (CommandType.GoSearch, new(new(Code.Slash), "Search")),
            (CommandType.PreviousComponent, new(new(ModCode.Alt, Code.ArrowUp), "Previous component")),
            (CommandType.NextComponent, new(new(ModCode.Alt, Code.ArrowDown), "Next component")),
            (CommandType.PreviousStory, new(new(ModCode.Alt, Code.ArrowLeft), "Previous story")),
            (CommandType.NextStory, new(new(ModCode.Alt, Code.ArrowRight), "Next story")),
            (CommandType.CollapseAll, new(new(ModCode.Ctrl|ModCode.Shift, Code.ArrowUp), "Collapse all"))
        });

        return commandServce;
    }

    private async Task OnSideBarSizeChanged()
    {
        await this.HelperScript.SetLocalStorageItemAsync(this.SideBarSizeKeyName, _SideBarSize);
    }

    private ValueTask OnToggleSideBarVisible() => this.ToggleSideBarVisibliy(this._SideBarVisibleCommand);

    private ValueTask OnToggleFullscreen() => this.ToggleSideBarVisibliy(this._GoFullScreenCommand);

    private async ValueTask ToggleSideBarVisibliy(Command? command)
    {
        if (command == null)
        {
            return;
        }

        command.ToggleFlag();
        this._SidBarVisibilityState = this.GetSidebarVisible() ? SidBarVisibilityStates.SidebarShowing : SidBarVisibilityStates.SidebarHiding;
        await ValueTask.CompletedTask;
    }

    private bool GetSidebarVisible() => (this._SideBarVisibleCommand?.Flag ?? true) && !(this._GoFullScreenCommand?.Flag ?? false);

    #endregion Private Methods
}
