using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;
using Moq;

namespace BlazingStory.Test.ToolKit.JSInterop;

public class IJSExtensionTest
{
    [Test]
    public void GetUpdateToken_on_ServerSide_Test()
    {
        var token = Mock.Of<IJSRuntime>().GetUpdateToken("update-token");
        token.Is("?v=update-token");
    }

    [Test]
    public void GetUpdateToken_when_OnLine_Test()
    {
        // Given
        var jsInProcRuntime = new Mock<IJSInProcessRuntime>();
#if NET10_0_OR_GREATER
        jsInProcRuntime
            .Setup(js => js.GetValue<bool>(It.Is<string>(arg => arg == "navigator.onLine")))
            .Returns(true);
#else
        jsInProcRuntime
            .Setup(js => js.Invoke<bool>(
                It.Is<string>(arg => arg == "Toolbelt.Blazor.getProperty"),
                It.Is<string>(arg => arg == "navigator.onLine")))
            .Returns(true);
#endif

        // When
        var token = jsInProcRuntime.Object.GetUpdateToken("update-token-foo");

        // Then
        token.Is("?v=update-token-foo");
    }


    [Test]
    public void GetUpdateToken_when_OffLine_Test()
    {
        // Given
        var jsInProcRuntime = new Mock<IJSInProcessRuntime>();
#if NET10_0_OR_GREATER
        jsInProcRuntime
            .Setup(js => js.GetValue<bool>(It.Is<string>(arg => arg == "navigator.onLine")))
            .Returns(false);
#else
        jsInProcRuntime
            .Setup(js => js.Invoke<bool>(
                It.Is<string>(arg => arg == "Toolbelt.Blazor.getProperty"),
                It.Is<string>(arg => arg == "navigator.onLine")))
            .Returns(false);
#endif

        // When
        var token = jsInProcRuntime.Object.GetUpdateToken("update-token-bar");

        // Then
        token.Is("");
    }
}
