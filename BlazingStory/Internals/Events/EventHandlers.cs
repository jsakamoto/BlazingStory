using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Events;

[EditorBrowsable(EditorBrowsableState.Never)]
[EventHandler("onanimationend", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
public class EventHandlers
{
}
