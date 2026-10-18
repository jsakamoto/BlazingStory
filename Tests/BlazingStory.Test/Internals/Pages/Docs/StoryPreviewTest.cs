using BlazingStory.Internals.Components.Layouts;
using BlazingStory.Internals.Pages.Docs;
using BlazingStory.Test._Fixtures;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;
using RazorClassLib1.Components.Button;

namespace BlazingStory.Test.Internals.Pages.Docs;

internal class StoryPreviewTest
{
    private static BunitContext CreateContext()
    {
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(_ => new ValueTask<IJSObjectReference>(Substitute.For<IJSObjectReference>()));

        var ctx = new BunitContext();
        ctx.Services.AddScoped(_ => jsRuntime);
        ctx.ComponentFactories.AddStub<PooledIFrame>();
        ctx.ComponentFactories.AddStub<CodeView>();
        return ctx;
    }

    [Test]
    public void Render_ToolbarEnabled_ShowsOpenCanvasInNewTabButton_Test()
    {
        // Given
        using var ctx = CreateContext();
        var story = TestHelper.CreateStory<Button>();

        // When
        var cut = ctx.Render<StoryPreview>(parameters => parameters
            .Add(p => p.Story, story)
            .Add(p => p.EnableZoom, true));

        // Then
        cut.FindAll("[title='Open canvas in new tab']").Count.Is(1);
    }

    [Test]
    public void Render_ToolbarDisabled_DoesNotShowOpenCanvasInNewTabButton_Test()
    {
        // Given
        using var ctx = CreateContext();
        var story = TestHelper.CreateStory<Button>();

        // When
        var cut = ctx.Render<StoryPreview>(parameters => parameters
            .Add(p => p.Story, story)
            .Add(p => p.EnableZoom, false));

        // Then
        cut.FindAll("[title='Open canvas in new tab']").Count.Is(0);
    }
}
