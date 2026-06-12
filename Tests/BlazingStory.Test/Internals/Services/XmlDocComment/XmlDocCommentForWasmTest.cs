using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.DependencyInjection;
using RazorClassLib1.Components.Button;
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
