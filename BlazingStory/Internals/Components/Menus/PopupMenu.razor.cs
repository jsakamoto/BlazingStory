using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Components.Menus;

public partial class PopupMenu : ComponentBase, IAsyncDisposable
{
    #region Public Properties

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public int MarginTop { get; set; } = 0;

    [Parameter]
    public RenderFragment? Trigger { get; set; }

    [Parameter]
    public RenderFragment? MenuItems { get; set; }

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    private string _PopupStylesString => $"margin-top:{this.MarginTop}px;";

    #endregion Private Properties

    #region Private Fields

    private readonly JSModule _JSModule;
    private HotKeysContext? _HotKeysContext;

    private ElementReference _PopupMenuElement;

    private bool _PopupShown = false;
    private DotNetObjectReference<PopupMenu> _This;

    private IJSObjectReference? _EventSubscriber;

    #endregion Private Fields

    #region Public Constructors

    public PopupMenu()
    {
        this._This = DotNetObjectReference.Create(this);
        this._JSModule = new(() => this.JSRuntime, "Internals/Components/Menus/PopupMenu.razor.js");
    }

    #endregion Public Constructors

    #region Public Methods

    [JSInvokable(nameof(ClosePopup))]
    public async ValueTask ClosePopup()
    {
        await this.UnsubscribeEventAsync();

        if (this._PopupShown == false)
        {
            return;
        }

        this._PopupShown = false;
        this.StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        await this.UnsubscribeEventAsync();
        await this._JSModule.DisposeAsync();
        this._This?.Dispose();
    }

    #endregion Public Methods

    #region Private Methods

    private async Task OpenPopup()
    {
        if (this._PopupShown)
        {
            await this.ClosePopup();
            return;
        }

        this._EventSubscriber = await this._JSModule.InvokeAsync<IJSObjectReference>("subscribeDocumentEvent", "pointerdown", this._This, nameof(ClosePopup), this._PopupMenuElement);
        this._HotKeysContext = this.Services.GetRequiredService<HotKeys>().CreateContext().Add(Code.Escape, this.ClosePopup);
        this._PopupShown = true;
    }

    private async ValueTask UnsubscribeEventAsync()
    {
        if (this._HotKeysContext is not null)
        {
            await this._HotKeysContext.DisposeAsync();
        }

        this._HotKeysContext = null;

        await this._EventSubscriber.DisposeIfConnectedAsync("dispose");
        this._EventSubscriber = null;
    }

    #endregion Private Methods
}
