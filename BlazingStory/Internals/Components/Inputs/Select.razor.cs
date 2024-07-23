using BlazingStory.Internals.Extensions;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Inputs;

public partial class Select : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public object? Value { get; set; }

    [Parameter, EditorRequired]
    public Array? Items { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    #endregion Public Properties

    #region Private Fields

    private IEnumerable<(bool Selected, string? Text)> _Options = Enumerable.Empty<(bool, string?)>();

    private bool _NoSelected = false;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        this._Options = (this.Items ?? Array.Empty<object?>())
            .Select(item => (Selected: this.Value?.Equals(item) == true, Text: item?.ToString()))
            .ToArray();

        this._NoSelected = !this._Options.Any(item => item.Selected);
    }

    #endregion Protected Methods
}
