using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

internal class AddonManager : IAddonBuilder, IDisposable
{
    private readonly List<ToolbarContentDescriptor> _toolbarContents = [];

    private readonly List<PreviewDecoratorDescriptor> _previewDecorators = [];

    internal event EventHandler? GlobalArgumentsChanged;

    internal void Initialize(AddonStore addonStore)
    {
        foreach (var addon in addonStore.GetAddons())
        {
            addon.Initialize(this);
        }
    }

    void IAddonBuilder.AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match)
    {
        var toolbarContentDescriptor = new ToolbarContentDescriptor(order, match, typeof(TToolbarContentComponent));
        toolbarContentDescriptor.Globals.ArgumentsChanged += this.OnGlobalArgumentsChanged;
        this._toolbarContents.Add(toolbarContentDescriptor);
    }

    void IAddonBuilder.AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>()
    {
        var previewDecoratorDescriptor = new PreviewDecoratorDescriptor(typeof(TPreviewDecoratorComponent));
        this._previewDecorators.Add(previewDecoratorDescriptor);
    }

    internal IEnumerable<ToolbarContentDescriptor> GetToolbarContents(ViewMode viewMode)
    {
        return this._toolbarContents.Where(x => x.Match(viewMode)).OrderBy(x => x.Order);
    }

    internal IEnumerable<PreviewDecoratorDescriptor> GetPreviewDecorators()
    {
        return this._previewDecorators;
    }

    private void OnGlobalArgumentsChanged(object? sender, EventArgs args)
    {
        this.GlobalArgumentsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        this._toolbarContents.ForEach(x => x.Globals.ArgumentsChanged -= this.OnGlobalArgumentsChanged);
    }
}
