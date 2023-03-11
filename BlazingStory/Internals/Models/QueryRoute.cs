namespace BlazingStory.Internals.Models;

public class QueryRoute
{
    internal readonly string ViewName;

    internal readonly string Parameter;

    internal readonly Type ViewComponent;

    internal QueryRoute(string viewName, string parameter, Type viewComponent)
    {
        this.ViewName = viewName;
        this.Parameter = parameter;
        this.ViewComponent = viewComponent;
    }
}
