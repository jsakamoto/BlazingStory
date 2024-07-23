using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages.Settings.Panels;

public partial class AboutPanel : ComponentBase
{
    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Fields

    private string? _ReadmeMd;

    #endregion Private Fields

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var webAssets = this.Services.GetRequiredService<WebAssets>();
        var readmeMd = await webAssets.GetStringAsync("_content/BlazingStory/README.txt");

        this._ReadmeMd = readmeMd;
    }

    #endregion Protected Methods
}
