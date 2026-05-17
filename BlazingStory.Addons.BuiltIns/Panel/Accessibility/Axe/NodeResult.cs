namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

public class NodeResult
{
    public string Html { get; init; } = string.Empty;
    public string? Impact { get; init; } // type ImpactValue = 'minor' | 'moderate' | 'serious' | 'critical' | null;
    public IEnumerable<string> Target { get; init; } = [];
    public IEnumerable<CheckResult> Any { get; init; } = [];
    public IEnumerable<CheckResult> All { get; init; } = [];
    public IEnumerable<CheckResult> None { get; init; } = [];
    public string? FailureSummary { get; init; }
}
