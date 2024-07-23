using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar.SearchAndHistory;

public partial class History : ComponentBase
{
    #region Public Properties

    [Parameter]
    public EventCallback ExitHistoryView { get; set; }

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private NavigationService? _NavigationService;

    private IEnumerable<NavigationListItem> _HistoryItems = Enumerable.Empty<NavigationListItem>();

    #endregion Private Fields

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        this._NavigationService = this.Services.GetRequiredService<NavigationService>();
        this._HistoryItems = await this._NavigationService.GetHistoryItemsAsync();
    }

    #endregion Protected Methods

    #region Private Methods

    private SvgIconType GetIconType(NavigationItemType itemType) => itemType switch
    {
        NavigationItemType.Component => SvgIconType.Component,
        NavigationItemType.Story => SvgIconType.Component,
        _ => SvgIconType.Document
    };

    private string GetCssClass(NavigationItemType itemType) => itemType switch
    {
        NavigationItemType.Component => "type-component",
        NavigationItemType.Story => "type-component",
        _ => "type-docs"
    };

    private async Task OnClickHistoryItem(NavigationListItem historyItem)
    {
        this._NavigationService?.NavigateTo(historyItem);
        await this.ExitHistoryView.InvokeAsync();
    }

    private async Task BackToComponents()
    {
        await this.ExitHistoryView.InvokeAsync();
    }

    private async Task ClearHistory()
    {
        if (this._NavigationService == null)
        {
            return;
        }

        await this._NavigationService.ClearHistoryAsync();
        await this.ExitHistoryView.InvokeAsync();
    }

    #endregion Private Methods
}
