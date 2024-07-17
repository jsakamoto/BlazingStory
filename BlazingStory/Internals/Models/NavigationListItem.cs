namespace BlazingStory.Internals.Models;

public class NavigationListItem : INavigationPath
{
    #region Public Properties

    /// <summary>
    /// Gets a navigation path string for the item. <br /> (ex. "/story/example-button--primary", "/docs/example-button--docs")
    /// </summary>
    public string? NavigationPath { get; init; }

    #endregion Public Properties

    #region Public Fields

    public int Id;

    public string? Caption;

    public NavigationItemType Type;
    public IEnumerable<string>? Segments;

    #endregion Public Fields

    #region Internal Methods

    internal static NavigationListItem CreateFrom(int id, NavigationTreeItem treeItem)
    {
        return new NavigationListItem
        {
            Id = id,
            Type = treeItem.Type,
            Caption = treeItem.Caption,
            NavigationPath = treeItem.NavigationPath,
            Segments = treeItem.PathSegments
        };
    }

    /// <summary>
    /// Compares the equality of two <see cref="NavigationListItem" />, except the <see cref="Id" /> field.
    /// </summary>
    /// <param name="other">
    /// The other <see cref="NavigationListItem" /> to compare.
    /// </param>
    internal bool Equals(NavigationListItem? other)
    {
        if (other == null)
        {
            return false;
        }

        return this.NavigationPath == other.NavigationPath && this.Caption == other.Caption && this.Type == other.Type;
    }

    #endregion Internal Methods
}
