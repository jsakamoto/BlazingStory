using System.Linq.Expressions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Components;

/// <summary>
/// Configure the initial value of the component parameter. You can specify the initial value of the
/// component parameter by using the Arg component inside the Arguments render fragment parameter of
/// the Story component.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <typeparam name="TParameter">
/// The type of the parameter.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
public partial class Arg<TComponent, TParameter> : ComponentBase where TComponent : notnull
{
    /// <summary>
    /// Gets or sets the component parameter. The component parameter is specified by using a lambda
    /// expression. For example: (x) =&gt; x.MyParameter. The lambda expression must be a member
    /// access expression. The member access expression must be a member of the component type.
    /// </summary>
    [Parameter]
    public Expression<Func<TComponent, TParameter>>? For { get; set; }

    /// <summary>
    /// Gets or sets the initial value of the component parameter. The initial value of the
    /// component parameter.
    /// </summary>
    [Parameter]
    public TParameter? Value { get; set; }

    [CascadingParameter]
    internal Story<TComponent> Story { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (this.Story.Stories == null)
        {
            throw new InvalidOperationException($"The Stories cascading parameter is required.");
        }

        if (this.Story.StoryContext == null)
        {
            throw new InvalidOperationException($"The StoryContext parameter is required.");
        }

        if (!this.Story.Stories.ShowAllPropertiesForAllStories && !this.Story.ShowAllProperties)
        {
            if (this.Story.Template == null)
            {
                throw new InvalidOperationException($"The Template parameter is required.");
            }

            var xmlDocComment = this.Story.Stories.Services.GetRequiredService<IXmlDocComment>();
            var componentParameter = ParameterExtractor.GetComponentParameterByName(typeof(TComponent), ParameterExtractor.GetParameterName(this.For), xmlDocComment);

            var componentParameters = this.Story.StoryContext.Parameters?.ToList() ?? new List<ComponentParameter>();

            componentParameters.Add(componentParameter);

            var olArgs = this.Story.StoryContext.Args;

            this.Story.StoryContext = new(componentParameters);

            foreach (var arg in olArgs)
            {
                this.Story.StoryContext.InitArgument(arg.Key, arg.Value);
            }

            foreach (var arg in this.Story.Stories.ArgProps)
            {
                var param = this.Story.StoryContext.Parameters.FirstOrDefault(x => x.Name.Equals(arg.Name));

                if (param is not null)
                {
                    param.Control = arg.Control;
                    param.Options = arg.Options;
                }
            }

            this.Story.Stories.ComponentParameters = this.Story.StoryContext.Parameters;

            var renderFragment = (RenderFragment<StoryContext>)this.Story.Template.Clone();

            this.Story.Stories.StoryContainer?.RegisterStory(this.Story.Name, this.Story.StoryContext, this.Story.Stories.StoryContainer.Layout, this.Story.Stories.Layout, renderFragment, this.Story.Description);
        }

        this.Story.StoryContext.InitArgument(ParameterExtractor.GetParameterName(this.For), this.Value);
    }
}
