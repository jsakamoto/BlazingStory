using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;
using BlazingStory.Test._Fixtures.Components;
using Bunit;

namespace BlazingStory.Test.Internals.Components.ParameterControllers;

public class ArrayParameterControllerTest
{
    [Test]
    public void ArrayParameterController_Render_IntArray_ShowsNumberInputs_Test()
    {
        // Given
        using var ctx = new BunitContext();
        var parameter = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Numbers));

        // When
        var cut = ctx.Render<ArrayParameterController>(builder => builder
            .Add(_ => _.Key, "numbers")
            .Add(_ => _.Parameter, parameter)
            .Add(_ => _.Value, new[] { 1, 2 }));

        // Then
        cut.FindAll("input[type='number']").Count.Is(2);
    }
}
