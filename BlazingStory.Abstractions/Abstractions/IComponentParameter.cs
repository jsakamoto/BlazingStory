using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Abstractions;

/// <summary>
/// Represents a component parameter with its metadata and value.
/// </summary>
public interface IComponentParameter
{
    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the CLR type of the parameter.
    /// </summary>
    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces | PublicProperties)]
    Type Type { get; }

    /// <summary>
    /// Gets the type structure describing nullability and generic information of the parameter.
    /// </summary>
    TypeStructure TypeStructure { get; }

    /// <summary>
    /// Gets the summary description of the parameter extracted from XML documentation.
    /// </summary>
    MarkupString Summary { get; }

    /// <summary>
    /// Gets a value indicating whether the parameter is required.
    /// </summary>
    bool Required { get; }

    /// <summary>
    /// Gets or sets the UI control type used to edit this parameter.
    /// </summary>
    ControlType Control { get; set; }

    /// <summary>
    /// Gets or sets the default value of the parameter.
    /// </summary>
    object? DefaultValue { get; set; }

    /// <summary>
    /// Update summary property text of this parameter by reading a XML document comment file.
    /// </summary>
    ValueTask UpdateSummaryFromXmlDocCommentAsync();

    /// <summary>
    /// Gets the string representations of the parameter type.
    /// </summary>
    IEnumerable<string> GetParameterTypeStrings();
}
