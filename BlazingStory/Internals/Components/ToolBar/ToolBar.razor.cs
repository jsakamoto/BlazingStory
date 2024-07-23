using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.ToolBar;

public partial class ToolBar : ComponentBase
{
    #region Public Properties

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public bool Visible { get; set; } = true;

    #endregion Public Properties
}
