using BlazingStory.Internals.Addons;

namespace BlazingStory.Internals.Services.Addons;

public class AddonsStore
{
    #region Private Fields

    private readonly List<IAddonComponent> _Addons = new();

    #endregion Private Fields

    #region Internal Events

    internal event EventHandler? OnFrameArgumentsChanged;

    #endregion Internal Events

    #region Internal Methods

    internal void RegisterAddon(IAddonComponent addon)
    {
        this._Addons.Add(addon);
    }

    internal IEnumerable<IAddonComponent> GetAddons(AddonType addonType)
    {
        return this._Addons.Where(a => a.AddonType.HasFlag(addonType)).OrderBy(a => a.ToolbuttonOrder);
    }

    internal IReadOnlyDictionary<string, object?> GetFrameArguments(AddonType addonType)
    {
        return this._Addons
            .Where(a => a.AddonType.HasFlag(addonType))
            .SelectMany(a => a.FrameArguments)
            .Where(item => item.Value != null)
            .ToDictionary(item => item.Key, item => item.Value);
    }

    internal void FrameArgumentsHasChanged()
    {
        this.OnFrameArgumentsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion Internal Methods
}
