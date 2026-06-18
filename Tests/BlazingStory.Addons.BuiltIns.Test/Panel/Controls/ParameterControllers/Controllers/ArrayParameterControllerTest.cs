using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;
using BlazingStory.Addons.BuiltIns.Test._Fixtures.Components;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Controls.ParameterControllers.Controllers;

public class ArrayParameterControllerTest
{
    [Test]
    public void ArrayParameterController_Render_IntArray_ShowsNumberInputs_Test()
    {
        // Given
        using var ctx = new BunitContext();
        var parameter = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Numbers));
        var parameterContext = new ParameterControllerContext("numbers", parameter, new[] { 1, 2, 3 }, EventCallback.Factory.Create<ParameterInputEventArgs>(new(), _ => { }));

        // When
        var cut = ctx.Render<CascadingValue<ParameterControllerContext>>(parameters => parameters
            .Add(p => p.Value, parameterContext)
            .AddChildContent<ArrayParameterController>());

        // Then
        cut.FindAll("input[type='number']").Count.Is(3);
    }
}
