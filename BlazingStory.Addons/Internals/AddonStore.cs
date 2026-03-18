namespace BlazingStory.Addons.Internals;

/// <summary>
/// The default implementation of <see cref="IAddonStore"/> that holds registered <see cref="IAddon"/> instances.
/// </summary>
internal class AddonStore : IAddonStore
{
    private readonly List<IAddon> _addon = [];

    /// <summary>
    /// Creates and stores a new instance of <typeparamref name="TAddon"/>.
    /// </summary>
    public IAddonStore Register<TAddon>() where TAddon : IAddon, new()
    {
        this._addon.Add(new TAddon());
        return this;
    }

    internal IEnumerable<IAddon> GetAddons() => this._addon;
}
