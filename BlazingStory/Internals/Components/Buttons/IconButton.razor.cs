using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Internals.Components.Buttons;

public partial class IconButton : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    [Parameter]
    public SvgIconType Icon { get; set; }

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
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the class.
    /// </summary>
    /// <value>
    /// The class.
    /// </value>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the command.
    /// </summary>
    /// <value>
    /// The command.
    /// </value>
    [Parameter]
    public Command? Command { get; set; }

    /// <summary>
    /// Gets or sets the href.
    /// </summary>
    /// <value>
    /// The href.
    /// </value>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the target.
    /// </summary>
    /// <value>
    /// The target.
    /// </value>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [flag style].
    /// </summary>
    /// <value>
    /// <c>true</c> if [flag style]; otherwise, <c>false</c>.
    /// </value>
    [Parameter]
    public bool FlagStyle { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IconButton" /> is active.
    /// </summary>
    /// <value>
    /// <c>true</c> if active; otherwise, <c>false</c>.
    /// </value>
    [Parameter]
    public bool Active { get; set; }

    #endregion Public Properties

    #region Private Methods

    private string GetCssClass()
    {
        var active = this.Active || (this.Command?.Flag ?? false == true);
        return $"{this.Class} {(active && this.FlagStyle ? "active" : "")}";
    }

    private string? GetTitleText()
    {
        if (string.IsNullOrEmpty(this.Title)) return this.Command?.GetTitleText();
        return string.Format(this.Title, this.Command?.GetHotKeyName());
    }

    private async Task ClickHandler(MouseEventArgs args)
    {
        var t1 = this.OnClick.InvokeAsync(args);
        if (this.Command != null) await this.Command.InvokeAsync();
        await t1;
    }

    #endregion Private Methods
}
