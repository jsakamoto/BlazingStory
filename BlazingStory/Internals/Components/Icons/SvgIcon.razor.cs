using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Internals.Components.Icons;

public partial class SvgIcon : ComponentBase
{
    #region Public Properties

    [Parameter]
    public SvgIconType Type { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    #endregion Public Properties
}
