using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Abstractions;

/// <summary>
/// Describes the structure of a type, including nullability, generics, and constituent types.
/// </summary>
public class TypeStructure
{
    /// <summary>
    /// Indicates whether the type is nullable.
    /// </summary>
    public readonly bool IsNullable;

    /// <summary>
    /// Indicates whether the type is a generic type.
    /// </summary>
    public readonly bool IsGeneric;

    /// <summary>
    /// The primary (outermost) type.
    /// </summary>
    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)]
    public readonly Type PrimaryType;

    /// <summary>
    /// The generic type arguments, if any.
    /// </summary>
    public readonly Type[] SecondaryTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeStructure"/> class.
    /// </summary>
    /// <param name="isNullable">Whether the type is nullable.</param>
    /// <param name="isGeneric">Whether the type is generic.</param>
    /// <param name="primaryType">The primary type.</param>
    /// <param name="secondaryTypes">The generic type arguments.</param>
    public TypeStructure(bool isNullable, bool isGeneric, [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type primaryType, Type[] secondaryTypes)
    {
        this.IsNullable = isNullable;
        this.IsGeneric = isGeneric;
        this.PrimaryType = primaryType;
        this.SecondaryTypes = secondaryTypes;
    }

    /// <summary>
    /// Deconstructs this instance into its constituent parts.
    /// </summary>
    /// <param name="isNullable">Whether the type is nullable.</param>
    /// <param name="isGeneric">Whether the type is generic.</param>
    /// <param name="primaryType">The primary type.</param>
    /// <param name="secondaryTypes">The generic type arguments.</param>
    public void Deconstruct(out bool isNullable, out bool isGeneric, out Type primaryType, out Type[] secondaryTypes)
    {
        isNullable = this.IsNullable;
        isGeneric = this.IsGeneric;
        primaryType = this.PrimaryType;
        secondaryTypes = this.SecondaryTypes;
    }
}
