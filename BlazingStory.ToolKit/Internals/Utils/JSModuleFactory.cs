using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;

namespace BlazingStory.ToolKit.Internals.Utils;

internal static class JSModuleFactory
{
    private const string _DefaultBasePath = "./_content/BlazingStory.Toolkit/";

    internal static JSModule Create(Func<IJSRuntime> jSRuntimeFactory, string modulePath)
    {
        return new JSModule(jSRuntimeFactory, _DefaultBasePath + modulePath, VersionInfo.GetEscapedVersionText());
    }
}
