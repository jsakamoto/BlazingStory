using BlazingStory.Internals.Pages.Canvas;

namespace BlazingStory.Internals.Models;

public class AddonPanelDescriptor : IDisposable
{
    internal string Badge { get; private set; } = "";
    internal readonly string Name;
    internal readonly Type PanelComponentType;

    internal AddonPanelDescriptor(string name, Type panelComponentType)
    {
        this.Name = name;
        this.PanelComponentType = panelComponentType;
    }

    internal event EventHandler? Updated;

    public void Dispose() => this.Dispose(true);

    internal virtual void SetParameters(Story? story, IServiceProvider? services, CanvasPageContext? canvasPageContext)
    {
    }

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

    private void NotifyUpdated()
    {
        this.Updated?.Invoke(this, EventArgs.Empty);
    }
}
