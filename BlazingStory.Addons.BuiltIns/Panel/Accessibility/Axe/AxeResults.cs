namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

public class AxeResults
{
    public IEnumerable<Result> Violations { get; init; } = [];
    public IEnumerable<Result> Passes { get; init; } = [];
    public IEnumerable<Result> Incomplete { get; init; } = [];
}
