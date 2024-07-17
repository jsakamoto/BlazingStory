using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Pages.Canvas.Controls;

internal class ControlsPanelDescriptor : AddonPanelDescriptor
{
    #region Internal Constructors

    internal ControlsPanelDescriptor() : base("Controls", typeof(ControlsPanel))
    {
    }

    #endregion Internal Constructors

    #region Internal Methods

    internal override void SetParameters(Story? story, IServiceProvider? services, CanvasPageContext? canvasPageContext)
    {
        base.SetParameters(story, services, canvasPageContext);

        var newBadge = story?.Context.GetNoEventParameterCount().ToString() ?? "";
        this.UpdateBadge(newBadge);
    }

    #endregion Internal Methods
}
