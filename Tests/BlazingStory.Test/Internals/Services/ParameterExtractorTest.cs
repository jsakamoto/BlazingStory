using System.Linq.Expressions;
using BlazingStory.Internals.Services;
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
    public void GetParametersFromComponentType_Test()
    {
        var parameters = ParameterExtractor.GetParametersFromComponentType(typeof(Button));
        parameters
            .Select(p => $"{p.Name}, {p.Type.Name}, {p.Required}, {p.Summary}")
            .Is("Text, String, True, Set a text that is button caption.",
                "Color, ButtonColor, False, Set a color of the button.",
                "OnClick, EventCallback`1, False, Set a callback method that will be invoked when users click the button.");
    }
}
