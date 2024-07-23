using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Components.SideBar.SearchAndHistory;

public partial class SearchAndHistory : ComponentBase, IAsyncDisposable
{
    #region Public Properties

    [Parameter]
    public bool SearchMode { get; set; } = false;

    [Parameter]
    public EventCallback<bool> SearchModeChanged { get; set; }

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private string? _SearchText;

    private HotKeysContext? _HotKeysContext;

    #endregion Private Fields

    #region Public Methods

    public async ValueTask DisposeAsync()
    {
        if (this._HotKeysContext is not null)
        {
            await this._HotKeysContext.DisposeAsync();
        }
    }

    #endregion Public Methods

    #region Private Methods

    private async Task OnFocus()
    {
        this.SearchMode = true;
        await this.SearchModeChanged.InvokeAsync(this.SearchMode);

        if (this._HotKeysContext is not null)
        {
            await this._HotKeysContext.DisposeAsync();
        }

        this._HotKeysContext = this.Services
            .GetRequiredService<HotKeys>()
            .CreateContext()
            .Add(Code.Escape, this.OnEscapeKeyPressed, exclude: Exclude.None);
    }

    private async Task OnCleared()
    {
        await this.ExitSearchMode();
    }

    private async ValueTask OnEscapeKeyPressed()
    {
        if (!string.IsNullOrWhiteSpace(this._SearchText))
        {
            this._SearchText = "";
            this.StateHasChanged();
            return;
        }

        this._SearchText = "";
        await this.ExitSearchMode();
    }

    private async Task ExitSearchMode()
    {
        this.SearchMode = false;

        if (this._HotKeysContext is not null)
        {
            await this._HotKeysContext.DisposeAsync();
        }

        this._HotKeysContext = null;
        await this.SearchModeChanged.InvokeAsync(this.SearchMode);

        var helperScript = this.Services.GetRequiredService<HelperScript>();
        await helperScript.InvokeVoidAsync("releaseFocus");
    }

    #endregion Private Methods
}
