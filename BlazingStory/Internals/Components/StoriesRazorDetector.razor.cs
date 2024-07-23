using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components;

public partial class StoriesRazorDetector : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public IEnumerable<Assembly>? Assemblies { get; set; }

    [Parameter, EditorRequired]
    public StoriesStore? StoriesStore { get; set; }

    #endregion Public Properties

    #region Private Fields

    private IEnumerable<StoriesRazorDescriptor> _StoriesRazors = Enumerable.Empty<StoriesRazorDescriptor>();

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        this._StoriesRazors = Services.StoriesRazorDetector.Detect(this.Assemblies);
    }

    #endregion Protected Methods
}
