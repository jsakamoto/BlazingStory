using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Internals.Components.ToolBar;

public partial class TabButton : ComponentBase
{
    #region Public Properties

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Active { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    #endregion Public Properties
}
