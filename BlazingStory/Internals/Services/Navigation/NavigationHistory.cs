using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services.Navigation;

public class NavigationHistory
{
    private const int MAX_HISTORY_ITEMS = 8;

    private readonly HelperScript _HelperScript;

    private LinkedList<NavigationHistoryItem> _HistoryItems = new();

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
            this._HistoryItems = await this._HelperScript.LoadObjectFromLocalStorageAsync(StorageKey, new LinkedList<NavigationHistoryItem>());
        }
    }

    internal async ValueTask<IEnumerable<NavigationHistoryItem>> GetItemsAsync()
    {
        await this.EnsureInitializeAsync();
        return this._HistoryItems;
    }

    internal async ValueTask AddAsync(NavigationTreeItem root, NavigationTreeItem active)
    {
        await this.EnsureInitializeAsync();

        var historyItem = default(NavigationHistoryItem);
        var nextIds = Enumerable.Range(0, int.MaxValue).Where(n => this._HistoryItems.All(item => item.Id != n));

        if (active.Type == NavigationTreeItemType.Story)
        {
            var componentItem = root.FindParentOf(active);
            if (componentItem == null) return;

            var firstStory = componentItem.SubItems.FirstOrDefault(item => item.Type == NavigationTreeItemType.Story);
            if (firstStory == null) return;

            historyItem = new()
            {
                Id = nextIds.First(),
                Caption = componentItem.Caption,
                Type = NavigationTreeItemType.Story,
                NavigationPath = firstStory.NavigationPath,
                Segments = componentItem.PathSegments
            };
        }
        else
        {
            return;
        }

        if (historyItem == null) return;
        var latestHistoryItem = this._HistoryItems.FirstOrDefault();
        if (latestHistoryItem != null && latestHistoryItem.NavigationPath == historyItem.NavigationPath) return;

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
