using BlazingStory.Internals.Pages.Canvas;

namespace BlazingStory.Internals.Models;

public class AddonPanelDescriptor : IDisposable
{
    internal readonly string Name;

    internal string Badge { get; private set; } = "";

    internal readonly Type PanelComponentType;

    internal event EventHandler? Updated;

    internal AddonPanelDescriptor(string name, Type panelComponentType)
    {
        this.Name = name;
        this.PanelComponentType = panelComponentType;
    }

    internal virtual void SetParameters(Story? story, IServiceProvider services, CanvasPageContext canvasPageContext) { }

    private void NotifyUpdated()
    {
        this.Updated?.Invoke(this, EventArgs.Empty);
    }

    protected void UpdateBadge(string badge)
    {
        if (this.Badge == badge) return;
        this.Badge = badge;
        this.NotifyUpdated();
    }

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool disposing) { }
}
