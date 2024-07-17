using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Pages.Canvas.Actions;
using BlazingStory.Internals.Pages.Canvas.Controls;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Splitter;

namespace BlazingStory.Internals.Pages.Canvas;

public partial class AddOnPanel : ComponentBase, IDisposable
{
    #region Public Properties

    [Parameter, EditorRequired]
    public Story? Story { get; set; }

    [Parameter]
    public SplitterOrientation SplitterOrientation { get; set; }

    [CascadingParameter]
    public IServiceProvider? Services { get; init; }

    [CascadingParameter]
    public CanvasPageContext? CanvasPageContext { get; init; }

    #endregion Public Properties

    #region Private Properties

    private SvgIconType ToggleOrientationButtonIcon => this.SplitterOrientation switch
    {
        SplitterOrientation.Horizontal => SvgIconType.RightSidePane,
        _ => SvgIconType.BottomSidePane
    };

    #endregion Private Properties

    #region Private Fields

    private readonly Dictionary<string, object?> _AddonPanelParameters = new();

    private readonly IEnumerable<AddonPanelDescriptor> _AddonPanelDescriptors = new AddonPanelDescriptor[]
    {
        new ControlsPanelDescriptor(),
        new ActionsPanelDescriptor(),
        // TODO: Not implemented yet.
        //new InteractionsPanelDescriptor()
    };

    private CommandService? _Commands;
    private AddonPanelDescriptor? _ActiveAddonPanelDescriptor;

    #endregion Private Fields

    #region Public Methods

    public void Dispose()
    {
        this._AddonPanelDescriptors.ForEach(descriptor =>
        {
            descriptor.Updated -= this.Descriptor_Updated;
            descriptor.Dispose();
        });
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnParametersSet()
    {
        this._AddonPanelParameters["Story"] = this.Story;
        this._AddonPanelDescriptors.ForEach(descriptor => descriptor.SetParameters(this.Story, this.Services, this.CanvasPageContext));
    }

    protected override void OnInitialized()
    {
        this._Commands = this.Services?.GetRequiredService<CommandService>();
        this._ActiveAddonPanelDescriptor = this._AddonPanelDescriptors.First();
        this._AddonPanelDescriptors.ForEach(descriptor => descriptor.Updated += this.Descriptor_Updated);
    }

    #endregion Protected Methods

    #region Private Methods

    private void Descriptor_Updated(object? sender, EventArgs args)
    {
        this.StateHasChanged();
    }

    private void OnSelectAddonPanel(AddonPanelDescriptor descriptor)
    {
        this._ActiveAddonPanelDescriptor = descriptor;
    }

    #endregion Private Methods
}
