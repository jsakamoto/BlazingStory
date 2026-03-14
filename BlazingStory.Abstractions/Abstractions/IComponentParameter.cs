using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Abstractions;

public interface IComponentParameter
{
    string Name { get; }

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces | PublicProperties)]
    Type Type { get; }

    TypeStructure TypeStructure { get; }

    MarkupString Summary { get; }

    bool Required { get; }

    ControlType Control { get; set; }

    object? DefaultValue { get; set; }

    /// <summary>
    /// Update summary property text of this parameter by reading a XML document comment file.
    /// </summary>
    ValueTask UpdateSummaryFromXmlDocCommentAsync();

    IEnumerable<string> GetParameterTypeStrings();
}
