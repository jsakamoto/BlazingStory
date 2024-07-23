using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;

namespace BlazingStory.Internals.Pages.Canvas.Actions;

internal class ActionsPanelDescriptor : AddonPanelDescriptor
{
    #region Private Fields

    private ComponentActionLogs? _ActionLogs;

    #endregion Private Fields

    #region Internal Constructors

    internal ActionsPanelDescriptor() : base("Actions", typeof(ActionsPanel))
    {
    }

    #endregion Internal Constructors

    #region Internal Methods

    internal override void SetParameters(Story? story, IServiceProvider? services, CanvasPageContext? canvasPageContext)
    {
        base.SetParameters(story, services, canvasPageContext);
        this.UnsubscribeActionLogsEvent();
        this._ActionLogs = canvasPageContext?.GetRequiredItem<ComponentActionLogs>();

        if (this._ActionLogs != null)
        {
            this._ActionLogs.Updated += this.ActionLogs_Updated;
        }
    }

    #endregion Internal Methods

    #region Protected Methods

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        this.UnsubscribeActionLogsEvent();
    }

    #endregion Protected Methods

    #region Private Methods

    private void ActionLogs_Updated(object? sender, EventArgs e)
    {
        var numberOfActions = this._ActionLogs?.Sum(log => log.Repeat) ?? 0;
        this.UpdateBadge(numberOfActions == 0 ? "" : numberOfActions.ToString());
    }

    private void UnsubscribeActionLogsEvent()
    {
        if (this._ActionLogs != null)
        {
            this._ActionLogs.Updated -= this.ActionLogs_Updated;
            this._ActionLogs = null;
        }
    }

    #endregion Private Methods
}
