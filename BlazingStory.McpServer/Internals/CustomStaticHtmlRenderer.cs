using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.Extensions.Logging;

namespace BlazingStory.McpServer.Internals;

internal class CustomStaticHtmlRenderer : StaticHtmlRenderer
{
    public CustomStaticHtmlRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
    {
    }

    protected override IComponent ResolveComponentForRenderMode([DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type componentType, int? parentComponentId, IComponentActivator componentActivator, IComponentRenderMode renderMode)
    {
        return componentActivator.CreateInstance(componentType);
    }
}
