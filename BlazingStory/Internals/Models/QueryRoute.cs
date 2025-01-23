using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

public class QueryRoute
{
    internal readonly string ViewName;

    internal readonly string Parameter;

    [DynamicallyAccessedMembers(All)]
    internal readonly Type ViewComponent;

    internal QueryRoute(string viewName, string parameter, [DynamicallyAccessedMembers(All)] Type viewComponent)
    {
        this.ViewName = viewName;
        this.Parameter = parameter;
        this.ViewComponent = viewComponent;
    }
}
