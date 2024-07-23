using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Components;

/// <summary>
/// The story component is used to define a story for a component. A story is a way to render a
/// component in a specific way, with specific parameters. It is used to show how a component can be
/// used in different scenarios.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
[CascadingTypeParameter(nameof(TComponent))]
public partial class Story<TComponent> : ComponentBase where TComponent : notnull
{
    #region Public Properties

    /// <summary>
    /// The name of the story that will be shown in the sidebar navigation tree.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [Parameter, EditorRequired]
    public string Name { get; set; } = "";

    /// <summary>
    /// A type of the layout component to use when displaying this story.
    /// </summary>
    [Parameter]
    public Type? Layout { get; set; }

    /// <summary>
    /// The arguments are the parameters that will be passed to the component when rendering the story.
    /// </summary>
    /// <value>
    /// The arguments.
    /// </value>
    [Parameter]
    public RenderFragment? Arguments { get; set; }

    /// <summary>
    /// The template is the content of the story.
    /// </summary>
    /// <value>
    /// The template.
    /// </value>
    [Parameter, EditorRequired]
    public RenderFragment<StoryContext>? Template { get; set; }

    /// <summary>
    /// Show all properties for this story. If set to true, all properties will be shown for this
    /// story. If set to false, only properties that are different from the default values will be shown.
    /// </summary>
    [Parameter]
    public bool ShowAllProperties { get; set; } = false;

    /// <summary>
    /// The description of the story.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal Stories<TComponent>? Stories { get; set; }

    #endregion Internal Properties

    #region Private Fields

    internal StoryContext? StoryContext;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        if (this.Template == null)
        {
            throw new InvalidOperationException($"The Template parameter is required.");
        }

        if (this.Stories == null)
        {
            throw new InvalidOperationException($"The Stories cascading parameter is required.");
        }

        var xmlDocComment = this.Stories.Services.GetRequiredService<IXmlDocComment>();

        if (this.Stories.ShowAllPropertiesForAllStories || this.ShowAllProperties)
        {
            if (this.Stories.ComponentParameters is null || !this.Stories.ComponentParameters.Any())
            {
                this.Stories.ComponentParameters = ParameterExtractor.GetParametersFromComponentType(typeof(TComponent), xmlDocComment);
            }

            foreach (var arg in this.Stories.ArgProps)
            {
                var param = this.Stories.ComponentParameters.FirstOrDefault(x => x.Name.Equals(arg.Name));

                if (param is not null)
                {
                    param.Control = arg.Control;
                    param.Options = arg.Options;
                }
            }

            this.StoryContext = new(this.Stories.ComponentParameters);
        }
        else
        {
            this.StoryContext = new(Enumerable.Empty<ComponentParameter>());
        }

        if (this.Stories.StoryContainer == null)
        {
            this.Stories.StoryContainer = new StoryContainer(typeof(TComponent), null, this.Stories.StoriesRazorDescriptor, this.Stories.Services);
            this.Stories.StoriesStore.RegisterStoryContainer(this.Stories.StoryContainer);
        }

        this.Stories.StoryContainer.RegisterStory(this.Name, this.StoryContext, this.Stories.StoryContainer.Layout, this.Layout, this.Template, this.Description);
    }

    /// <summary>
    /// When this story should be rendered, notify it to the canvas frame (preview frame inside the
    /// iframe) through the <see cref="Types.StoryContext" />.
    /// </summary>
    protected override bool ShouldRender()
    {
        var shouldRender = base.ShouldRender();

        if (shouldRender)
        {
            this.StoryContext?.InvokeShouldRender();
        }

        return shouldRender;
    }

    #endregion Protected Methods
}
