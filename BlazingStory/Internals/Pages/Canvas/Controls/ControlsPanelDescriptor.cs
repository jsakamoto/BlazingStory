using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Pages.Canvas.Controls;

internal class ControlsPanelDescriptor : AddonPanelDescriptor
{
    public ControlsPanelDescriptor() : base("Controls", typeof(ControlsPanel))
    {
    }

    public override void Initialize(Story story, IServiceProvider services)
    {
        base.Initialize(story, services);
        this.Badge = story.Context.GetNoEventParameterCount().ToString();
    }
}
