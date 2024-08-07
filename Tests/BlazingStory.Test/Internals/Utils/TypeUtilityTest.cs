using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;
using System.Net;

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

    [Test]
    public void GetOpenType_NonGetenricType_Test()
    {
        TypeUtility.GetOpenType(typeof(IPAddress)).Is(typeof(IPAddress));
    }

    [Test]
    public void GetOpenType_GetenricType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<>)).Is(typeof(List<>));
    }

    [Test]
    public void GetOpenType_SignleTypeArgType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<IPAddress>)).Is(typeof(List<>));
    }

    [Test]
    public void GetOpenType_SignleTypeArgNestedType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<List<IPAddress>>)).Is(typeof(List<>));
    }

    [Test]
    public void GetOpenType_DoubleTypeArgTypes_Test()
    {
        TypeUtility.GetOpenType(typeof(Dictionary<int, IPAddress>)).Is(typeof(Dictionary<,>));
    }

    [Test]
    public void GetOpenType_DoubleTypeArgNestedTypes_Test()
    {
        TypeUtility.GetOpenType(typeof(Dictionary<int, List<IPAddress>>)).Is(typeof(Dictionary<,>));
    }
}
