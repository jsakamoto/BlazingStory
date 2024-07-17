using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Icons;

public partial class Badge : ComponentBase
{
    #region Public Properties

    [Parameter]
    public string? Text { get; set; }

    #endregion Public Properties
}
