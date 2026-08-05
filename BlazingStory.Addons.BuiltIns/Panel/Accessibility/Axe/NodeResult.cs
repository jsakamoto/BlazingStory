using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class NodeResult
{
    private string _html = string.Empty;

    public string Html
    {
        get => this._html;
        init => this._html = Regex.Replace(value ?? string.Empty, "[\\s]*<!--!-->[\\s]*", "");
    }

    public string? Impact { get; init; } // type ImpactValue = 'minor' | 'moderate' | 'serious' | 'critical' | null;
    public IEnumerable<string> Target { get; init; } = [];
    public IEnumerable<CheckResult> Any { get; init; } = [];
    public IEnumerable<CheckResult> All { get; init; } = [];
    public IEnumerable<CheckResult> None { get; init; } = [];
    public string? FailureSummary { get; init; }
}
