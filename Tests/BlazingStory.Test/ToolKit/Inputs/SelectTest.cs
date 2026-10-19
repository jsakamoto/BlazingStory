using BlazingStory.ToolKit.Inputs;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;

namespace BlazingStory.Test.ToolKit.Inputs;

internal class SelectTest
{
    /// <summary>
    /// 25 items, enough to make the Select component searchable.
    /// </summary>
    private static readonly string[] _Items = Enumerable.Range(1, 25).Select(n => $"Item {n:D2}").ToArray();

    private static BunitContext CreateContext()
    {
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(_ => new ValueTask<IJSObjectReference>(Substitute.For<IJSObjectReference>()));

        var ctx = new BunitContext();
        ctx.Services.AddScoped(_ => jsRuntime);
        return ctx;
    }

    private static IRenderedComponent<Select> RenderSelect(BunitContext ctx, string? value = null, Action<ChangeEventArgs>? onChange = null)
    {
        return ctx.Render<Select>(parameters => parameters
            .Add(p => p.Items, _Items)
            .Add(p => p.Value, value)
            .Add(p => p.OnChange, args => onChange?.Invoke(args)));
    }

    private static string? GetHighlightedText(IRenderedComponent<Select> cut)
    {
        return cut.FindAll(".searchable-select-option.highlighted").SingleOrDefault()?.TextContent.Trim();
    }

    [Test]
    public async Task KeyDown_F4_TogglesDropdown_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);
        var input = cut.Find("input");
        cut.FindAll(".searchable-select-dropdown").Count.Is(0);

        // When / Then
        await input.KeyDownAsync(new KeyboardEventArgs { Key = "F4" });
        cut.FindAll(".searchable-select-dropdown").Count.Is(1);
        cut.Find("input").GetAttribute("aria-expanded").Is("true");

        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });
        cut.FindAll(".searchable-select-dropdown").Count.Is(0);
        cut.Find("input").GetAttribute("aria-expanded").Is("false");
    }

    [Test]
    public async Task KeyDown_AltArrowDown_OpensDropdown_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown", AltKey = true });

        // Then
        cut.FindAll(".searchable-select-dropdown").Count.Is(1);
        GetHighlightedText(cut).IsNull();
    }

    [Test]
    public async Task KeyDown_AltArrowUp_ClosesDropdown_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowUp", AltKey = true });

        // Then
        cut.FindAll(".searchable-select-dropdown").Count.Is(0);
    }

    [Test]
    public async Task OpenDropdown_HighlightsSelectedOption_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx, value: "Item 07");

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });

        // Then
        GetHighlightedText(cut).Is("Item 07");
        var highlightedId = cut.Find(".searchable-select-option.highlighted").Id;
        cut.Find("input").GetAttribute("aria-activedescendant").Is(highlightedId);
    }

    [Test]
    public async Task KeyDown_ArrowDownAndArrowUp_MoveHighlight_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });

        // When / Then
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        GetHighlightedText(cut).Is("Item 01");

        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        GetHighlightedText(cut).Is("Item 02");

        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowUp" });
        GetHighlightedText(cut).Is("Item 01");
    }

    [Test]
    public async Task KeyDown_ArrowUpAndArrowDown_WrapAround_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });

        // When / Then
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowUp" });
        GetHighlightedText(cut).Is("Item 25");

        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        GetHighlightedText(cut).Is("Item 01");
    }

    [Test]
    public async Task KeyDown_Enter_SelectsHighlightedOption_Test()
    {
        // Given
        using var ctx = CreateContext();
        var selectedValues = new List<object?>();
        var cut = RenderSelect(ctx, onChange: args => selectedValues.Add(args.Value));
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "Enter" });

        // Then
        selectedValues.Is("Item 02");
        cut.FindAll(".searchable-select-dropdown").Count.Is(0);
    }

    [Test]
    public async Task KeyDown_F4_AfterSelecting_ShowsAllOptions_Test()
    {
        // Given
        using var ctx = CreateContext();
        var cut = RenderSelect(ctx);
        await cut.Find("input").InputAsync(new ChangeEventArgs { Value = "Item 2" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "Enter" });

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "F4" });

        // Then
        cut.FindAll(".searchable-select-option").Count.Is(_Items.Length);
    }

    [Test]
    public async Task KeyDown_ArrowDown_AfterFiltering_MovesWithinFilteredOptions_Test()
    {
        // Given
        using var ctx = CreateContext();
        var selectedValues = new List<object?>();
        var cut = RenderSelect(ctx, onChange: args => selectedValues.Add(args.Value));
        await cut.Find("input").InputAsync(new ChangeEventArgs { Value = "Item 2" });
        GetHighlightedText(cut).IsNull();

        // When
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
        await cut.Find("input").KeyDownAsync(new KeyboardEventArgs { Key = "Enter" });

        // Then
        selectedValues.Is("Item 21");
    }
}
