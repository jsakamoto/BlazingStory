using System.Net;
using System.Net.Sockets;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStoryApp1.Stories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLib1.Components.Button;
using Toolbelt.Diagnostics;

namespace BlazingStory.Test._Fixtures;

internal static class TestHelper
{
    internal static readonly RenderFragment<BlazingStory.Types.StoryContext> EmptyFragment = ctx => ((RenderTreeBuilder _) => { });

    internal static StoriesRazorDescriptor Descriptor(string title)
    {
        return new(typeof(object), new(title));
    }

    internal static class StoryContext
    {
        internal static BlazingStory.Types.StoryContext CreateEmpty() => new(Enumerable.Empty<ComponentParameter>());
    }

    internal static Story CreateStory<TComponent>(string title = "", string name = "")
    {
        var parameters = ParameterExtractor.GetParametersFromComponentType(typeof(TComponent), XmlDocComment.Dummy);
        var context = new BlazingStory.Types.StoryContext(parameters);
        return new Story(Descriptor(title), typeof(TComponent), name, context, null, null, EmptyFragment);
    }

    internal static IEnumerable<StoryContainer> GetExampleStories1(IServiceProvider services) => [
        new(typeof(Button), null, new(typeof(Button_stories), new("Examples/Button")), services) { Stories = {
            CreateStory<Button>(title: "Examples/Button", name: "Default Button"),
            CreateStory<Button>(title: "Examples/Button", name: "Primary Button"),
        }},
        new(typeof(Button), null, new(typeof(Select_stories), new("Examples/Select")), services) { Stories = {
            CreateStory<Button>(title: "Examples/Select", name: "Select"),
        }}
    ];

    internal static void XProcessOptions(XProcessOptions options)
    {
        options.WhenDisposing = XProcessTerminate.EntireProcessTree;
    }

    /// <summary>Get an available TCP v4 port number.</summary>
    internal static int GetAvailableTCPv4Port()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
