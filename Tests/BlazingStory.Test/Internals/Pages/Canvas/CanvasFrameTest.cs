using System.Runtime.CompilerServices;
using BlazingStory.Internals.Pages.Canvas;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazingStory.Test.Internals.Pages.Canvas;

public class CanvasFrameTest
{
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    private extern static void set_JSRuntime(CanvasFrame @this, IJSRuntime jsRuntime);

    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    private extern static void set_NavigationManager(CanvasFrame @this, NavigationManager navigationManager);

    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    private extern static void OnInitialized(CanvasFrame @this);

    private class CanvasFrameTestContext : IAsyncDisposable
    {
        public Bunit.TestContext Context { get; }

        public CanvasFrame CanvasFrame { get; }

        public BunitJSModuleInterop JSModule { get; }

        public CanvasFrameTestContext()
        {
            this.Context = new Bunit.TestContext();
            this.Context.JSInterop.Mode = JSRuntimeMode.Loose;
            this.Context.JSInterop.Setup<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine").SetResult(false);
            this.JSModule = this.Context.JSInterop.SetupModule("./_content/BlazingStory/Internals/Pages/Canvas/CanvasFrame.razor.js");

            this.CanvasFrame = new CanvasFrame();
            set_JSRuntime(this.CanvasFrame, this.Context.JSInterop.JSRuntime);
            set_NavigationManager(this.CanvasFrame, this.Context.Services.GetRequiredService<NavigationManager>());
            OnInitialized(this.CanvasFrame);
        }

        public async ValueTask DisposeAsync() { await this.CanvasFrame.DisposeAsync(); this.Context.Dispose(); }
    }

    [Test]
    public async Task EventTArgMonitorHandler_Test()
    {
        // GIVEN
        await using var context = new CanvasFrameTestContext();

        // WHEN
        var eventArgs = new KeyboardEventArgs
        {
            Key = "A",
            Code = "KeyA",
            Location = 0,
            Repeat = false,
            CtrlKey = true,
            ShiftKey = false,
            AltKey = false,
            MetaKey = false,
            Type = "keydown"
        };
        await context.CanvasFrame.EventTArgMonitorHandler("OnClick", eventArgs);

        // THEN
        context.JSModule.VerifyInvoke("dispatchComponentActionEvent").Arguments.Is(
            "OnClick",
            """
            {
              "Key": "A",
              "Code": "KeyA",
              "Location": 0,
              "Repeat": false,
              "CtrlKey": true,
              "ShiftKey": false,
              "AltKey": false,
              "MetaKey": false,
              "Type": "keydown"
            }
            """);
    }

    public class TestEvent : EventArgs
    {
        public RenderFragment Fragment { get; init; } = builder => { builder.AddMarkupContent(0, "<div>Test</div>"); };
        public RenderFragment<int> FragmentWithArg { get; init; } = (int n) => builder => { builder.AddMarkupContent(0, $"<div>{n}</div>"); };
        public Action SystemAction { get; init; } = () => { };
        public Action<int, string> SystemActionWithArg { get; init; } = (int n, string s) => { };
        public Func<bool> SystemFunc { get; init; } = () => true;
        public Func<int, string, bool> SystemFuncWithArg { get; init; } = (int n, string s) => false;
        public Circular CircularObject { get; init; } = new();
    }

    public class Circular
    {
        public Circular SelfReference { get; set; }

        public Circular()
        {
            this.SelfReference = this;
        }
    }

    [Test]
    public async Task EventTArgMonitorHandler_ComplicatedEventArgsType_Test()
    {
        // GIVEN
        await using var context = new CanvasFrameTestContext();

        // WHEN
        var eventArgs = new TestEvent();
        await context.CanvasFrame.EventTArgMonitorHandler("OnTest", eventArgs);

        // THEN
        context.JSModule.VerifyInvoke("dispatchComponentActionEvent").Arguments.Is(
            "OnTest",
            """
            {
              "Fragment": "Serialization of type 'RenderFragment' is not supported.",
              "FragmentWithArg": "Serialization of type 'RenderFragment<int>' is not supported.",
              "SystemAction": "Serialization of type 'Action' is not supported.",
              "SystemActionWithArg": "Serialization of type 'Action<int, string>' is not supported.",
              "SystemFunc": "Serialization of type 'Func<bool>' is not supported.",
              "SystemFuncWithArg": "Serialization of type 'Func<int, string, bool>' is not supported.",
              "CircularObject": {
                "SelfReference": "Serialization of cyclic references is not supported."
              }
            }
            """);
    }
}
