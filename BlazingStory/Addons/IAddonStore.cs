namespace BlazingStory.Addons;

public interface IAddonStore
{
    IAddonStore Register<TAddon>() where TAddon : IAddon, new();
}
