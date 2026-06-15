using BlazingStory.Abstractions;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;
using BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.UserControllers;
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
        RenderFragment customFragment = (builder) =>
        {
            customFragmentRendered = true;
            builder.AddContent(0, "Custom Fragment");
        };

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, "test-key")
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, "test-value"));

        // Then
        customFragmentRendered.Is(true, "Custom fragment should be rendered when UserControllerFragment is set");
        cut.Markup.Contains("Custom Fragment").Is(true, "Custom fragment content should be visible in markup");
    }

    /// <summary>
    /// Verifies that UserControllerContext is properly cascaded to the custom fragment.
    /// </summary>
    [Test]
    public void ParameterController_CascadeUserControllerContext_WhenCustomFragmentIsRendered()
    {
        // Given
        using var ctx = new BunitContext();

        var testKey = "test-param-key";
        var testValue = "test-param-value";
        UserControllerContext? capturedContext = null;

        // Use a dummy object as the component instance
        var componentInstance = new object();

        RenderFragment customFragment = (builder) =>
        {
            // This render fragment captures the cascading UserControllerContext
            builder.OpenComponent<CascadingParameterCapture>(0);
            var onContextCaptured = 
                EventCallback.Factory.Create<UserControllerContext?>(componentInstance, (userCtx) => capturedContext = userCtx);
            builder.AddAttribute(1, "OnContextCaptured", onContextCaptured);
            builder.CloseComponent();
        };

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, testKey)
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, testValue));

        // Then
        capturedContext.IsNotNull("UserControllerContext should be cascaded to child components");
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
        cut.Markup.Contains("textarea").Is(true, "TextArea component should be rendered for string type when custom fragment is null");
    }

    /// <summary>
    /// Verifies that custom fragment takes precedence over built-in controllers even for types that have built-in support.
    /// </summary>
    [Test]
    public void ParameterController_PreferCustomFragment_OverBuiltInControllerForSupportedTypes()
    {
        // Given
        using var ctx = new BunitContext();

        var customFragmentContent = "Custom Boolean Controller";
        RenderFragment customFragment = (builder) =>
        {
            builder.AddContent(0, customFragmentContent);
        };

        // Create a bool parameter with a custom fragment
        var parameter = CreateMockParameter(primaryType: typeof(bool), userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, "test-key")
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, true));

        // Then
        cut.Markup.Contains(customFragmentContent).Is(true,
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
        UserControllerContext? capturedContext = null;

        var componentInstance = new object();
        RenderFragment customFragment = (builder) =>
        {
            builder.OpenComponent<CascadingParameterCapture>(0);
            var onContextCaptured = 
                EventCallback.Factory.Create<UserControllerContext?>(componentInstance, (userCtx) => capturedContext = userCtx);
            builder.AddAttribute(1, "OnContextCaptured", onContextCaptured);
            builder.CloseComponent();
        };

        var parameter = CreateMockParameter(userControllerFragment: customFragment);

        // When
        var cut = ctx.Render<ParameterController>(builder => builder
            .Add(c => c.Key, testKey)
            .Add(c => c.Parameter, parameter)
            .Add(c => c.Value, testValue));

        // Then
        // Verify the context is available and contains all required properties
        capturedContext.IsNotNull("Context should be available");
        capturedContext.Key.Is(testKey, "Context key should be set");
        capturedContext.Value?.Equals(testValue).Is(true, "Context value should be set");
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
/// Helper component that captures the cascading UserControllerContext for testing purposes.
/// </summary>
internal class CascadingParameterCapture : ComponentBase
{
    [CascadingParameter(Name = "Context")]
    public UserControllerContext? Context { get; set; }

    [Parameter]
    public EventCallback<UserControllerContext?> OnContextCaptured { get; set; }

    protected override Task OnInitializedAsync()
    {
        return this.OnContextCaptured.InvokeAsync(this.Context);
    }
}

