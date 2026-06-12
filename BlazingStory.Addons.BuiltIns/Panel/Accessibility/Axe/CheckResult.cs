using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

[DynamicallyAccessedMembers(All)]
public class CheckResult
{
    public string Id { get; init; } = string.Empty;
    public string Impact { get; init; } = string.Empty;  // type ImpactValue = 'minor' | 'moderate' | 'serious' | 'critical' | null;
    public string Message { get; init; } = string.Empty;
}
