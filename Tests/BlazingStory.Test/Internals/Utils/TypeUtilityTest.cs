using System.Net;
using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

public class TypeUtilityTest
{
    private enum EnumA
    { ValueX, ValueY, ValueZ }

    [Test]
    public void TryConvertType_RenderFragment_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(RenderFragment));

        // When
        TypeUtility.TryConvertType(target, "Hello, World.", out var result).IsTrue();

        // Then
        var renderFragment = result.IsInstanceOf<RenderFragment>();

        using var ctx = new Bunit.TestContext();
        using var cut = ctx.Render(renderFragment);
        cut.Markup.Is("Hello, World.");

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_RenderFragmentT_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(RenderFragment<DateTime>));

        // When
        TypeUtility.TryConvertType(target, "Tempura et dolor", out var result).IsTrue();

        // Then
        var renderFragment = result.IsInstanceOf<RenderFragment<DateTime>>();

        using var ctx = new Bunit.TestContext();
        using var cut = ctx.Render(renderFragment.Invoke(default));
        cut.Markup.Is("Tempura et dolor");

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableEnum_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(EnumA?));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableEnum_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(EnumA?));

        // When
        TypeUtility.TryConvertType(target, "ValueY", out var result).IsTrue();

        // Then
        result.IsInstanceOf<EnumA>().Is(EnumA.ValueY);

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableBool_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(bool?));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableBool_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(bool?));

        // When
        TypeUtility.TryConvertType(target, "true", out var result).IsTrue();

        // Then
        result.IsInstanceOf<bool>().IsTrue();

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableInt_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(int?));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_NullableInt_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(int?));

        // When
        TypeUtility.TryConvertType(target, "1024", out var result).IsTrue();

        // Then
        result.IsInstanceOf<int>().Is(1024);

        Assert.Pass();
    }

    [Test]
    public void TryConvertType_Double_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(double));

        // When
        TypeUtility.TryConvertType(target, "3.141592", out var result).IsTrue();

        // Then
        var doubleValue = result.IsInstanceOf<double>();
        doubleValue.Is(3.141592);

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_NonGetenricType_Test()
    {
        TypeUtility.GetOpenType(typeof(IPAddress)).Is(typeof(IPAddress));

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_GetenricType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<>)).Is(typeof(List<>));

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_SignleTypeArgType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<IPAddress>)).Is(typeof(List<>));

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_SignleTypeArgNestedType_Test()
    {
        TypeUtility.GetOpenType(typeof(List<List<IPAddress>>)).Is(typeof(List<>));

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_DoubleTypeArgTypes_Test()
    {
        TypeUtility.GetOpenType(typeof(Dictionary<int, IPAddress>)).Is(typeof(Dictionary<,>));

        Assert.Pass();
    }

    [Test]
    public void GetOpenType_DoubleTypeArgNestedTypes_Test()
    {
        TypeUtility.GetOpenType(typeof(Dictionary<int, List<IPAddress>>)).Is(typeof(Dictionary<,>));

        Assert.Pass();
    }

    [Test]
    public void IsParsableType_for_int_Test()
    {
        TypeUtility.IsParsableType(typeof(int)).IsTrue();

        Assert.Pass();
    }

    [Test]
    public void IsParsableType_for_string_Test()
    {
        TypeUtility.IsParsableType(typeof(TypeUtility)).IsFalse();

        Assert.Pass();
    }
}
