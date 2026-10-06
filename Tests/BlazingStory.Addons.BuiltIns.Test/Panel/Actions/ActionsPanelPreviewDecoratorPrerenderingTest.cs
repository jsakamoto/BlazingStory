using BlazingStory.Addons.BuiltIns.Panel.Actions;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Actions;

public class ActionsPanelPreviewDecoratorPrerenderingTest
{
    private static (BunitContext Context, IJSRuntime JSRuntime, IJSObjectReference Module) CreateContext()
    {
        var module = Substitute.For<IJSObjectReference>();
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(_ => new ValueTask<IJSObjectReference>(module));

        var ctx = new BunitContext();
        ctx.Services.AddScoped(_ => jsRuntime);
        return (ctx, jsRuntime, module);
    }

    private static string?[] GetDispatchedActionNames(IJSObjectReference module)
    {
        return module.ReceivedCalls()
            .Select(call => call.GetArguments())
            .Where(args => args.Length > 1 && (string?)args[0] == "dispatchComponentActionEvent")
            .Select(args => (args[1] as object?[])?[0] as string)
            .ToArray();
    }

    [Test]
    public async Task EventTArgMonitorHandler_BeforeFirstRender_DoesNotInvokeJavaScript_Test()
    {
        // Given
        // A component that has not been rendered yet is in the same situation as one being prerendered on the server:
        // JavaScript interop is not available.
        var (ctx, jsRuntime, module) = CreateContext();
        using var _ = ctx;
        var decorator = new ActionsPanelPreviewDecorator();
        ctx.ComponentFactories.Add(type => type == typeof(ActionsPanelPreviewDecorator), _ => decorator);

        // When
        await decorator.EventTArgMonitorHandler("OnClick", "args");

        // Then
        jsRuntime.ReceivedCalls().Count().Is(0);
    }

    [Test]
    public async Task EventTArgMonitorHandler_BeforeFirstRender_DispatchesTheActionAfterFirstRender_Test()
    {
        // Given
        var (ctx, _, module) = CreateContext();
        using var __ = ctx;
        var decorator = new ActionsPanelPreviewDecorator();
        ctx.ComponentFactories.Add(type => type == typeof(ActionsPanelPreviewDecorator), _ => decorator);
        await decorator.EventTArgMonitorHandler("OnInitialized", "args");

        // When
        var cut = ctx.Render<ActionsPanelPreviewDecorator>();

        // Then
        cut.WaitForAssertion(() => GetDispatchedActionNames(module).Is("OnInitialized"));
    }

    [Test]
    public async Task EventTArgMonitorHandler_AfterFirstRender_DispatchesTheActionImmediately_Test()
    {
        // Given
        var (ctx, _, module) = CreateContext();
        using var __ = ctx;
        var cut = ctx.Render<ActionsPanelPreviewDecorator>();

        // When
        await cut.InvokeAsync(() => cut.Instance.EventTArgMonitorHandler("OnClick", "args"));

        // Then
        GetDispatchedActionNames(module).Is("OnClick");
    }
}
