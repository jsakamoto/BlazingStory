using System.Reflection.Metadata;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Components;

/// <summary>
/// The Stories component indicates to the Blazing Story runtime what is the target component of the
/// story with its TComponent type parameter. The "Controls" panel will be built from this
/// component-type information. The Stories component can include one or more Story components. The
/// Story component has the Name parameter, which will be shown in the sidebar navigation tree to
/// identify each story.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
[CascadingTypeParameter(nameof(TComponent))]
public partial class Stories<TComponent> : ComponentBase where TComponent : notnull
{
    #region Public Properties

    /// <summary>
    /// A type of the layout component to use when displaying these stories.
    /// </summary>
    [Parameter]
    public Type? Layout { get; set; }

    /// <summary>
    /// This is the content of the story. It can be a single story or multiple stories.
    /// </summary>
    /// <value>
    /// The content of the child.
    /// </value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool ShowAllPropertiesForAllStories { get; set; } = false;

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    internal StoriesStore StoriesStore { get; init; } = default!;

    [CascadingParameter]
    internal StoriesRazorDescriptor StoriesRazorDescriptor { get; init; } = default!;

    [CascadingParameter]
    internal IEnumerable<ComponentParameter> ComponentParameters { get; set; } = Enumerable.Empty<ComponentParameter>();

    [CascadingParameter]
    internal StoryContainer? StoryContainer { get; set; }

    [CascadingParameter]
    internal List<ArgProp> ArgProps { get; set; } = new();

    #endregion Internal Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        this.StoryContainer = new StoryContainer(typeof(TComponent), this.Layout, this.StoriesRazorDescriptor, this.Services);
        this.StoriesStore.RegisterStoryContainer(this.StoryContainer);

        var xmlDocComment = this.Services.GetRequiredService<IXmlDocComment>();
        this.ComponentParameters = this.ShowAllPropertiesForAllStories ? ParameterExtractor.GetParametersFromComponentType(typeof(TComponent), xmlDocComment) : Enumerable.Empty<ComponentParameter>();
    }

    #endregion Protected Methods
}

public class ArgProp
{
    #region Public Properties

    public string? Name { get; set; }
    public ControlType Control { get; set; } = ControlType.Default;
    public object? DefaultValue { get; set; }
    public string[]? Options { get; set; }

    #endregion Public Properties
}
