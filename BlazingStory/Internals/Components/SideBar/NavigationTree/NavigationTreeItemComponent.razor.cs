using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar.NavigationTree;

public partial class NavigationTreeItemComponent : ComponentBase
{
    #region Public Properties

    [CascadingParameter]
    public QueryRouteData? RouteData { get; set; }

    [Parameter, EditorRequired]
    public NavigationTreeItem? Item { get; set; }

    [Parameter]
    public bool SubHeading { get; set; }

    [Parameter]
    public int IndentLevel { get; set; }

    [Parameter]
    public EventCallback ExpansionChanged { get; set; }

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private NavigationService? _NavigationService;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        this._NavigationService = this.Services.GetRequiredService<NavigationService>();
    }

    #endregion Protected Methods

    #region Private Methods

    private static SvgIconType GetIconType(NavigationTreeItem item)
    {
        return item.Type switch
        {
            NavigationItemType.Component => SvgIconType.Component,
            NavigationItemType.Docs => SvgIconType.Document,
            NavigationItemType.Story => SvgIconType.BookmarkHollow,
            _ => SvgIconType.Folder
        };
    }

    private async Task ToggleItemExpansion(NavigationTreeItem item)
    {
        item.Expanded = !item.Expanded;
        await this.ExpansionChanged.InvokeAsync();

        if (item.Type == NavigationItemType.Component && item.Expanded && item.SubItems.Any())
        {
            var storyItemOf1st = item.SubItems.First();
            this._NavigationService?.NavigateTo(storyItemOf1st);
        }
    }

    #endregion Private Methods
}
