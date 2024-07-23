using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Addons;

public partial class CommonAddonComponent : ComponentBase, IAddonComponent
{
    #region Public Properties

    [CascadingParameter]
    public AddonsStore AddonsStore { get; init; } = default!;

    [CascadingParameter]
    public IServiceProvider Services { get; init; } = default!;

    [Parameter, EditorRequired]
    public SvgIconType Icon { get; set; }

    [Parameter, EditorRequired]
    public string? Title { get; set; }

    [Parameter, EditorRequired]
    public string StorageKey { get; set; } = default!;

    [Parameter, EditorRequired]
    public string FrameArgumentName { get; set; } = default!;

    [Parameter, EditorRequired]
    public int ToolbuttonOrder { get; set; }

    [Parameter, EditorRequired]
    public AddonType AddonType { get; set; }

    public RenderFragment? ToolbarContents => this._ToobarContentsRef?.ChildContent;
    public IReadOnlyDictionary<string, object?> FrameArguments => this._FrameArguments;

    #endregion Public Properties

    #region Private Fields

    private readonly Dictionary<string, object?> _FrameArguments = new();
    private HelperScript HelperScript = default!;

    private bool _Enable = false;

    private AddonToobarContents? _ToobarContentsRef;

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

        var enableStr = await this.HelperScript.GetLocalStorageItemAsync(this.StorageKey);
        this._Enable = bool.TryParse(enableStr, out var enable) ? enable : this._Enable;
        this.SetFrameArguments();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnClickButton()
    {
        this._Enable = !this._Enable;
        this.SetFrameArguments();
        await this.HelperScript.SetLocalStorageItemAsync(this.StorageKey, this._Enable);
    }

    private void SetFrameArguments()
    {
        this._FrameArguments[this.FrameArgumentName] = this._Enable ? "true" : null;
        this.AddonsStore.FrameArgumentsHasChanged();
    }

    #endregion Private Methods
}
