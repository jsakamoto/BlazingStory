using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Addons.Theme;

public partial class ThemeAddon<TEnum> : ComponentBase, IAddonComponent where TEnum : struct, Enum
{
    #region Public Properties

    [CascadingParameter]
    public AddonsStore AddonsStore { get; init; } = default!;

    public AddonType AddonType { get; } = AddonType.CanvasPage | AddonType.DocsPage;

    public int ToolbuttonOrder { get; } = 600;

    public RenderFragment? ToolbarContents => this._ToobarContentsRef?.ChildContent;

    public IReadOnlyDictionary<string, object?> FrameArguments => this._FrameArguments;

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Public Fields

    public readonly Dictionary<string, object?> _FrameArguments = [];

    #endregion Public Fields

    #region Private Fields

    private AddonToobarContents? _ToobarContentsRef;
    private HelperScript HelperScript = default!;

    private TEnum _ThemeMode = default;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        this.HelperScript = this.Services.GetRequiredService<HelperScript>();
        this.AddonsStore.RegisterAddon(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        var themeModeStr = await this.HelperScript.GetLocalStorageItemAsync(StorageKeys.ThemeMode);
        if (Enum.TryParse<TEnum>(themeModeStr, out var themeMode))
        {
            this._ThemeMode = themeMode;
        }
        else
        {
            // Provide a sensible default value for _ThemeMode
            this._ThemeMode = default;
        }

        this.SetFrameArguments();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnClickBackground(TEnum theme)
    {
        this._ThemeMode = theme;
        await this.HelperScript.SetLocalStorageItemAsync(StorageKeys.ThemeMode, this._ThemeMode.ToString());

        this.SetFrameArguments();
    }

    private void SetFrameArguments()
    {
        this._FrameArguments["theme.value"] = this._ThemeMode.ToString();
        this.AddonsStore.FrameArgumentsHasChanged();
    }

    #endregion Private Methods

    #region Private Classes

    private static class StorageKeys
    {
        #region Public Fields

        public const string ThemeMode = "Addons.Theme.Mode";

        #endregion Public Fields
    }

    #endregion Private Classes
}
