using BlazingStory.Internals.Components.Icons;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.SideBar.SearchAndHistory;

public partial class NavigationList : ComponentBase
{
    #region Public Properties

    [Parameter]
    public IEnumerable<NavigationListItem>? Items { get; set; }

    [Parameter]
    public EventCallback<NavigationListItem> OnClickItem { get; set; }

    [Parameter]
    public RenderFragment<NavigationList>? Actions { get; set; }

    [Parameter]
    public Func<NavigationItemType, SvgIconType> GetIconType { get; set; }

    [Parameter]
    public Func<NavigationItemType, string> GetCssClass { get; set; }

    [Parameter]
    public IEnumerable<string>? Keywords { get; set; }

    #endregion Public Properties

    #region Private Fields

    private string? _LastHoveredId;

    #endregion Private Fields

    #region Public Constructors

    public NavigationList()
    {
        this.GetIconType = itemType => itemType switch
        {
            NavigationItemType.Component => SvgIconType.Component,
            NavigationItemType.Story => SvgIconType.BookmarkHollow,
            _ => SvgIconType.Document
        };

        this.GetCssClass = itemType => itemType switch
        {
            NavigationItemType.Component => "type-component",
            NavigationItemType.Story => "type-story",
            _ => "type-docs"
        };
    }

    #endregion Public Constructors

    #region Public Methods

    public bool IsSelected(string id) => this._LastHoveredId == id;

    public void OnHover(string id)
    {
        this._LastHoveredId = id;
    }

    #endregion Public Methods
}
