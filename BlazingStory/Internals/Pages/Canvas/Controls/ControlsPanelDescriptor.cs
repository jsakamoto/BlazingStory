using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Pages.Canvas.Controls;

internal class ControlsPanelDescriptor : AddonPanelDescriptor
{
    internal ControlsPanelDescriptor() : base("Controls", typeof(ControlsPanel))
    {
    }

    internal override void SetParameters(Story? story, IServiceProvider services)
    {
        base.SetParameters(story, services);

        var newBadge = story?.Context.GetNoEventParameterCount().ToString() ?? "";
        if (this.Badge != newBadge)
        {
            this.Badge = newBadge;
            this.NotifyUpdated();
        }
    }
}
