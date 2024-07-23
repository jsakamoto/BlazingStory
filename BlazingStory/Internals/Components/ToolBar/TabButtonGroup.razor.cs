using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.ToolBar;

public partial class TabButtonGroup : ComponentBase
{
    #region Public Properties

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion Public Properties
}
