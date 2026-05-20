using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

[DynamicallyAccessedMembers(All)]
public class AxeResults
{
    public IEnumerable<Result> Violations { get; init; } = [];
    public IEnumerable<Result> Passes { get; init; } = [];
    public IEnumerable<Result> Incomplete { get; init; } = [];
}
