using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Buttons;

public partial class SquareButton : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the on click.
    /// </summary>
    /// <value>
    /// The on click.
    /// </value>
    [Parameter]
    public EventCallback OnClick { get; set; }

    #endregion Public Properties
}
