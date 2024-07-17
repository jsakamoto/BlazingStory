using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Internals.Components.Buttons;

public partial class CornerButton : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the class.
    /// </summary>
    /// <value>
    /// The class.
    /// </value>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the on click.
    /// </summary>
    /// <value>
    /// The on click.
    /// </value>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the content of the child.
    /// </summary>
    /// <value>
    /// The content of the child.
    /// </value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion Public Properties
}
