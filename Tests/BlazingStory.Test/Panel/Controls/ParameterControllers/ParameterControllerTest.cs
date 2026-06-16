using BlazingStory.Abstractions;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;
using BlazingStory.Types;
using Bunit;
using Microsoft.AspNetCore.Components;
using NSubstitute;

namespace BlazingStory.Test.Panel.Controls.ParameterControllers;

/// <summary>
/// Tests for ParameterController component to verify custom fragment routing and context cascading.
/// </summary>
public class ParameterControllerTest
{
    /// <summary>
    /// Verifies that when UserControllerFragment is set, it is rendered instead of built-in controllers.
    /// </summary>
    [Test]
    public void ParameterController_RenderCustomFragment_WhenUserControllerFragmentIsSet()
    {
        // Given
        using var ctx = new BunitContext();

        var customFragmentRendered = false;
        var customFragment = (RenderFragment)(builder =>
        {
            customFragmentRendered = true;
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Custom Fragment");
            builder.CloseElement();
        });

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, "test-key")
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, "test-value"));

        // Then
        customFragmentRendered.IsTrue("Custom fragment should be rendered when UserControllerFragment is set");
        cut.Markup.Normalize().Is("<div>Custom Fragment</div>", "Custom fragment content should be visible in markup");
    }

    /// <summary>
    /// Verifies that ParameterControllerContext is properly cascaded to the custom fragment.
    /// </summary>
    [Test]
    public void ParameterController_CascadeParameterControllerContext_WhenCustomFragmentIsRendered()
    {
        // Given
        using var ctx = new BunitContext();

        var testKey = "test-param-key";
        var testValue = "test-param-value";
        var capturedContext = default(ParameterControllerContext?);

        var customFragment = (RenderFragment)(builder =>
        {
            // This render fragment captures the cascading ParameterControllerContext
            builder.OpenComponent<CascadingParameterCapture>(0);
            var onContextCaptured =
                EventCallback.Factory.Create<ParameterControllerContext?>(new(), (userCtx) => capturedContext = userCtx);
            builder.AddAttribute(1, "OnContextCaptured", onContextCaptured);
            builder.CloseComponent();
        });

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, testKey)
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, testValue));

        // Then
        capturedContext.IsNotNull("ParameterControllerContext should be cascaded to child components");
        capturedContext.Key.Is(testKey, "Context key should match the passed key");
        capturedContext.Value.Is(testValue, "Context value should match the passed value");
    }

    /// <summary>
    /// Verifies that when UserControllerFragment is null, the built-in TextParameterController is used for string types.
    /// </summary>
    [Test]
    public void ParameterController_RenderBuiltInController_WhenUserControllerFragmentIsNull()
    {
        // Given
        using var ctx = new BunitContext();

        var parameter = CreateMockParameter(primaryType: typeof(string), userControllerFragment: null);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, "test-key")
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, "test-value"));

        // Then
        // TextParameterController should be rendered for string type, which renders a TextArea component
        // Look for the component signature that indicates it was rendered
        var textarea = cut.Find("textarea").IsNotNull("TextArea component should be rendered for string type when custom fragment is null");
        textarea.GetAttribute("value").Is("test-value", "TextArea should have the correct value");
    }

    /// <summary>
    /// Verifies that custom fragment takes precedence over built-in controllers even for types that have built-in support.
    /// </summary>
    [Test]
    public void ParameterController_PreferCustomFragment_OverBuiltInControllerForSupportedTypes()
    {
        // Given
        using var ctx = new BunitContext();

        var customFragment = (RenderFragment)(builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Custom Boolean Controller");
            builder.CloseElement();
        });

        // Create a bool parameter with a custom fragment
        var parameter = CreateMockParameter(primaryType: typeof(bool), userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, "test-key")
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, true));

        // Then
        cut.Markup.Normalize().Is("<div>Custom Boolean Controller</div>",
            "Custom fragment should take precedence over built-in BoolParameterController");
        cut.FindComponents<BoolParameterController>().Count.Is(0,
            "Built-in BoolParameterController should not be rendered when custom fragment is present");
    }

    /// <summary>
    /// Verifies that the custom fragment can access all context properties needed for user controller implementation.
    /// </summary>
    [Test]
    public void ParameterController_ContextContainsAllRequiredProperties()
    {
        // Given
        using var ctx = new BunitContext();

        var testKey = "param-1";
        var testValue = 42;
        var capturedContext = default(ParameterControllerContext?);

        var componentInstance = new object();
        var customFragment = (RenderFragment)(builder =>
        {
            builder.OpenComponent<CascadingParameterCapture>(0);
            var onContextCaptured =
                EventCallback.Factory.Create<ParameterControllerContext?>(componentInstance, (userCtx) => capturedContext = userCtx);
            builder.AddAttribute(1, "OnContextCaptured", onContextCaptured);
            builder.CloseComponent();
        });

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, testKey)
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, testValue));

        // Then
        // Verify the context is available and contains all required properties
        capturedContext.IsNotNull("Context should be available");
        capturedContext.Key.Is(testKey, "Context key should be set");
        capturedContext.Value?.Equals(testValue).IsTrue("Context value should be set");
        capturedContext.Parameter.IsNotNull("Context parameter should be set");
        // The OnInput callback is set by ParameterController, even if empty
        capturedContext.OnInput.IsNotNull("Context OnInput callback should be set");
    }

    // Helper method to create a mock IComponentParameter
    private static IComponentParameter CreateMockParameter(
        Type? primaryType = null,
        RenderFragment? userControllerFragment = null)
    {
        primaryType ??= typeof(string);

        var typeStructure = new TypeStructure(isNullable: false, isGeneric: false, primaryType, Array.Empty<Type>());

        var parameter = Substitute.For<IComponentParameter>();
        parameter.Name.Returns("TestParameter");
        parameter.Type.Returns(primaryType);
        parameter.TypeStructure.Returns(typeStructure);
        parameter.Control.Returns(ControlType.Default);
        parameter.UserControllerFragment.Returns(userControllerFragment);
        parameter.Summary.Returns(new MarkupString("Test parameter summary"));
        parameter.Required.Returns(false);
        parameter.DefaultValue.Returns(null);

        return parameter;
    }
}

/// <summary>
/// Helper component that captures the cascading ParameterControllerContext for testing purposes.
/// </summary>
internal class CascadingParameterCapture : ComponentBase
{
    [CascadingParameter]
    public ParameterControllerContext? Context { get; set; }

    [Parameter]
    public EventCallback<ParameterControllerContext?> OnContextCaptured { get; set; }

    protected override Task OnInitializedAsync()
    {
        return this.OnContextCaptured.InvokeAsync(this.Context);
    }
}

