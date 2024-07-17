using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Buttons;

public partial class ResetButton : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the on click.
    /// </summary>
    /// <value>
    /// The on click.
    /// </value>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the class.
    /// </summary>
    /// <value>
    /// The class.
    /// </value>
    [Parameter]
    public string? Class { get; set; }

    #endregion Public Properties
}
