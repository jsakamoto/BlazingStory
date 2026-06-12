using System.Net;
using BlazingStory.ToolKit.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

[SetCulture("en-US")]
public class TypeUtilityTest
{
    private sealed class SampleRef { }

    [Test]
    public void TryConvertType_RenderFragment_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(RenderFragment));

        // When
        TypeUtility.TryConvertType(target, "Hello, World.", out var result).IsTrue();

        // Then
        var renderFragment = result.IsInstanceOf<RenderFragment>();

        using var ctx = new Bunit.BunitContext();
        using var cut = ctx.Render(renderFragment);
        cut.Markup.Is("Hello, World.");
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

        using var ctx = new Bunit.BunitContext();
        using var cut = ctx.Render(renderFragment.Invoke(default));
        cut.Markup.Is("Tempura et dolor");
    }

    private enum EnumA { ValueX, ValueY, ValueZ }

    [Test]
    public void TryConvertType_NullableEnum_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(EnumA?));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
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
    }

    // The encoder emits "(null)" for any null arg value. The decoder must honour it for reference types too, not just Nullable<T>.
    // Also pins the documented behaviour change: a user-typed literal "(null)" in a string control becomes null.
    [Test]
    public void TryConvertType_String_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(string));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_RenderFragment_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(RenderFragment));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_Action_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(Action));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_List_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(List<int>));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_CustomClass_from_Null_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(SampleRef));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsTrue();

        // Then
        result.IsNull();
    }

    // Collection elements must recurse through TryConvertType and a null element must round-trip back to null, not the literal "(null)".
    [Test]
    public void TryConvertType_ListOfString_with_NullElement_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(List<string>));

        // When
        TypeUtility.TryConvertType(target, "a,(null),c", out var result).IsTrue();

        // Then
        var list = result.IsInstanceOf<List<string>>();
        list.Count.Is(3);
        list[0].Is("a");
        list[1].IsNull();
        list[2].Is("c");
    }

    // Regression guards: "(null)" must NOT collapse non-nullable value types - the CLR cannot bind null to int/bool
    // Catches a future "simplification" that widens the null shortcut to all types.
    [Test]
    public void TryConvertType_NonNullableInt_from_NullLiteral_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(int));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsFalse();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_NonNullableBool_from_NullLiteral_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(bool));

        // When
        TypeUtility.TryConvertType(target, "(null)", out var result).IsFalse();

        // Then
        result.IsNull();
    }

    [Test]
    public void TryConvertType_String_from_EmptyString_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(string));

        // When
        TypeUtility.TryConvertType(target, "", out var result).IsTrue();

        // Then
        result.Is("");
    }

    [Test]
    public void TryConvertType_String_from_LiteralValue_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(string));

        // When
        TypeUtility.TryConvertType(target, "Hello", out var result).IsTrue();

        // Then
        result.Is("Hello");
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
    }

    [Test]
    public void TryConvertType_StringArray_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(string[]));

        // When
        TypeUtility.TryConvertType(target, "apple%2c orange,banana,cherry 100%25", out var result).IsTrue();

        // Then
        var stringArray = result.IsInstanceOf<string[]>();
        stringArray.Length.Is(3);
        stringArray[0].Is("apple, orange");
        stringArray[1].Is("banana");
        stringArray[2].Is("cherry 100%");
    }

    [Test]
    public void TryConvertType_IntList_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(List<int>));

        // When
        TypeUtility.TryConvertType(target, "1,2,3,4,5", out var result).IsTrue();

        // Then
        var intList = result.IsInstanceOf<List<int>>();
        intList.Count.Is(5);
        intList[0].Is(1);
        intList[1].Is(2);
        intList[2].Is(3);
        intList[3].Is(4);
        intList[4].Is(5);
    }

    [Test]
    public void TryConvertType_DoubleIEnumerable_from_Value_Test()
    {
        // Given
        var target = TypeUtility.ExtractTypeStructure(typeof(IEnumerable<double>));

        // When
        TypeUtility.TryConvertType(target, "1.1,2.2,3.3", out var result).IsTrue();

        // Then
        var doubleList = result.IsInstanceOf<IEnumerable<double>>();
        doubleList.Count().Is(3);
        doubleList.ElementAt(0).Is(1.1);
        doubleList.ElementAt(1).Is(2.2);
        doubleList.ElementAt(2).Is(3.3);
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

    [Test]
    public void IsParsableType_for_int_Test()
    {
        TypeUtility.IsParsableType(typeof(int)).IsTrue();
    }

    [Test]
    public void IsParsableType_for_string_Test()
    {
        TypeUtility.IsParsableType(typeof(TypeUtility)).IsFalse();
    }
}
