using BlazingStory.ToolKit.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.ToolKit.Utils;

public class RenderFragmentKitTest
{
    [Test]
    public async Task ToMarkupStringAsync_For_RenderFragmentT_Test()
    {
        // Given
        RenderFragment<DateTime> renderFragment = (arg) => (builder) => builder.AddContent(0, "Dolor sit errata");

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("Dolor sit errata");
    }

    [Test]
    public async Task ToMarkupStringAsync_For_Nested_RenderFragment_TestAsync()
    {
        // Given
        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Lorem ipsum");
            builder.CloseElement();
        };

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("<div>Lorem ipsum</div>");
    }

    [Test]
    public async Task ToMarkupStringAsync_For_RenderFragment_With_Attributes_Test()
    {
        // Given
        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "test-class");
            builder.AddAttribute(2, "id", "test-id");
            builder.AddContent(3, "Hello, World!");
            builder.CloseElement();
        };

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("<div class=\"test-class\" id=\"test-id\">Hello, World!</div>");
    }

    [Test]
    public async Task ToMarkupStringAsync_For_RenderFragment_With_Dynamic_Attributes_Test()
    {
        // Given
        var attributes = new Dictionary<string, object>
        {
            { "class", "dynamic-class" },
            { "style", "color: red;" }
        };

        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "span");
            builder.AddMultipleAttributes(1, attributes);
            builder.AddContent(2, "Dynamic content");
            builder.CloseElement();
        };

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("<span class=\"dynamic-class\" style=\"color: red;\">Dynamic content</span>");
    }

    [Test]
    public async Task ToMarkupStringAsync_For_RenderFragment_With_Boolean_Attribute_Test()
    {
        // Given
        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "checkbox");
            builder.AddAttribute(2, "checked", true);
            builder.CloseElement();
        };

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("<input type=\"checkbox\" checked />");
    }

    [Test]
    public async Task ToMarkupStringAsync_For_RenderFragment_With_Conditional_Attribute_Test()
    {
        // Given
        var condition = true;

        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "div");
            if (condition)
            {
                builder.AddAttribute(1, "class", "active");
            }
            builder.AddContent(2, "Conditional content");
            builder.CloseElement();
        };

        // When
        var str = await renderFragment.ToMarkupStringAsync();

        // Then
        str.Is("<div class=\"active\">Conditional content</div>");
    }
}
