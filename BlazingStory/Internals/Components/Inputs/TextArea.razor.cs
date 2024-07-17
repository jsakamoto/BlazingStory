using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Inputs;

public partial class TextArea : ComponentBase
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the place holder.
    /// </summary>
    /// <value>
    /// The place holder.
    /// </value>
    [Parameter]
    public string? PlaceHolder { get; set; }

    /// <summary>
    /// Gets or sets the on input.
    /// </summary>
    /// <value>
    /// The on input.
    /// </value>
    [Parameter]
    public EventCallback<ChangeEventArgs> OnInput { get; set; }

    /// <summary>
    /// Gets or sets the on change.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    #endregion Public Properties

    #region Private Fields

    private string? _Value;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this._Value = this.Value;
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnTextAreaInput(ChangeEventArgs args)
    {
        this._Value = args.Value?.ToString();
        await this.OnInput.InvokeAsync(args);
    }

    private async Task OnTextAreaChange(ChangeEventArgs args)
    {
        this._Value = args.Value?.ToString();
        await this.OnChange.InvokeAsync(args);
    }

    #endregion Private Methods
}
