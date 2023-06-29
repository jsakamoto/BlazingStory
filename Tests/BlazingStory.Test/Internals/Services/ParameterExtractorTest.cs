using System.Linq.Expressions;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using RazorClassLib1.Components.Button;

namespace BlazingStory.Test.Internals.Services;

internal class ParameterExtractorTest
{
    [Test]
    public void GetParameterName_From_Expression_Test()
    {
        Expression<Func<Button, ButtonColor>> expression = (Button _) => _.Color;
        var parameterName = ParameterExtractor.GetParameterName(expression);
        parameterName.Is("Color");
    }

    [Test]
    public async Task GetParametersFromComponentType_Test()
    {
        var xmlDocComment = new XmlDocCommentForWasm(XmlDocCommentLoader.CreateHttpClientFor<Button>(), NullLogger<XmlDocCommentForWasm>.Instance);
        var parameters = ParameterExtractor.GetParametersFromComponentType(typeof(Button), xmlDocComment);
        foreach (var item in parameters)
        {
            await item.UpdateSummaryFromXmlDocCommentAsync();
        }

        parameters
            .Select(p => $"{p.Name}, {p.Type.Name}, {p.Required}, {p.Summary}")
            .Is("Bold, Boolean, False, ",
                "Text, String, True, Set a text that is button caption.",
                "Color, ButtonColor, False, Set a color of the button. \"ButtonColor.Default\" is default.",
                "Size, ButtonSize, False, Set a size of the button. \"ButtonSize.Medium\" is default.",
                "OnClick, EventCallback`1, False, Set a callback method that will be invoked when users click the button.");
    }
}
