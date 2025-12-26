using System.ComponentModel;
using BlazingStory.Internals.Events.Args;
using Microsoft.AspNetCore.Components;

namespace BlazingStory;

[EditorBrowsable(EditorBrowsableState.Never)]
[EventHandler("onframeheightchange", typeof(FrameHeightChangeEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("oncomponentaction", typeof(ComponentActionEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onintersectionchange", typeof(IntersectionChangeEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
public static class EventHandlers
{
}
