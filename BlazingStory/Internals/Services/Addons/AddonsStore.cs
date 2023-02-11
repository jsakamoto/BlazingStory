using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Addons;

public class AddonsStore
{
    private class CanvasToolbarRenderer { public int Order; public required RenderFragment Fragment; }

    private IEnumerable<CanvasToolbarRenderer> _CanvasToolbarRenderers = Enumerable.Empty<CanvasToolbarRenderer>();

    internal IEnumerable<RenderFragment> CanvasToolbarRenderers => this._CanvasToolbarRenderers.Select(r => r.Fragment);

    internal event EventHandler<CanvasFrameArgumentsEventArgs>? OnSetCanvasFrameArguments;

    private readonly Dictionary<string, object?> _CanvasFrameArguments = new();

    internal IReadOnlyDictionary<string, object?> CanvasFrameArguments => this._CanvasFrameArguments;

    internal void RegisterCanvasToolbarRenderer(int toolButtonOrder, RenderFragment renderer)
    {
        this._CanvasToolbarRenderers = this._CanvasToolbarRenderers
            .Append(new() { Order = toolButtonOrder, Fragment = renderer })
            .OrderBy(r => r.Order)
            .ToArray();

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
