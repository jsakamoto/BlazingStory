using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Actions;

public partial class ActionsPanelLogLine : ComponentBase
{
    #region Public Properties

    [Parameter]
    public int Repeat { get; set; }

    [Parameter, EditorRequired]
    public string? Name { get; set; }

    [Parameter, EditorRequired]
    public JsonElement Value { get; set; }

    #endregion Public Properties

    #region Private Fields

    private bool _IsExpanded = false;

    #endregion Private Fields

    #region Private Methods

    private void OnClickNodeIcon()
    {
        this._IsExpanded = !this._IsExpanded;
    }

    #endregion Private Methods
}
