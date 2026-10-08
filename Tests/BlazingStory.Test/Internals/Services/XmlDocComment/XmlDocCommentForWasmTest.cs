using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.DependencyInjection;
using RazorClassLib1.Components.Button;
using RazorClassLib1.Components.MySelect;
using RazorClassLib1.Components.TextInput;

namespace BlazingStory.Test.Internals.Services.XmlDocComment;

internal class XmlDocCommentForWasmTest
{
    private static TestHost CreateTestHost() => new(services =>
    {
        services.AddSingleton(_ => XmlDocCommentLoaderFromOutDir.CreateHttpClient());
        services.AddSingleton<IXmlDocComment, XmlDocCommentForWasm>();
    });

    [Test]
    public async Task GetSummaryOfProperty_Test()
    {
        // Given
        await using var host = CreateTestHost();
        var xmlDocComment = host.Services.GetRequiredService<IXmlDocComment>();

        // When
        var summary = await xmlDocComment.GetSummaryOfPropertyAsync(typeof(Button), nameof(Button.Text));

        // Then
        summary.Value.Is("Set a text that is button caption.");
    }

    [Test]
    [TestCase(nameof(MySelect<string, int>.Multiselect), "Allows selecting more than one item. The default is false.")]
    [TestCase(nameof(MySelect<string, int>.Value), "The selected value. If \"MySelect&lt;TKey, TValue&gt;.Multiselect\" is true, this property will be ignored.")]
    [TestCase(nameof(MySelect<string, int>.Items), "The items to select from, keyed by \"Dictionary\".")]
    // An element nested inside another element, such as this "see" inside a "para", must not be dropped.
    [TestCase(nameof(MySelect<string, int>.Placeholder), "The placeholder text. Set null to hide it.")]
    public async Task GetSummaryOfProperty_OnGenericComponent_Test(string propertyName, string expected)
    {
        // Given
        await using var host = CreateTestHost();
        var xmlDocComment = host.Services.GetRequiredService<IXmlDocComment>();

        // When
        var summary = await xmlDocComment.GetSummaryOfPropertyAsync(typeof(MySelect<string, int>), propertyName);

        // Then
        summary.Value.Is(expected);
    }

    [Test]
    public async Task GetSummaryOfGenericType_Test()
    {
        // Given
        await using var host = CreateTestHost();
        var xmlDocComment = host.Services.GetRequiredService<IXmlDocComment>();

        // When
        var summary = await xmlDocComment.GetSummaryOfTypeAsync(typeof(MySelect<string, int>));

        // Then
        summary.Value.Is("A generic select component. See also \"MySelect&lt;TKey, TValue&gt;.Value\".");
    }

    [Test]
    public async Task GetSummaryOfGenericComponent_Test()
    {
        // Given
        await using var host = CreateTestHost();
        var xmlDocComment = host.Services.GetRequiredService<IXmlDocComment>();

        // When
        var summary = await xmlDocComment.GetSummaryOfTypeAsync(typeof(TextInputBase<string>));

        // Then
        summary.Value.Is("A base class for text input components.");
    }
}
