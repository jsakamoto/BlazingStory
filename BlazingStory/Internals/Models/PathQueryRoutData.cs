namespace BlazingStory.Internals.Models;

public class PathQueryRoutData
{
    internal readonly string Path;

    internal readonly string View;

    internal readonly string Parameter;

    internal readonly bool RouteToStoryOrDocs;

    public PathQueryRoutData(string path)
    {
        this.Path = path;

        var segments = path.Trim('/').Split('/');
        this.View = segments.FirstOrDefault() ?? "";
        this.Parameter = string.Join('/', segments.Skip(1));
        this.RouteToStoryOrDocs = this.View is "story" or "docs";
    }
}
