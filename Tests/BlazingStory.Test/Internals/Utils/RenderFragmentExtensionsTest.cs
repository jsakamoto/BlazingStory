using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Internals.Utils;

public class RenderFragmentExtensionsTest
{
    [Test]
    public void ToString_For_RenderFragmentT_Test()
    {
        // Given
        RenderFragment<DateTime> renderFragment = (arg) => (builder) => builder.AddContent(0, "Dolor sit errata");

        // When
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("Dolor sit errata");

        Assert.Pass();
    }

    [Test]
    public void TryToString_For_RenderFragmentT_Test()
    {
        // Given
        RenderFragment<string> renderFragment = (arg) => (builder) => builder.AddContent(0, "Ipsum feudist est");

        // When
        RenderFragmentExtensions.TryToString(renderFragment, out var str).IsTrue();

        // Then
        str.Is("Ipsum feudist est");

        Assert.Pass();
    }

    [Test]
    public void ToString_For_Nested_RenderFragment_Test()
    {
        // Given
        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Lorem ipsum");
            builder.CloseElement();
        };

        // When
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("<div>Lorem ipsum</div>");

        Assert.Pass();
    }

    [Test]
    public void TryToString_For_Empty_RenderFragment_Test()
    {
        // Given
        RenderFragment renderFragment = (builder) => { };

        // When
        RenderFragmentExtensions.TryToString(renderFragment, out var str).IsTrue();

        // Then
        str.Is("");

        Assert.Pass();
    }

    [Test]
    public void TryToString_For_Null_RenderFragment_Test()
    {
        // Given
        RenderFragment? renderFragment = null;

        // When
        RenderFragmentExtensions.TryToString(renderFragment, out var str).IsFalse();

        // Then
        str.IsNull();

        Assert.Pass();
    }

    [Test]
    public void TryToString_For_RenderFragment_With_Multiple_Elements_Test()
    {
        // Given
        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "span");
            builder.AddContent(1, "First");
            builder.CloseElement();
            builder.OpenElement(2, "span");
            builder.AddContent(3, "Second");
            builder.CloseElement();
        };

        // When
        RenderFragmentExtensions.TryToString(renderFragment, out var str).IsTrue();

        // Then
        str.Is("<span>First</span><span>Second</span>");

        Assert.Pass();
    }

    [Test]
    public void ToString_For_RenderFragment_With_Attributes_Test()
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
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("<div class=\"test-class\" id=\"test-id\">Hello, World!</div>");

        Assert.Pass();
    }

    [Test]
    public void ToString_For_RenderFragment_With_Dynamic_Attributes_Test()
    {
        // Given
        var attributes = new List<KeyValuePair<string, object>>
        {
            new("class", "dynamic-class"),
            new("style", "color: red;")
        };

        RenderFragment renderFragment = (builder) =>
        {
            builder.OpenElement(0, "span");

            if (attributes != null)
            {
                builder.AddMultipleAttributes(1, attributes);
                builder.AddContent(2, "Dynamic content");
            }
            else
            {
                builder.AddContent(1, "Default content");
            }
            builder.CloseElement();
        };

        // When
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("<span class=\"dynamic-class\" style=\"color: red;\">Dynamic content</span>");

        Assert.Pass();
    }

    [Test]
    public void ToString_For_RenderFragment_With_Boolean_Attribute_Test()
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
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("<input type=\"checkbox\" checked=\"True\"></input>");

        Assert.Pass();
    }

    [Test]
    public void ToString_For_RenderFragment_With_Conditional_Attribute_Test()
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
        var str = renderFragment.ToMarkupString();

        // Then
        str.Is("<div class=\"active\">Conditional content</div>");

        Assert.Pass();
    }
}
