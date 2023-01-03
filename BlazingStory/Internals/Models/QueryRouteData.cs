namespace BlazingStory.Internals.Models;

public class QueryRouteData : IEquatable<QueryRouteData?>
{
    internal readonly string View;

    internal readonly string Parameter;

    internal readonly bool RouteToStoryOrDocs;

    public QueryRouteData(string path)
    {
        var segments = path.Trim('/').Split('/');
        this.View = segments.FirstOrDefault() ?? "";
        this.Parameter = string.Join('/', segments.Skip(1));
        this.RouteToStoryOrDocs = this.View is "story" or "docs";
    }

    public QueryRouteData(string viewMode, string id)
    {
        this.View = viewMode;
        this.Parameter = id;
    }

    public override bool Equals(object? obj) => this.Equals(obj as QueryRouteData);

    public bool Equals(QueryRouteData? other)
    {
        return other is not null &&
               this.View == other.View &&
               this.Parameter == other.Parameter &&
               this.RouteToStoryOrDocs == other.RouteToStoryOrDocs;
    }

    public override int GetHashCode() => HashCode.Combine(this.View, this.Parameter, this.RouteToStoryOrDocs);

    public static bool operator ==(QueryRouteData? left, QueryRouteData? right) => EqualityComparer<QueryRouteData>.Default.Equals(left, right);

    public static bool operator !=(QueryRouteData? left, QueryRouteData? right) => !(left == right);
}
