using BlazingStory.Internals.Services.XmlDocComment;
using RazorClassLib1.Components.MySelect;

namespace BlazingStory.Test.Internals.Services.XmlDocComment;

internal class CrefTextTest
{
    [Test]
    [TestCase("T:System.String", "String")]
    [TestCase("F:RazorClassLib1.Components.ButtonColor.Default", "ButtonColor.Default")]
    [TestCase("T:RazorClassLib1.Components.Button.Button", "Button")]
    [TestCase("M:Foo.Bar.Fizz(System.String,System.Int32)", "Bar.Fizz")]
    [TestCase("", "")]
    public void GetDisplayText_NonGeneric_Test(string cref, string expected)
    {
        CrefText.GetDisplayText(cref, contextType: null).Is(expected);
    }

    [Test]
    [TestCase("P:RazorClassLib1.Components.MySelect.MySelect`2.Multiselect", "MySelect<TKey, TValue>.Multiselect")]
    [TestCase("T:RazorClassLib1.Components.MySelect.MySelect`2", "MySelect<TKey, TValue>")]
    public void GetDisplayText_ResolvesTypeParameterNames_Of_ContextType_Test(string cref, string expected)
    {
        CrefText.GetDisplayText(cref, typeof(MySelect<string, int>)).Is(expected);
    }

    [Test]
    public void GetDisplayText_DropsArity_When_TypeParameterNames_Are_Unknown_Test()
    {
        // The documentation identifier of a type records its arity, but not the names of its type parameters,
        // so the names of a type other than the context type can not be recovered.
        CrefText.GetDisplayText("T:System.Collections.Generic.Dictionary`2", typeof(MySelect<string, int>))
            .Is("Dictionary");
    }

    [Test]
    [TestCase("M:Probe.MySelect`2.Convert``1(``0)", "MySelect.Convert")]
    [TestCase("T:System.Collections.Generic.Dictionary{System.String,System.Int32}", "Dictionary<String, Int32>")]
    [TestCase("T:System.Collections.Generic.List{System.Collections.Generic.List{System.String}}", "List<List<String>>")]
    public void GetDisplayText_GenericNotation_Test(string cref, string expected)
    {
        CrefText.GetDisplayText(cref, contextType: null).Is(expected);
    }

    [Test]
    public void GetDisplayText_Unresolved_Cref_Test()
    {
        // The compiler emits a "!:" prefix with the source text as-is when it can not resolve a "cref".
        CrefText.GetDisplayText("!:Dictionary<string, int>", contextType: null).Is("Dictionary<string, int>");
    }
}
