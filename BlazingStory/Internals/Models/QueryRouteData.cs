using System.Web;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents the structured route data of the "path" query string of a URL like "http://.../?path=/story/example-button--primary".
/// </summary>
public class QueryRouteData : IEquatable<QueryRouteData?>
{
    /// <summary>
    /// Gets the "path" query string that is source of this <see cref="QueryRouteData"/> instance.<br/>
    /// If a URL like "http://.../?path=/story/example-button--primary" is given, this field will return "/story/example-button--primary".
    /// </summary>
    internal readonly string Path;

    /// <summary>
    /// Gets the first segment of the "path" query string.<br/>
    /// If a URL like "http://.../?path=/story/example-button--primary" is given, this field will return "story".
    /// </summary>
    internal readonly string View;

    /// <summary>
    /// Gets the second segment of the "path" query string.<br/>
    /// If a URL like "http://.../?path=/story/example-button--primary" is given, this field will return "example-button--primary".
    /// </summary>
    internal readonly string Parameter;

    /// <summary>
    /// Gets whether the View is "story" or "docs".<br/>
    /// If a URL like "http://.../?path=/story/example-button--primary" is given, this field will return true.<br/>
    /// If a URL like "http://.../?path=/settings/about" is given, this field will return false.<br/>
    /// </summary>
    internal readonly bool RouteToStoryDocsOrCustom;

    /// <summary>
    /// Initialize a new instance of <see cref="QueryRouteData"/> from an URL like "http://.../?path=/story/example-button--primary" and a name of query parameter.
    /// </summary>
    /// <param name="uri">An URL to create a query route data from.</param>
    /// <param name="queryName">A query parameter name to create a query route data from.</param>
    public QueryRouteData(Uri uri, string queryName)
    {
        var queryStrings = HttpUtility.ParseQueryString(uri.Query);
        var queryString = queryStrings[queryName]?.Trim('/') ?? "";
        var segments = queryString.Trim('/').Split('/');

        this.Path = "/" + queryString;
        this.View = segments.FirstOrDefault() ?? "";
        this.Parameter = string.Join('/', segments.Skip(1));
        this.RouteToStoryDocsOrCustom = this.View is "story" or "docs" or "custom";
    }

    /// <summary>
    /// Initialize a new instance of <see cref="QueryRouteData"/> with arguments to specify each field directly.<br/>
    /// The <see cref="Path"/> field will be created from the <paramref name="view"/> and <paramref name="parameter"/> arguments.
    /// </summary>
    /// <param name="view">An argument for the <see cref="View"/> field</param>
    /// <param name="parameter">An argument for the <see cref="Parameter"/> field</param>
    public QueryRouteData(string view, string parameter)
    {
        this.Path = $"/{view}/{parameter}";
        this.View = view;
        this.Parameter = parameter;
    }

    public override bool Equals(object? obj) => this.Equals(obj as QueryRouteData);

    public bool Equals(QueryRouteData? other) => other is not null && this.Path == other.Path;

    public override int GetHashCode() => HashCode.Combine(this.Path);

    public static bool operator ==(QueryRouteData? left, QueryRouteData? right) => EqualityComparer<QueryRouteData>.Default.Equals(left, right);

    public static bool operator !=(QueryRouteData? left, QueryRouteData? right) => !(left == right);
}
