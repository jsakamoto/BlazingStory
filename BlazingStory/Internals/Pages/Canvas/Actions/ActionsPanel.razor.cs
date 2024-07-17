using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Actions;

public partial class ActionsPanel : ComponentBase, IDisposable
{
    #region Public Properties

    [Parameter]
    public Story? Story { get; set; }

    [CascadingParameter]
    public CanvasPageContext? CanvasPageContext { get; init; }

    #endregion Public Properties

    #region Private Fields

    private ComponentActionLogs? _ActionLogs;

    #endregion Private Fields

    #region Public Methods

    public void Dispose()
    {
        if (this._ActionLogs != null)
        {
            this._ActionLogs.Updated -= this.ActionLogs_Updated;
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        this._ActionLogs = this.CanvasPageContext?.GetRequiredItem<ComponentActionLogs>();

        if (this._ActionLogs != null)
        {
            this._ActionLogs.Updated += this.ActionLogs_Updated;
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void ActionLogs_Updated(object? sender, EventArgs e)
    {
        this.StateHasChanged();
    }

    private void OnClickClear()
    {
        this._ActionLogs?.Clear();
    }

    #endregion Private Methods
}
