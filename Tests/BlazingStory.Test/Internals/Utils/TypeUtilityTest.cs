using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

public class TypeUtilityTest
{
    [Test]
    public void TryConvertType_RenderFragment_Test()
    {
        // Given
        var targetType = typeof(RenderFragment);
        var targetTypeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, targetTypeStruct, "Hello, World.", out var result).IsTrue();

        // Then
        var renderFragment = result.IsInstanceOf<RenderFragment>();

        using var ctx = new Bunit.TestContext();
        using var cut = ctx.Render(renderFragment);
        cut.Markup.Is("Hello, World.");
    }

    [Test]
    public void TryConvertType_RenderFragmentT_Test()
    {
        // Given
        var targetType = typeof(RenderFragment<DateTime>);
        var targetTypeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, targetTypeStruct, "Tempura et dolor", out var result).IsTrue();

        // Then
        var renderFragment = result.IsInstanceOf<RenderFragment<DateTime>>();

        using var ctx = new Bunit.TestContext();
        using var cut = ctx.Render(renderFragment.Invoke(default));
        cut.Markup.Is("Tempura et dolor");
    }

    private enum EnumA { ValueX, ValueY, ValueZ }

    [Test]
    public void TryConvertType_NullableEnum_from_Null_Test()
    {
        // Given
        var targetType = typeof(EnumA?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_NullableEnum_from_Value_Test()
    {
        // Given
        var targetType = typeof(EnumA?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "ValueY", out var result).IsTrue();

        // Then
        result.IsInstanceOf<EnumA>().Is(EnumA.ValueY);
    }

    [Test]
    public void TryConvertType_NullableBool_from_Null_Test()
    {
        // Given
        var targetType = typeof(bool?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_NullableBool_from_Value_Test()
    {
        // Given
        var targetType = typeof(bool?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "true", out var result).IsTrue();

        // Then
        result.IsInstanceOf<bool>().IsTrue();
    }

    [Test]
    public void TryConvertType_NullableInt_from_Null_Test()
    {
        // Given
        var targetType = typeof(int?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_NullableInt_from_Value_Test()
    {
        // Given
        var targetType = typeof(int?);
        var typeStruct = TypeUtility.ExtractTypeStructure(targetType);

        // When
        TypeUtility.TryConvertType(targetType, typeStruct, "1024", out var result).IsTrue();

        // Then
        result.IsInstanceOf<int>().Is(1024);
    }
}
