using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Addons.ChangeSize;

public partial class ChangeSizeAddon : ComponentBase, IAddonComponent
{
    #region Public Properties

    [CascadingParameter]
    public AddonsStore AddonsStore { get; init; } = default!;

    public AddonType AddonType { get; } = AddonType.CanvasPage;

    public int ToolbuttonOrder { get; } = 300;

    public RenderFragment? ToolbarContents => this._ToobarContentsRef?.ChildContent;

    public IReadOnlyDictionary<string, object?> FrameArguments { get; } = new Dictionary<string, object?>();

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    private bool Active => this._CurrentState.Size != SizeType.None;
    private string DisplayName => this.SizeTypeToDetail[this._CurrentState.Size].DisplayName;

    #endregion Private Properties

    #region Private Fields

    private const string StorageKey = "Addons.ChangeSize.State";

    private readonly IReadOnlyDictionary<SizeType, (string DisplayName, int ShrotSide, int LongSide)> SizeTypeToDetail = new Dictionary<SizeType, (string, int, int)>
    {
        [SizeType.None] = ("", 0, 0),
        [SizeType.SmallMobile] = ("Small mobile", 320, 568),
        [SizeType.LargeMobile] = ("Large mobile", 414, 896),
        [SizeType.Tablet] = ("Tablet", 834, 1112),
    };

    private AddonToobarContents? _ToobarContentsRef;
    private HelperScript HelperScript = default!;
    private SizeState _CurrentState = new();

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

        this._CurrentState = await this.HelperScript.LoadObjectFromLocalStorageAsync(StorageKey, this._CurrentState);
    }

    #endregion Protected Methods

    #region Private Methods

    private (int Size1, int Size2) GetSizes()
    {
        var sizeDetail = this.SizeTypeToDetail[this._CurrentState.Size];
        var orientation = this._CurrentState.Orientation;

        return (
            Size1: orientation == Orientation.Portrait ? sizeDetail.ShrotSide : sizeDetail.LongSide,
            Size2: orientation == Orientation.Portrait ? sizeDetail.LongSide : sizeDetail.ShrotSide
        );
    }

    private async Task OnClickSize(SizeType size)
    {
        if (size == SizeType.None)
        {
            await Task.Delay(10);
        }

        this._CurrentState.Size = size;
        await this.UpdateSize();
    }

    private async Task OnClickRotateViewport()
    {
        this._CurrentState.Orientation = this._CurrentState.Orientation == Orientation.Portrait ? Orientation.Landscape : Orientation.Portrait;
        await this.UpdateSize();
    }

    private async ValueTask UpdateSize()
    {
        await this.HelperScript.SaveObjectToLocalStorageAsync(StorageKey, this._CurrentState);
        this.AddonsStore.FrameArgumentsHasChanged();
    }

    #endregion Private Methods

    #region Private Classes

    private class SizeState
    {
        #region Public Fields

        public SizeType Size = SizeType.None;
        public Orientation Orientation = Orientation.Portrait;

        #endregion Public Fields
    }

    #endregion Private Classes
}
