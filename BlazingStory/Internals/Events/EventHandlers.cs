using System.ComponentModel;
using BlazingStory.Internals.Events.Args;
using Microsoft.AspNetCore.Components;

namespace BlazingStory;

[EditorBrowsable(EditorBrowsableState.Never)]
[EventHandler("ontransitionend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onanimationend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onframeheightchange", typeof(FrameHeightChangeEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("onintersectionchange", typeof(IntersectionChangeEventArgs), enableStopPropagation: true, enablePreventDefault: false)]
public static class EventHandlers
{
}
