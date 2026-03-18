namespace BlazingStory.Addons;

/// <summary>
/// Provides a registration point for <see cref="IAddon"/> implementations.
/// </summary>
public interface IAddonStore
{
    /// <summary>
    /// Registers an addon of type <typeparamref name="TAddon"/> with the store.
    /// </summary>
    IAddonStore Register<TAddon>() where TAddon : IAddon, new();
}
