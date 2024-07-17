using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar.SearchAndHistory;

public partial class SearchResult : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public string? SearchText { get; set; }

    [Parameter]
    public EventCallback ExitSearchView { get; set; }

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private NavigationService? _NavigationService;

    private IEnumerable<string> _SearchKeywords = Enumerable.Empty<string>();

    private IEnumerable<NavigationListItem> _Results = Enumerable.Empty<NavigationListItem>();

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        this._NavigationService = this.Services.GetRequiredService<NavigationService>();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (this._NavigationService == null)
        {
            return;
        }

        var seachKeywords = (this.SearchText ?? "").Split(' ').Select(word => word.Trim()).Where(word => !string.IsNullOrEmpty(word)).ToArray();

        if (this._SearchKeywords.SequenceEqual(seachKeywords))
        {
            return;
        }

        this._SearchKeywords = seachKeywords;
        this._Results = this._NavigationService.Search(this._SearchKeywords);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnClickSearchResultItem(NavigationListItem item)
    {
        this._NavigationService?.NavigateTo(item);
        await this.ExitSearchView.InvokeAsync();
    }

    #endregion Private Methods
}
