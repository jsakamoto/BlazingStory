using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Menus;

public partial class MenuItem : ComponentBase
{
    #region Public Properties

    [Parameter]
    public EventCallback OnClick { get; set; }

    [Parameter]
    public bool Active { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion Public Properties
}
