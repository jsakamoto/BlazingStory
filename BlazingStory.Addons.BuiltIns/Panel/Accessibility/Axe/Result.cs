using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

[DynamicallyAccessedMembers(All)]
public class Result
{
    public string Description { get; init; } = string.Empty;
    public string Help { get; init; } = string.Empty;
    public string HelpUrl { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
    public string? Impact { get; init; } // type ImpactValue = 'minor' | 'moderate' | 'serious' | 'critical' | null;
    public string?[] Tags { get; init; } = [];
    public IEnumerable<NodeResult> Nodes { get; init; } = [];
}
