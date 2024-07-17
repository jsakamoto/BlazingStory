using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Addons;

public partial class AddonToobarContents : ComponentBase
{
    #region Public Properties

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion Public Properties
}
