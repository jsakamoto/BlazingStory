using BlazingStory.Services;
using Microsoft.JSInterop;
using NSubstitute;
using System.Linq;

namespace BlazingStory.Test.Services;

internal class BlazingStoryActionLoggerTest
{
    [Test]
    public async Task LogAsync_with_empty_action_name_does_nothing_Test()
    {
        // Given
        var jsRuntime = Substitute.For<IJSRuntime>();
        var logger = new BlazingStoryActionLogger(jsRuntime);

        // When
        await logger.LogAsync("");

        // Then
        jsRuntime.ReceivedCalls().Any().IsFalse();
    }

    [Test]
    public async Task LogAsync_with_null_payload_sends_void_Test()
    {
        // Given
        var module = Substitute.For<IJSObjectReference>();
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(new ValueTask<IJSObjectReference>(module));

        var logger = new BlazingStoryActionLogger(jsRuntime);

        // When
        await logger.LogAsync("Clicked", null);

        // Then
        var dispatchCall = module.ReceivedCalls().Single(c =>
        {
            var args = c.GetArguments();
            return args.Length > 0 && (string?)args[0] == "dispatchComponentActionEvent";
        });

        var payloadArgs = dispatchCall.GetArguments()[1] as object?[];
        payloadArgs.IsNotNull();
        payloadArgs!.Length.Is(2);
        ((string?)payloadArgs[0]).Is("Clicked");
        ((string?)payloadArgs[1]).Is("void");
    }

    [Test]
    public async Task LogAsync_with_object_payload_serializes_and_dispatches_Test()
    {
        // Given
        var module = Substitute.For<IJSObjectReference>();
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(new ValueTask<IJSObjectReference>(module));

        var logger = new BlazingStoryActionLogger(jsRuntime);

        // When
        await logger.LogAsync("ValueChanged", new Dictionary<string, object?> { ["count"] = 3 });

        // Then
        var dispatchCall = module.ReceivedCalls().Single(c =>
        {
            var args = c.GetArguments();
            return args.Length > 0 && (string?)args[0] == "dispatchComponentActionEvent";
        });

        var payloadArgs = dispatchCall.GetArguments()[1] as object?[];
        payloadArgs.IsNotNull();
        payloadArgs!.Length.Is(2);
        ((string?)payloadArgs[0]).Is("ValueChanged");

        var serializedPayload = payloadArgs[1] as string;
        serializedPayload.IsNotNull();
        serializedPayload!.Contains("\"count\":3").IsTrue();
    }

    [Test]
    public async Task LogAsync_uses_expected_dispatch_identifier_Test()
    {
        // Given
        var module = Substitute.For<IJSObjectReference>();
        var jsRuntime = Substitute.For<IJSRuntime>();
        jsRuntime
            .InvokeAsync<IJSObjectReference>("import", Arg.Any<object?[]?>())
            .Returns(new ValueTask<IJSObjectReference>(module));

        var logger = new BlazingStoryActionLogger(jsRuntime);

        // When
        await logger.LogAsync("Saved", new { Success = true });

        // Then
        module.ReceivedCalls().Any(c =>
        {
            var args = c.GetArguments();
            return args.Length > 0 && (string?)args[0] == "dispatchComponentActionEvent";
        }).IsTrue();
    }
}
