using System.Text.RegularExpressions;
using System.Web;
using BlazingStory.Internals.Utils;
using Microsoft.JSInterop;
using Moq;

namespace BlazingStory.Test.Internals.Utils;

internal class UriParameterKitTest
{
    [Test]
    public void EncodeKeyValues_with_Null_Test()
    {
        UriParameterKit.EncodeKeyValues(null).Is("");
    }

    [Test]
    public void EncodeKeyValues_Test()
    {
        UriParameterKit.EncodeKeyValues(new Dictionary<string, object?>
        {
            ["ABC"] = "DEF",
            ["GH:I"] = "JKL",
            ["MNO"] = "P:QR",
            ["S;TU"] = "VW;X",
        }).Is("ABC:DEF;GH%3AI:JKL;MNO:P%3AQR;S%3BTU:VW%3BX");
    }

    [Test]
    public void DecodeKeyValues_with_Empty_Test()
    {
        UriParameterKit.DecodeKeyValues("").Count.Is(0);
    }

    [Test]
    public void DecodeKeyValues_with_Null_Test()
    {
        UriParameterKit.DecodeKeyValues(null).Count.Is(0);
    }

    [Test]
    public void DecodeKeyValues_Test()
    {
        var keyValues = UriParameterKit.DecodeKeyValues("ABC:DEF;GH%3AI:JKL;MNO:P%3AQR;S%3BTU:VW%3BX");
        keyValues["ABC"].Is("DEF");
        keyValues["GH:I"].Is("JKL");
        keyValues["MNO"].Is("P:QR");
        keyValues["S;TU"].Is("VW;X");
    }

    [Test]
    public void GetUri_Test()
    {
        var uri = UriParameterKit.GetUri("./iframe.html", new Dictionary<string, object?>
        {
            ["viewMode"] = "story",
            ["args"] = UriParameterKit.EncodeKeyValues(new Dictionary<string, object?>
            {
                ["Flag"] = true,
                ["Text"] = "Date: 2023/02/01",
                ["Width"] = 640
            })
        });
        uri.Is("./iframe.html?viewMode=story&args=Flag:True;Text:Date%253A%25202023%252F02%252F01;Width:640");

        var queryStrings = HttpUtility.ParseQueryString(new Uri("https://example.com/" + uri).Query);
        queryStrings["viewMode"].Is("story");
        var args = UriParameterKit.DecodeKeyValues(queryStrings["args"]);
        args["Flag"].Is("True");
        args["Text"].Is("Date: 2023/02/01");
        args["Width"].Is("640");
    }

    [Test]
    public void GetUpdateToken_on_ServerSide_Test()
    {
        var token = UriParameterKit.GetUpdateToken(Mock.Of<IJSRuntime>());
        token.Is("");
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
        var token = UriParameterKit.GetUpdateToken(jsInProcRuntime.Object);

        // Then
        Regex.IsMatch(token, @"^\?v=.+$").IsTrue();
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
        var token = UriParameterKit.GetUpdateToken(jsInProcRuntime.Object);

        // Then
        token.Is("");
    }
}
