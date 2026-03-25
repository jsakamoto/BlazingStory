using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;
using NSubstitute;

namespace BlazingStory.Test.ToolKit.JSInterop;

public class IJSExtensionTest
{
    [Test]
    public void GetUpdateToken_on_ServerSide_Test()
    {
        var token = Substitute.For<IJSRuntime>().GetUpdateToken("update-token");
        token.Is("?v=update-token");
    }

    [Test]
    public void GetUpdateToken_when_OnLine_Test()
    {
        // Given
        var jsInProcRuntime = Substitute.For<IJSInProcessRuntime>();
#if NET10_0_OR_GREATER
        jsInProcRuntime
            .GetValue<bool>("navigator.onLine")
            .Returns(true);
#else
        jsInProcRuntime
            .Invoke<bool>(
                "Toolbelt.Blazor.getProperty",
                "navigator.onLine")
            .Returns(true);
#endif

        // When
        var token = jsInProcRuntime.GetUpdateToken("update-token-foo");

        // Then
        token.Is("?v=update-token-foo");
    }


    [Test]
    public void GetUpdateToken_when_OffLine_Test()
    {
        // Given
        var jsInProcRuntime = Substitute.For<IJSInProcessRuntime>();
#if NET10_0_OR_GREATER
        jsInProcRuntime
            .GetValue<bool>("navigator.onLine")
            .Returns(false);
#else
        jsInProcRuntime
            .Invoke<bool>(
                "Toolbelt.Blazor.getProperty",
                "navigator.onLine")
            .Returns(false);
#endif

        // When
        var token = jsInProcRuntime.GetUpdateToken("update-token-bar");

        // Then
        token.Is("");
    }
}
