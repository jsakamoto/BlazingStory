using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services.Navigation;

public class NavigationHistory
{
    private const int MAX_HISTORY_ITEMS = 8;

    private readonly HelperScript _HelperScript;

    private LinkedList<NavigationListItem> _HistoryItems = new();

    private const string StorageKey = "SideBar.NavigationHistory";

    private bool _Initialized = false;

    internal NavigationHistory(HelperScript helperScript)
    {
        this._HelperScript = helperScript;
    }

    private async ValueTask EnsureInitializeAsync()
    {
        if (!this._Initialized)
        {
            this._Initialized = true;
            this._HistoryItems = await this._HelperScript.LoadObjectFromLocalStorageAsync(StorageKey, new LinkedList<NavigationListItem>());
        }
    }

    internal async ValueTask<IEnumerable<NavigationListItem>> GetItemsAsync()
    {
        await this.EnsureInitializeAsync();
        return this._HistoryItems;
    }

    internal async ValueTask AddAsync(NavigationTreeItem root, NavigationTreeItem active)
    {
        if (active.Type is not NavigationItemType.Docs and not NavigationItemType.Story and not NavigationItemType.Custom) return;

        var componentItem = root.FindParentOf(active);
        if (componentItem == null) return;

        await this.EnsureInitializeAsync();
        var nextId = Enumerable.Range(0, int.MaxValue).Where(n => this._HistoryItems.All(item => item.Id != n)).First();
        var historyItem = active.Type switch
        {
            NavigationItemType.Story => new NavigationListItem()
            {
                Id = nextId,
                Caption = componentItem.Caption,
                Type = active.Type,
                NavigationPath = componentItem.SubItems.First(item => item.Type is NavigationItemType.Docs or NavigationItemType.Story).NavigationPath,
                Segments = componentItem.PathSegments
            },
            NavigationItemType.Docs => new NavigationListItem()
            {
                Id = nextId,
                Caption = active.Caption,
                Type = active.Type,
                NavigationPath = active.NavigationPath,
                Segments = active.PathSegments
            },
            NavigationItemType.Custom => new NavigationListItem()
            {
                Id = nextId,
                Caption = active.Caption,
                Type = active.Type,
                NavigationPath = active.NavigationPath,
                Segments = active.PathSegments
            },
            _ => throw new NotImplementedException()
        };

        var latestHistoryItem = this._HistoryItems.FirstOrDefault();
        if (historyItem.Equals(latestHistoryItem)) return;

        while (this._HistoryItems.Count >= MAX_HISTORY_ITEMS) this._HistoryItems.RemoveLast();
        this._HistoryItems.AddFirst(historyItem);

        await this._HelperScript.SaveObjectToLocalStorageAsync(StorageKey, this._HistoryItems);
    }

    internal async ValueTask ClearAsync()
    {
        await this.EnsureInitializeAsync();
        this._HistoryItems.Clear();
        await this._HelperScript.SaveObjectToLocalStorageAsync(StorageKey, this._HistoryItems);
    }
}
