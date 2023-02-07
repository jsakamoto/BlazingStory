using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Addons;

public class AddonsStore
{
    private readonly List<RenderFragment> _CanvasToolbarRenderer = new();

    internal IEnumerable<RenderFragment> CanvasToolbarRenderer => this._CanvasToolbarRenderer;

    internal event EventHandler<CanvasFrameArgumentsEventArgs>? OnSetCanvasFrameArguments;

    private readonly Dictionary<string, object?> _CanvasFrameArguments = new();

    internal IReadOnlyDictionary<string, object?> CanvasFrameArguments => this._CanvasFrameArguments;

    internal void RegisterCanvasToolbarRenderer(RenderFragment renderer)
    {
        this._CanvasToolbarRenderer.Add(renderer);
    }

    internal void SetCanvasFrameArguments(params (string Key, object? Value)[] args)
    {
        foreach (var (Key, Value) in args)
        {
            if (Value == null) this._CanvasFrameArguments.Remove(Key);
            else this._CanvasFrameArguments[Key] = Value;
        }

        this.OnSetCanvasFrameArguments?.Invoke(this, new(args));
    }
}
