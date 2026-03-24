using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;

namespace BlazingStory.Addons.BuiltIns.Internals.Utils;

/// <summary>
/// Creates <see cref="JSModule"/> instances scoped to the BlazingStory.Addons.BuiltIns content root.
/// </summary>
internal static class JSModuleFactory
{
    private const string _DefaultBasePath = "./_content/BlazingStory.Addons.BuiltIns/";

    /// <summary>
    /// Creates a <see cref="JSModule"/> for the specified module path relative to the BuiltIns content root.
    /// </summary>
    /// <param name="jSRuntimeFactory">A factory that returns the <see cref="IJSRuntime"/> instance to use.</param>
    /// <param name="modulePath">The path to the JavaScript module, relative to the BuiltIns content root.</param>
    internal static JSModule Create(Func<IJSRuntime> jSRuntimeFactory, string modulePath)
    {
        return new JSModule(jSRuntimeFactory, _DefaultBasePath + modulePath, VersionInfo.GetEscapedVersionText());
    }
}
