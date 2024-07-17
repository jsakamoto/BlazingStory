using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Addons.Background;

public partial class BackgroundAddon : ComponentBase, IAddonComponent
{
    #region Public Properties

    [CascadingParameter]
    public AddonsStore AddonsStore { get; init; } = default!;

    public AddonType AddonType { get; } = AddonType.CanvasPage | AddonType.DocsPage;

    public int ToolbuttonOrder { get; } = 100;

    public RenderFragment? ToolbarContents => this._ToobarContentsRef?.ChildContent;

    public IReadOnlyDictionary<string, object?> FrameArguments => this._FrameArguments;

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Public Fields

    public readonly Dictionary<string, object?> _FrameArguments = new();

    #endregion Public Fields

    #region Private Fields

    private AddonToobarContents? _ToobarContentsRef;
    private HelperScript HelperScript = default!;

    private BackgroundMode _BackgroundMode = BackgroundMode.None;

    #endregion Private Fields

    #region Private Enums

    private enum BackgroundMode
    {
        None,
        Light,
        Dark
    }

    #endregion Private Enums

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

        var backgroudnModeStr = await this.HelperScript.GetLocalStorageItemAsync(StorageKeys.BackgroundMode);
        this._BackgroundMode = Enum.TryParse<BackgroundMode>(backgroudnModeStr, out var backgroundMode) ? backgroundMode : BackgroundMode.None;
        this.SetFrameArguments();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnClickBackground(BackgroundMode backgroundMode)
    {
        if (backgroundMode == BackgroundMode.None)
        {
            await Task.Delay(10);
        }

        this._BackgroundMode = backgroundMode;
        await this.HelperScript.SetLocalStorageItemAsync(StorageKeys.BackgroundMode, this._BackgroundMode.ToString());

        this.SetFrameArguments();
    }

    private void SetFrameArguments()
    {
        this._FrameArguments["backgrounds.value"] = this._BackgroundMode switch { BackgroundMode.Light => "#F8F8F8", BackgroundMode.Dark => "#333333", _ => "transparent" };
        this.AddonsStore.FrameArgumentsHasChanged();
    }

    #endregion Private Methods

    #region Private Classes

    private static class StorageKeys
    {
        #region Public Fields

        public const string BackgroundMode = "Addons.Background.Mode";

        #endregion Public Fields
    }

    #endregion Private Classes
}
