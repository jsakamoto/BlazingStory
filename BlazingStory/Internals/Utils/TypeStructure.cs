namespace BlazingStory.Internals.Utils;

internal class TypeStructure
{
    #region Internal Fields

    internal readonly bool IsNullable;
    internal readonly bool IsGeneric;
    internal readonly Type PrimaryType;
    internal readonly Type[] SecondaryTypes;

    #endregion Internal Fields

    #region Public Constructors

    public TypeStructure(bool isNullable, bool isGeneric, Type primaryType, Type[] secondaryTypes)
    {
        this.IsNullable = isNullable;
        this.IsGeneric = isGeneric;
        this.PrimaryType = primaryType;
        this.SecondaryTypes = secondaryTypes;
    }

    #endregion Public Constructors

    #region Public Methods

    public void Deconstruct(out bool isNullable, out bool isGeneric, out Type primaryType, out Type[] secondaryTypes)
    {
        isNullable = this.IsNullable;
        isGeneric = this.IsGeneric;
        primaryType = this.PrimaryType;
        secondaryTypes = this.SecondaryTypes;
    }

    #endregion Public Methods
}
