using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages.Settings.Panels;

public partial class ReleaseNotesPanel : ComponentBase
{
    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    private record ReleaseNoteSection(string VersionTitle, IEnumerable<string> ChangeLogs);

    #region Private Fields

    private IEnumerable<ReleaseNoteSection> _ReleaseNoteSections = Enumerable.Empty<ReleaseNoteSection>();

    #endregion Private Fields

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var webAssets = this.Services.GetRequiredService<WebAssets>();
        var releaseNotesText = await webAssets.GetStringAsync("_content/BlazingStory/RELEASE-NOTES.txt");

        var releaseNotesLines = releaseNotesText
            .Split("\n")
            .Select(line => line.Trim('\r'))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Append("v.0.0.0");

        var releaseNoteSections = new List<ReleaseNoteSection>();
        var currentVersion = "";
        var currentChangeLogs = new List<string>();

        foreach (var line in releaseNotesLines)
        {
            if (line.StartsWith("v."))
            {
                if (currentVersion != null)
                {
                    releaseNoteSections.Add(new(currentVersion, currentChangeLogs));
                    currentVersion = line;
                    currentChangeLogs = new();
                }
            }
            else
            {
                currentChangeLogs.Add(line.TrimStart('-', ' '));
            }
        }

        this._ReleaseNoteSections = releaseNoteSections;
    }

    #endregion Protected Methods
}
