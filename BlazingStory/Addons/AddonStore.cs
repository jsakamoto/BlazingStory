namespace BlazingStory.Addons;

internal class AddonStore : IAddonStore
{
    private readonly List<IAddon> _addon = [];

    public IAddonStore Register<TAddon>() where TAddon : IAddon, new()
    {
        this._addon.Add(new TAddon());
        return this;
    }
}
