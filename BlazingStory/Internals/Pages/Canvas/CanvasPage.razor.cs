using BlazingStory.Components;
using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Components.Preview;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Splitter;

namespace BlazingStory.Internals.Pages.Canvas;

public partial class CanvasPage : IDisposable
{
    #region Protected Properties

    [CascadingParameter]
    protected StoriesStore StoriesStore { get; init; } = default!;

    [CascadingParameter]
    protected QueryRouteData RouteData { get; init; } = default!;

    [CascadingParameter]
    protected AddonsStore AddonsStore { get; init; } = default!;

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    protected BlazingStoryApp BlazingStoryApp { get; init; } = default!;

    #endregion Protected Properties

    #region Private Properties

    [Inject] private NavigationManager? NavigationManager { get; set; }

    private bool AddonPanelVisible => (this._AddonPanelVisibleCommand?.Flag ?? true) && !(this._GoFullscreenCommand?.Flag ?? false);
    private string AddonPanelLayoutKeyName => this.GetType().Name + "." + nameof(this._AddonPanelLayout);

    private SvgIconType ToggleOrientationButtonIcon => this._AddonPanelLayout.SplitterOrientation switch
    {
        SplitterOrientation.Horizontal => SvgIconType.BottomSidePane,
        _ => SvgIconType.RightSidePane
    };

    #endregion Private Properties

    #region Private Fields

    private readonly Subscriptions _Subscriptions = new();
    private readonly CanvasPageContext _CanvasPageContext = new();
    private PreviewFrame? _PreviewFrame;

    private HelperScript HelperScript = default!;

    private CommandService _Commands = default!;

    private Command? _ToolbarVisibleCommand;

    private Command? _AddonPanelVisibleCommand;

    private Command? _GoFullscreenCommand;
    private int _AddonPanelSize = 210;

    private AddonPanelLayout _AddonPanelLayout = new();

    private string _PageTitle = "";

    private string _LastTitledParameter = "";

    private Story? _CurrentStory = null;
    private ComponentActionLogs? _ComponentActionLogs;

    #endregion Private Fields

    #region Public Methods

    public void Dispose()
    {
        this.AddonsStore.OnFrameArgumentsChanged -= this.AddonsStore_OnFrameArgumentsChanged;

        this._Subscriptions.Dispose();

        if (this._CurrentStory != null)
        {
            this._CurrentStory.Context.ArgumentChanged -= this.Context_ArgumentChanged;
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        this.HelperScript = this.Services.GetRequiredService<HelperScript>();
        this.AddonsStore.OnFrameArgumentsChanged += this.AddonsStore_OnFrameArgumentsChanged;

        this._Commands = this.Services.GetRequiredService<CommandService>();
        this._ToolbarVisibleCommand = this._Commands[CommandType.ToolBarVisible]!;
        this._AddonPanelVisibleCommand = this._Commands[CommandType.AddonPanelVisible]!;
        this._GoFullscreenCommand = this._Commands[CommandType.GoFullScreen];

        this._Subscriptions.Add(
            this._Commands.Subscribe(CommandType.AddonPanelOrientation, this.OnToggleOrientation),
            this._ToolbarVisibleCommand.Subscribe(this.OnToggleToolbarVisible),
            this._AddonPanelVisibleCommand.Subscribe(this.OnToggleAddonPanelVisible)
        );

        this._ComponentActionLogs = this.Services.GetRequiredService<ComponentActionLogs>();
        this._CanvasPageContext.SetItem(this._ComponentActionLogs);
    }

    protected override void OnParametersSet()
    {
        this.UpdateCurrentStory();
        this.UpdatePageTitle();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        this._AddonPanelLayout = await this.HelperScript.LoadObjectFromLocalStorageAsync(this.AddonPanelLayoutKeyName, this._AddonPanelLayout);

        this.UpdateAddonPanelSize(this._AddonPanelLayout.SplitterOrientation);
        this.StateHasChanged();
    }

    #endregion Protected Methods

    #region Private Methods

    private void UpdatePageTitle()
    {
        if (this._LastTitledParameter == this.RouteData.Parameter)
        {
            return;
        }

        if (this.StoriesStore.TryGetStoryByPath(this.RouteData.Parameter, out var story))
        {
            this._PageTitle = string.Join(" / ", story.Title.Split('/')) + " - " + story.Name + " - " + this.BlazingStoryApp.Title;
        }

        this._LastTitledParameter = this.RouteData.Parameter;
    }

    private void AddonsStore_OnFrameArgumentsChanged(object? sender, EventArgs args)
    {
        this.StateHasChanged();
    }

    private void UpdateCurrentStory()
    {
        if (this._CurrentStory != null)
        {
            this._CurrentStory.Context.ArgumentChanged -= this.Context_ArgumentChanged;
        }

        var path = this.RouteData.Parameter;

        if (this.StoriesStore.TryGetStoryByPath(path, out var story))
        {
            if (!object.ReferenceEquals(this._CurrentStory, story))
            {
                this._ComponentActionLogs?.Clear();
            }

            this._CurrentStory = story;
        }
        else
        {
            this._CurrentStory = null;
        }

        if (this._CurrentStory != null)
        {
            this._CurrentStory.Context.ArgumentChanged += this.Context_ArgumentChanged;
        }
    }

    private ValueTask Context_ArgumentChanged()
    {
        this.StateHasChanged();
        return ValueTask.CompletedTask;
    }

    private async Task OnAddonPanelSizeChanged()
    {
        if (this._AddonPanelLayout.SplitterOrientation == SplitterOrientation.Horizontal)
        {
            this._AddonPanelLayout.VerticalSize = this._AddonPanelSize;
        }
        else
        {
            this._AddonPanelLayout.HorizontalSize = this._AddonPanelSize;
        }

        await this.SaveAddonPanelLayoutAsync();
    }

    private void UpdateAddonPanelSize(SplitterOrientation splitterOrientation)
    {
        this._AddonPanelSize = splitterOrientation switch
        {
            SplitterOrientation.Vertical => this._AddonPanelLayout.HorizontalSize,
            _ => this._AddonPanelLayout.VerticalSize
        };
    }

    private async ValueTask OnToggleOrientation()
    {
        this._AddonPanelLayout.SplitterOrientation = this._AddonPanelLayout.SplitterOrientation switch
        {
            SplitterOrientation.Horizontal => SplitterOrientation.Vertical,
            _ => SplitterOrientation.Horizontal
        };
        this.UpdateAddonPanelSize(this._AddonPanelLayout.SplitterOrientation);
        await this.SaveAddonPanelLayoutAsync();
    }

    private async ValueTask OnToggleToolbarVisible()
    {
        this._ToolbarVisibleCommand?.ToggleFlag();
        await ValueTask.CompletedTask;
    }

    private async ValueTask OnToggleAddonPanelVisible()
    {
        this._AddonPanelVisibleCommand?.ToggleFlag();
        await ValueTask.CompletedTask;
    }

    private async Task OnClickCopyCanvasLink()
    {
        if (this._PreviewFrame is null || this.NavigationManager is null)
        {
            return;
        }

        var canvalLinkUrl = this._PreviewFrame.CurrentPreviewFrameUrl.TrimStart('.', '/');
        var absoluteCanvalLinkUrl = this.NavigationManager.ToAbsoluteUri(canvalLinkUrl);

        await this.HelperScript.CopyTextToClipboardAsync(absoluteCanvalLinkUrl.AbsoluteUri);
    }

    private async Task OnClickReload()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ReloadAsync();
        }
    }

    private async Task OnClickZoomIn()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ZoomInAsync();
        }
    }

    private async Task OnClickZoomOut()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ZoomOutAsync();
        }
    }

    private async Task OnClickResetZoom()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ResetZoomAsync();
        }
    }

    private async ValueTask SaveAddonPanelLayoutAsync()
    {
        await this.HelperScript.SaveObjectToLocalStorageAsync(this.AddonPanelLayoutKeyName, this._AddonPanelLayout);
    }

    private void OnComponentAction(ComponentActionEventArgs e)
    {
        this._ComponentActionLogs?.Add(e.Name, e.ArgsJson);
    }

    #endregion Private Methods
}
