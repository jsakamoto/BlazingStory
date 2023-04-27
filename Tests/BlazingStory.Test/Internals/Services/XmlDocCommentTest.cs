using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.DependencyInjection;
using RazorClassLib1.Components.Button;

namespace BlazingStory.Test.Internals.Services;

internal class XmlDocCommentTest
{
    [Test]
    public async Task GetSummaryOfProperty_Test()
    {
        await using var host = new TestHost(services =>
        {
            services.AddSingleton(_ => XmlDocCommentLoader.CreateHttpClientFor<Button>());
            services.AddSingleton<IXmlDocComment, XmlDocCommentForWasm>();
        });

        var xmlDocComment = host.Services.GetRequiredService<IXmlDocComment>();
        var summary = await xmlDocComment.GetSummaryOfPropertyAsync(typeof(Button), nameof(Button.Text));

        summary.Is("Set a text that is button caption.");
    }
}
