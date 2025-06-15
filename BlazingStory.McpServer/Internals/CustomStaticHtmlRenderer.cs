using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazingStory.McpServer.Internals;

/// <summary>
/// Provides a custom static HTML renderer, enabling rendering Razor Components or Render Fragments to HTML strings.
/// </summary>
internal class CustomStaticHtmlRenderer : StaticHtmlRenderer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomStaticHtmlRenderer"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <param name="loggerFactory">The logger factory for logging.</param>
    public CustomStaticHtmlRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
    {
    }

    /// <summary>
    /// Override the resolver of component instance to forcibly render the component as HTML regardless of the render mode.
    /// </summary>
    protected override IComponent ResolveComponentForRenderMode([DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type componentType, int? parentComponentId, IComponentActivator componentActivator, IComponentRenderMode renderMode)
    {
        return componentActivator.CreateInstance(componentType);
    }

    /// <summary>
    /// A helper component used to render a <see cref="RenderFragment"/> as HTML.
    /// </summary>
    internal class RenderFragmentRenderer : ComponentBase
    {
        /// <summary>
        /// Gets or sets the child content to render.
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Builds the render tree for the component.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, this.ChildContent);
    }

    /// <summary>
    /// Renders the specified <see cref="RenderFragment"/> to an HTML string.
    /// </summary>
    /// <param name="renderFragment">The render fragment to render.</param>
    /// <param name="services">The service provider for dependency resolution.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered HTML string.</returns>
    public static async ValueTask<string> RenderToHtmlStringAsync(RenderFragment? renderFragment, IServiceProvider services)
    {
        var parameterView = ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            [nameof(RenderFragmentRenderer.ChildContent)] = renderFragment
        });

        return await RenderToHtmlStringAsync(typeof(RenderFragmentRenderer), services, parameterView);
    }

    /// <summary>
    /// Renders the specified component type to an HTML string with the given parameters.
    /// </summary>
    /// <param name="componentType">The type of the component to render.</param>
    /// <param name="services">The service provider for dependency resolution.</param>
    /// <param name="parameters">The parameters to pass to the component.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rendered HTML string.</returns>
    public static async ValueTask<string> RenderToHtmlStringAsync(Type componentType, IServiceProvider services, ParameterView parameters)
    {
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var navigationManager = services.GetRequiredService<NavigationManager>() as IHostEnvironmentNavigationManager;

        var serverAddressesFeature = services
            .GetRequiredService<IServer>()
            .Features
            .GetRequiredFeature<IServerAddressesFeature>();
        var appUrl = serverAddressesFeature.Addresses.First().TrimEnd('/') + "/";
        try
        {
            navigationManager?.Initialize(appUrl, appUrl);
        }
        catch (InvalidOperationException e) when (e.Message.EndsWith(" already initialized.")) { }


        await using var customRenderer = new CustomStaticHtmlRenderer(services, loggerFactory);
        return await customRenderer.Dispatcher.InvokeAsync(async () =>
        {
            try
            {
                var content = customRenderer.BeginRenderingComponent(componentType, parameters);
                await content.QuiescenceTask;
                return content.ToHtmlString();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoriesTool>();
                logger.LogError(ex, $"Failed to render the {componentType.FullName} component.");
                throw;
            }
        });
    }
}
