namespace BlazingStory.Addons.Internals;

internal class AddonStore : IAddonStore
{
    private readonly List<IAddon> _addon = [];

    public IAddonStore Register<TAddon>() where TAddon : IAddon, new()
    {
        this._addon.Add(new TAddon());
        return this;
    }

    internal IEnumerable<IAddon> GetAddons() => this._addon;
}
