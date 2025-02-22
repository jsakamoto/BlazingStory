using BlazingStory.Internals.Models;

namespace BlazingStory.Test.Internals.Models;

internal class QueryRouteDataTest
{
    [Test]
    public void Ctor_From_StoryUri_Test()
    {
        var routeData = new QueryRouteData(new Uri("http://localhost/?path=/story/example/button--primary"), queryName: "path");
        routeData.Path.Is("/story/example/button--primary");
        routeData.View.Is("story");
        routeData.Parameter.Is("example/button--primary");
        routeData.RouteToStoryDocsOrCustom.IsTrue();
    }

    [Test]
    public void Ctor_From_SettingsUri_Test()
    {
        var routeData = new QueryRouteData(new Uri("http://localhost/?path=/settings/about"), queryName: "path");
        routeData.Path.Is("/settings/about");
        routeData.View.Is("settings");
        routeData.Parameter.Is("about");
        routeData.RouteToStoryDocsOrCustom.IsFalse();
    }

    [Test]
    public void Ctor_From_ViewAndParam_Test()
    {
        var routeData = new QueryRouteData("viewMode", "id");
        routeData.Path.Is("/viewMode/id");
        routeData.View.Is("viewMode");
        routeData.Parameter.Is("id");
        routeData.RouteToStoryDocsOrCustom.IsFalse();
    }
}
