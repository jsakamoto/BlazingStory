namespace BlazingStory.Internals.Models;

public class AddonPanelDescriptor
{
    internal readonly string Name;

    internal string Badge = "";

    internal readonly Type PanelComponentType;

    internal event EventHandler? Updated;

    internal AddonPanelDescriptor(string name, Type panelComponentType)
    {
        this.Name = name;
        this.PanelComponentType = panelComponentType;
    }

    internal virtual void SetParameters(Story? story, IServiceProvider services) { }

    internal void NotifyUpdated()
    {
        this.Updated?.Invoke(this, EventArgs.Empty);
    }
}
