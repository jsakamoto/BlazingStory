using BlazingStory.Internals.Pages.Canvas;

namespace BlazingStory.Internals.Models;

public class AddonPanelDescriptor : IDisposable
{
    #region Internal Properties

    internal string Badge { get; private set; } = "";

    #endregion Internal Properties

    #region Internal Fields

    internal readonly string Name;
    internal readonly Type PanelComponentType;

    #endregion Internal Fields

    #region Internal Constructors

    internal AddonPanelDescriptor(string name, Type panelComponentType)
    {
        this.Name = name;
        this.PanelComponentType = panelComponentType;
    }

    #endregion Internal Constructors

    #region Internal Events

    internal event EventHandler? Updated;

    #endregion Internal Events

    #region Public Methods

    public void Dispose() => this.Dispose(true);

    #endregion Public Methods

    #region Internal Methods

    internal virtual void SetParameters(Story? story, IServiceProvider? services, CanvasPageContext? canvasPageContext)
    {
    }

    #endregion Internal Methods

    #region Protected Methods

    protected void UpdateBadge(string badge)
    {
        if (this.Badge == badge)
        {
            return;
        }

        this.Badge = badge;
        this.NotifyUpdated();
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    #endregion Protected Methods

    #region Private Methods

    private void NotifyUpdated()
    {
        this.Updated?.Invoke(this, EventArgs.Empty);
    }

    #endregion Private Methods
}
