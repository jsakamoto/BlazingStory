namespace BlazingStory.Internals.Models;

public class QueryRoute
{
    #region Internal Fields

    internal readonly string ViewName;

    internal readonly string Parameter;

    internal readonly Type ViewComponent;

    #endregion Internal Fields

    #region Internal Constructors

    internal QueryRoute(string viewName, string parameter, Type viewComponent)
    {
        this.ViewName = viewName;
        this.Parameter = parameter;
        this.ViewComponent = viewComponent;
    }

    #endregion Internal Constructors
}
