using Microsoft.AspNetCore.Components;

namespace BlazingStory.Components;

/// <summary>
/// The default implementation of the brand logo of the Blazing Story app.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
public partial class BrandLogo : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// The title of the branding logo.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// The URL of the link applied to the branding logo title text.
    /// </summary>
    [Parameter]
    public string? Url { get; set; } = "./";

    /// <summary>
    /// The icon URL of the branding logo.
    /// </summary>
    [Parameter]
    public string? IconUrl { get; set; } = "../images/icon.min.svg";

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected BlazingStoryApp BlazingStoryApp { get; init; } = default!;

    #endregion Protected Properties
}
