#if NET10_0
using System.Xml.Linq;
using BlazingStory.Test._Fixtures;
using Toolbelt.Diagnostics;

namespace BlazingStory.Test.Build;

[Parallelizable(ParallelScope.Children)]
internal class BuildXmlDocCommentTest
{
    private static async ValueTask<(TestFixtureSpace, string)> CreateTestFixtureSpace(int targetFramework, int sdkVersion, bool allowPrerelease, string projectName)
    {
        var testFixtureSpace = new TestFixtureSpace();

        // Rewrite the test app's .csproj to set TargetFramework and import BlazingStory MSBuild targets.
        var appProjDir = testFixtureSpace.GetTestAppProjDir(projectName);
        CsprojTools.Rewrite(appProjDir, $"net{targetFramework}.0", testFixtureSpace.BlazingStoryTargetsPath);

        // Create global.json to set SDK version, and verify the used SDK version number.
        testFixtureSpace.CreateGlobalJson($"{sdkVersion}.0.0", "latestMinor", allowPrerelease);
        var dotnetVersion = await XProcess.Start("dotnet", "--version", appProjDir, TestHelper.XProcessOptions).WaitForExitAsync();
        var foundOutput = await dotnetVersion.WaitForOutputAsync(output => output.StartsWith($"{sdkVersion}.0"), millsecondsTimeout: 3000);
        foundOutput.IsTrue(message: $"dotnetVersion.Output is: \"{dotnetVersion.Output}\"");
        dotnetVersion.ExitCode.Is(0, message: dotnetVersion.Output);
        return (testFixtureSpace, appProjDir);
    }

    [TestCase(8, 8, false)]
    [TestCase(8, 9, false)]
    [TestCase(9, 9, false)]
    public async Task BlazorWasm_DotNetRun_Test(int targetFramework, int sdkVersion, bool allowPrerelease)
    {
        // Given
        var (testFixtureSpace, emptyBlazorWasmApp1Dir) = await CreateTestFixtureSpace(targetFramework, sdkVersion, allowPrerelease, "EmptyBlazorWasmApp1");

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

    [TestCase(8, 8, false)]
    [TestCase(8, 9, false)]
    [TestCase(9, 9, false)]
    public async Task BlazorWasm_Publish_Test(int targetFramework, int sdkVersion, bool allowPrerelease)
    {
        // Given
        var (testFixtureSpace, emptyBlazorWasmApp1Dir) = await CreateTestFixtureSpace(targetFramework, sdkVersion, allowPrerelease, "EmptyBlazorWasmApp1");

        // When
        using var dotnetPublish = XProcess.Start("dotnet", "publish -c Release -p BlazorEnableCompression=false", emptyBlazorWasmApp1Dir, TestHelper.XProcessOptions);
        await dotnetPublish.WaitForExitAsync();
        dotnetPublish.ExitCode.Is(0, message: dotnetPublish.Output);

        // Then
        var frameworkDir = Path.Combine(emptyBlazorWasmApp1Dir, "bin", "Release", $"net{targetFramework}.0", "publish", "wwwroot", "_framework");
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(frameworkDir, "EmptyBlazorWasmApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorWasmApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(frameworkDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(frameworkDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");
    }

    [TestCase(8, 8, false)]
    [TestCase(8, 9, false)]
    [TestCase(9, 9, false)]
    public async Task BlazorServer_DotNetRun_Test(int targetFramework, int sdkVersion, bool allowPrerelease)
    {
        // Given
        var (testFixtureSpace, emptyBlazorServerApp1Dir) = await CreateTestFixtureSpace(targetFramework, sdkVersion, allowPrerelease, "EmptyBlazorServerApp1");

        // When
        var listenUrl = $"http://localhost:{TestHelper.GetAvailableTCPv4Port()}";
        using var dotnetRun = XProcess.Start("dotnet", $"run --urls {listenUrl}", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions);
        var success = await dotnetRun.WaitForOutputAsync(output => output.TrimStart().StartsWith("Now listening on: http"), millsecondsTimeout: 30000);
        success.IsTrue(message: dotnetRun.Output);

        // Then
        var outputDir = Path.Combine(emptyBlazorServerApp1Dir, "bin", "Debug", $"net{targetFramework}.0");
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(outputDir, "EmptyBlazorServerApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorServerApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(outputDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(outputDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");

        dotnetRun.Process.Kill(entireProcessTree: true);
    }

    [TestCase(8, 8, false)]
    [TestCase(8, 9, false)]
    [TestCase(9, 9, false)]
    public async Task BlazorServer_Publish_Test(int targetFramework, int sdkVersion, bool allowPrerelease)
    {
        // Given
        var (testFixtureSpace, emptyBlazorServerApp1Dir) = await CreateTestFixtureSpace(targetFramework, sdkVersion, allowPrerelease, "EmptyBlazorServerApp1");

        // When
        using var dotnetPublish = XProcess.Start("dotnet", "publish -c Release", emptyBlazorServerApp1Dir, TestHelper.XProcessOptions);
        await dotnetPublish.WaitForExitAsync();
        dotnetPublish.ExitCode.Is(0, message: dotnetPublish.Output);

        // Then
        var publishDir = Path.Combine(emptyBlazorServerApp1Dir, "bin", "Release", $"net{targetFramework}.0", "publish");
        var xdocCommentForApp = XDocument.Load(uri: Path.Combine(publishDir, "EmptyBlazorServerApp1.xml"));
        (xdocCommentForApp.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("EmptyBlazorServerApp1");

        var xdocCommentForLib = XDocument.Load(uri: Path.Combine(publishDir, "RazorClassLib1.xml"));
        (xdocCommentForLib.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("RazorClassLib1");

        var xdocCommentForFramework = XDocument.Load(uri: Path.Combine(publishDir, "Microsoft.AspNetCore.Components.Web.xml"));
        (xdocCommentForFramework.Element("doc")?.Element("assembly")?.Element("name")?.Value).Is("Microsoft.AspNetCore.Components.Web");
    }
}
#endif