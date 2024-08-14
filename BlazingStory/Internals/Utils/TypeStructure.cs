using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

internal class TypeStructure
{
    internal readonly bool IsNullable;
    internal readonly bool IsGeneric;

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)]
    internal readonly Type PrimaryType;

    internal readonly Type[] SecondaryTypes;

    public TypeStructure(bool isNullable, bool isGeneric, [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type primaryType, Type[] secondaryTypes)
    {
        this.IsNullable = isNullable;
        this.IsGeneric = isGeneric;
        this.PrimaryType = primaryType;
        this.SecondaryTypes = secondaryTypes;
    }

    public void Deconstruct(out bool isNullable, out bool isGeneric, out Type primaryType, out Type[] secondaryTypes)
    {
        isNullable = this.IsNullable;
        isGeneric = this.IsGeneric;
        primaryType = this.PrimaryType;
        secondaryTypes = this.SecondaryTypes;
    }
}
