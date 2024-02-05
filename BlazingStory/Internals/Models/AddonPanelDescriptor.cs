namespace BlazingStory.Internals.Models;

internal class AddonPanelDescriptor
{
    public readonly string Name;

    public string Badge = "";

    public readonly Type PanelComponentType;

    public AddonPanelDescriptor(string name, Type panelComponentType)
    {
        this.Name = name;
        this.PanelComponentType = panelComponentType;
    }

    public virtual void Initialize(Story story, IServiceProvider services) { }


}
