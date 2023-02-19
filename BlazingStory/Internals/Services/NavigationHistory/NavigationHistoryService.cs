using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.NavigationHistory;

internal class NavigationHistoryService
{
    private readonly NavigationManager _NavigationManager;

    public NavigationHistoryService(NavigationManager navigationManager)
    {
        this._NavigationManager = navigationManager;
    }
}
