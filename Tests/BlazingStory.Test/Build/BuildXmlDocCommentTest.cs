#if NET9_0
using System.Xml.Linq;
using BlazingStory.Test._Fixtures;
using Toolbelt.Diagnostics;

namespace BlazingStory.Test.Build;

internal class BuildXmlDocCommentTest
{
    [Parallelizable(ParallelScope.Self)]
    [TestCase("net8.0", "8.0.0", false)]
    [TestCase("net9.0", "9.0.0", false)]
    public async Task BlazorWasm_DotNetRun_Test(string targetFramework, string SDKVersion, bool allowPrerelease)
    {
        // Given
        var testFixtureSpace = new TestFixtureSpace();
        testFixtureSpace.CreateGlobalJson(SDKVersion, "latestMinor", allowPrerelease);
        var emptyBlazorWasmApp1Dir = testFixtureSpace.GetTestAppProjDir("EmptyBlazorWasmApp1");
        CsprojTools.Rewrite(emptyBlazorWasmApp1Dir, targetFramework, testFixtureSpace.BlazingStoryTargetsPath);

        var dotnetVersion = await XProcess.Start("dotnet", "--version", emptyBlazorWasmApp1Dir, TestHelper.XProcessOptions).WaitForExitAsync();
        dotnetVersion.ExitCode.Is(0, message: dotnetVersion.Output);
        dotnetVersion.Output.Is(ver => ver.StartsWith(SDKVersion.Substring(0, 4)), message: dotnetVersion.Output);

        // When
        var listenUrl = $"http://localhost:{TestHelper.GetAvailableTCPv4Port()}";
        using var dotnetRun = XProcess.Start("dotnet", $"run --urls {listenUrl}", emptyBlazorWasmApp1Dir, TestHelper.XProcessOptions);
        var success = await dotnetRun.WaitForOutputAsync(output => output.TrimStart().StartsWith("Now listening on: http"), millsecondsTimeout: 30000);
        success.IsTrue(message: dotnetRun.Output);

        // Then
        var xdocCommentForApp = XDocument.Load(uri: $"{listenUrl}/_framework/EmptyBlazorWasmApp1.xml");
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorWasmApp1");

        var xdocCommentForLib = XDocument.Load(uri: $"{listenUrl}/_framework/RazorClassLib1.xml");
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: $"{listenUrl}/_framework/Microsoft.AspNetCore.Components.Web.xml");
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");

        dotnetRun.Process.Kill(entireProcessTree: true);
    }

    [Parallelizable(ParallelScope.Self)]
    [TestCase("net8.0", "8.0.0", false)]
    [TestCase("net9.0", "9.0.0", false)]
    public async Task BlazorWasm_Publish_Test(string targetFramework, string SDKVersion, bool allowPrerelease)
    {
        // Given
        var testFixtureSpace = new TestFixtureSpace();
        testFixtureSpace.CreateGlobalJson(SDKVersion, "latestMinor", allowPrerelease);
        var emptyBlazorWasmApp1Dir = testFixtureSpace.GetTestAppProjDir("EmptyBlazorWasmApp1");
        CsprojTools.Rewrite(emptyBlazorWasmApp1Dir, targetFramework, testFixtureSpace.BlazingStoryTargetsPath);

        var dotnetVersion = await XProcess.Start("dotnet", "--version", emptyBlazorWasmApp1Dir, TestHelper.XProcessOptions).WaitForExitAsync();
        dotnetVersion.ExitCode.Is(0, message: dotnetVersion.Output);
        dotnetVersion.Output.Is(ver => ver.StartsWith(SDKVersion.Substring(0, 4)), message: dotnetVersion.Output);

        // When
        using var dotnetPublish = XProcess.Start("dotnet", "publish -c Release -p BlazorEnableCompression=false", emptyBlazorWasmApp1Dir, TestHelper.XProcessOptions);
        await dotnetPublish.WaitForExitAsync();
        dotnetPublish.ExitCode.Is(0, message: dotnetPublish.Output);

        // Then
        var frameworkDir = Path.Combine(emptyBlazorWasmApp1Dir, "bin", "Release", targetFramework, "publish", "wwwroot", "_framework");
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(frameworkDir, "EmptyBlazorWasmApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorWasmApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(frameworkDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(frameworkDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");
    }

    [Parallelizable(ParallelScope.Self)]
    [TestCase("net8.0", "8.0.0", false)]
    [TestCase("net9.0", "9.0.0", false)]
    public async Task BlazorServer_DotNetRun_Test(string targetFramework, string SDKVersion, bool allowPrerelease)
    {
        // Given
        var testFixtureSpace = new TestFixtureSpace();
        testFixtureSpace.CreateGlobalJson(SDKVersion, "latestMinor", allowPrerelease);
        var emptyBlazorServerApp1Dir = testFixtureSpace.GetTestAppProjDir("EmptyBlazorServerApp1");
        CsprojTools.Rewrite(emptyBlazorServerApp1Dir, targetFramework, testFixtureSpace.BlazingStoryTargetsPath);

        var dotnetVersion = await XProcess.Start("dotnet", "--version", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions).WaitForExitAsync();
        dotnetVersion.ExitCode.Is(0, message: dotnetVersion.Output);
        dotnetVersion.Output.Is(ver => ver.StartsWith(SDKVersion.Substring(0, 4)), message: dotnetVersion.Output);

        // When
        var listenUrl = $"http://localhost:{TestHelper.GetAvailableTCPv4Port()}";
        using var dotnetRun = XProcess.Start("dotnet", $"run --urls {listenUrl}", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions);
        var success = await dotnetRun.WaitForOutputAsync(output => output.TrimStart().StartsWith("Now listening on: http"), millsecondsTimeout: 30000);
        success.IsTrue(message: dotnetRun.Output);

        // Then
        var outputDir = Path.Combine(emptyBlazorServerApp1Dir, "bin", "Debug", targetFramework);
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(outputDir, "EmptyBlazorServerApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorServerApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(outputDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(outputDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");

        dotnetRun.Process.Kill(entireProcessTree: true);
    }

    [Parallelizable(ParallelScope.Self)]
    [TestCase("net8.0", "8.0.0", false)]
    [TestCase("net9.0", "9.0.0", false)]
    public async Task BlazorServer_Publish_Test(string targetFramework, string SDKVersion, bool allowPrerelease)
    {
        // Given
        var testFixtureSpace = new TestFixtureSpace();
        testFixtureSpace.CreateGlobalJson(SDKVersion, "latestMinor", allowPrerelease);
        var emptyBlazorServerApp1Dir = testFixtureSpace.GetTestAppProjDir("EmptyBlazorServerApp1");
        CsprojTools.Rewrite(emptyBlazorServerApp1Dir, targetFramework, testFixtureSpace.BlazingStoryTargetsPath);

        var dotnetVersion = await XProcess.Start("dotnet", "--version", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions).WaitForExitAsync();
        dotnetVersion.ExitCode.Is(0, message: dotnetVersion.Output);
        dotnetVersion.Output.Is(ver => ver.StartsWith(SDKVersion.Substring(0, 4)), message: dotnetVersion.Output);

        // When
        using var dotnetPublish = XProcess.Start("dotnet", "publish -c Release", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions);
        await dotnetPublish.WaitForExitAsync();
        dotnetPublish.ExitCode.Is(0, message: dotnetPublish.Output);

        // Then
        var publishDir = Path.Combine(emptyBlazorServerApp1Dir, "bin", "Release", targetFramework, "publish");
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(publishDir, "EmptyBlazorServerApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorServerApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(publishDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(publishDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");
    }
}
#endif