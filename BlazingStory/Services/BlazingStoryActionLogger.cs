using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.JSInterop;

namespace BlazingStory.Services;

/// <summary>
/// Default implementation of <see cref="IBlazingStoryActionLogger"/>.
/// </summary>
public sealed class BlazingStoryActionLogger : IBlazingStoryActionLogger, IAsyncDisposable
{
    private const string _ActionDispatchModulePath = "./_content/BlazingStory.Addons.BuiltIns/Panel/Actions/ActionsPanelPreviewDecorator.razor.js";

    private readonly JSModule _JSModule;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlazingStoryActionLogger"/> class.
    /// </summary>
    /// <param name="jSRuntime">The JavaScript runtime used to dispatch action events.</param>
    public BlazingStoryActionLogger(IJSRuntime jSRuntime)
    {
        this._JSModule = new JSModule(() => jSRuntime, _ActionDispatchModulePath, VersionInfo.GetEscapedVersionText());
    }

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026")]
    public async ValueTask LogAsync(string actionName, object? payload = null)
    {
        if (string.IsNullOrWhiteSpace(actionName))
        {
            return;
        }

        var argsJson = payload is null ? "void" : JsonSerializer.Serialize(payload);
        await this._JSModule.InvokeVoidAsync("dispatchComponentActionEvent", actionName, argsJson);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this._JSModule.DisposeAsync();
    }
}
