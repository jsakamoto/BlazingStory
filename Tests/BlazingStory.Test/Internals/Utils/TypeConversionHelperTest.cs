using System.Text.Json;
using BlazingStory.Internals.Utils;

namespace BlazingStory.Test.Internals.Utils;

[SetCulture("en-US")]
public class TypeConversionHelperTest
{
    [Test]
    public void ConvertToExpectedType_Primitives_Test()
    {
        TypeConversionHelper.ConvertToExpectedType("42", typeof(int)).IsInstanceOf<int>().Is(42);
        TypeConversionHelper.ConvertToExpectedType("true", typeof(bool)).IsInstanceOf<bool>().IsTrue();
        TypeConversionHelper.ConvertToExpectedType("3.5", typeof(double)).IsInstanceOf<double>().Is(3.5d);
        TypeConversionHelper.ConvertToExpectedType("2.5", typeof(float)).IsInstanceOf<float>().Is(2.5f);
        TypeConversionHelper.ConvertToExpectedType("9.99", typeof(decimal)).IsInstanceOf<decimal>().Is(9.99m);
        TypeConversionHelper.ConvertToExpectedType("1234567890123", typeof(long)).IsInstanceOf<long>().Is(1234567890123L);
    }

    [Test]
    public void ConvertToExpectedType_Collections_From_JsonElement_Test()
    {
        using var stringArrayDocument = JsonDocument.Parse("[\"a\",\"b\",\"c\"]");
        using var intArrayDocument = JsonDocument.Parse("[1,2,3]");

        var stringArray = TypeConversionHelper.ConvertToExpectedType(stringArrayDocument.RootElement, typeof(string[]));
        var intList = TypeConversionHelper.ConvertToExpectedType(intArrayDocument.RootElement, typeof(List<int>));

        var stringValues = stringArray.IsInstanceOf<string[]>();
        stringValues.Is("a", "b", "c");

        var intValues = intList.IsInstanceOf<List<int>>();
        intValues.Is(1, 2, 3);
    }

    [Test]
    public void ConvertToExpectedType_Returns_Original_For_Unhandled_Type_Test()
    {
        var original = new object();

        ReferenceEquals(TypeConversionHelper.ConvertToExpectedType(original, typeof(object)), original).IsTrue();
    }

    [Test]
    public void AreValuesEqual_Uses_Deep_Comparison_Test()
    {
        TypeConversionHelper.AreValuesEqual(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }).IsTrue();
        TypeConversionHelper.AreValuesEqual(new[] { 1, 2, 3 }, new[] { 1, 2, 4 }).IsFalse();
    }
}