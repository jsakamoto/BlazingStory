namespace BlazingStory.Addons;

public interface IAddonBuilder
{
    void AddToolbarButton<TToolbarButtonComponent>(int order, Func<ViewMode, bool> match);
}
