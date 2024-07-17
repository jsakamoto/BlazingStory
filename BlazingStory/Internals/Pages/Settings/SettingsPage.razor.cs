using BlazingStory.Internals.Models;
using BlazingStory.Internals.Pages.Settings.Panels;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages.Settings;

public partial class SettingsPage : ComponentBase
{
    #region Public Properties

    [CascadingParameter]
    public QueryRouteData? RouteData { get; set; }

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    [Inject] private NavigationManager? NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Fields

    private static readonly (string Caption, QueryRoute Route)[] _SettingsPanels = new (string, QueryRoute)[]
    {
        ("About",              new("*", "about",         typeof(AboutPanel))),
        ("Release notes",      new("*", "release-notes", typeof(ReleaseNotesPanel))),
        ("Keyboard shortcuts", new("*", "shortcuts",     typeof(KeyboardShortcutsPanel))),
    };

    #endregion Private Fields

    #region Private Methods

    private void OnClickTabButton(QueryRoute route)
    {
        if (this.NavigationManager is not null)
        {
            this.NavigationManager.NavigateTo($"./?path=/settings/{route.Parameter}");
        }
    }

    private bool IsActive(QueryRoute route)
    {
        return route.Parameter == this.RouteData?.Parameter;
    }

    private void OnClickCloseButton()
    {
        var navService = this.Services.GetRequiredService<NavigationService>();
        navService.BackToLastNavigated();
    }

    #endregion Private Methods
}
