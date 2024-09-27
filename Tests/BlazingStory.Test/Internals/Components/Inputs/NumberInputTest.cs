using BlazingStory.Internals.Services;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Components.Inputs;

public class NumberInputTest
{
    [Test]
    public void NumberInput_Step_with_Empty_Test()
    {
        // Given
        using var ctx = new Bunit.TestContext();

        // When
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.Inputs.NumberInput>(builder => builder
            .Add(_ => _.AllowDecimalPoint, true)
            .Add(_ => _.Value, ""));

        // Then
        cut.Find("input").GetAttribute("step").Is("1");
    }

    [Test]
    public void NumberInput_Step_with_IntegerText_Test()
    {
        // Given
        using var ctx = new Bunit.TestContext();

        // When
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.Inputs.NumberInput>(builder => builder
            .Add(_ => _.AllowDecimalPoint, true)
            .Add(_ => _.Value, "321"));

        // Then
        cut.Find("input").GetAttribute("step").Is("1");
    }

    [Test]
    public void NumberInput_Step_with_DecimalText_Test()
    {
        // Given
        using var ctx = new Bunit.TestContext();

        // When
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.Inputs.NumberInput>(builder => builder
            .Add(_ => _.AllowDecimalPoint, true)
            .Add(_ => _.Value, "1.234"));

        // Then
        cut.Find("input").GetAttribute("step").Is("0.001");
    }

    [Test]
    public void NumberInput_Step_KeepPrecision_IncrementalSpin_Test()
    {
        // Given
        using var ctx = new Bunit.TestContext();

        // When: null value -> Then: the step is 1.
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.Inputs.NumberInput>(builder => builder
            .Add(_ => _.AllowDecimalPoint, true)
            .Add(_ => _.Value, null));
        cut.Find("input").GetAttribute("step").Is("1");

        // When: input text "1.29" for each typing -> Then: the step changes 1, 0.1, and 0.01.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1"));
        cut.Find("input").GetAttribute("step").Is("1");
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1."));
        cut.Find("input").GetAttribute("step").Is("0.1");
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.2"));
        cut.Find("input").GetAttribute("step").Is("0.1");
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.29"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: emulate incremental spin button -> Then: keep the step even though the decimal point length is descreased.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.3"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: redrawn was happened -> Then: keep the step.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.3"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: emulate incremental spin button -> Then: keep the step.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.31"));
        cut.Find("input").GetAttribute("step").Is("0.01");
    }

    [Test]
    public void NumberInput_Step_KeepPrecision_DecrementalSpin_Test()
    {
        // Given
        using var ctx = new Bunit.TestContext();

        // When: input "1.21" -> Then: the step is 0.01.
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.Inputs.NumberInput>(builder => builder
            .Add(_ => _.AllowDecimalPoint, true)
            .Add(_ => _.Value, "1.21"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: emulate decremental spin button -> Then: keep the step even though the decimal point length is descreased.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.2"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: redrawn was happened -> Then: keep the step.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.2"));
        cut.Find("input").GetAttribute("step").Is("0.01");

        // When: emulate decremental spin button -> Then: keep the step.
        cut.SetParametersAndRender(builder => builder.Add(_ => _.Value, "1.19"));
        cut.Find("input").GetAttribute("step").Is("0.01");
    }
}
