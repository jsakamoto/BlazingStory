namespace BlazingStory.Addons.Internals;

internal class AddonManager : IAddonBuilder, IDisposable
{
    private readonly List<ToolbarButtonDescriptor> _toolbarButtons = [];

    private readonly List<PreviewDecoratorDescriptor> _previewDecorators = [];

    internal event EventHandler? GlobalArgumentsChanged;

    internal void Initialize(AddonStore addonStore)
    {
        foreach (var addon in addonStore.GetAddons())
        {
            addon.Initialize(this);
        }
    }

    void IAddonBuilder.AddToolbarButton<TToolbarButtonComponent>(int order, Func<ViewMode, bool> match)
    {
        var toolbarButtonDescriptor = new ToolbarButtonDescriptor(order, match, typeof(TToolbarButtonComponent));
        toolbarButtonDescriptor.Globals.ArgumentsChanged += this.OnGlobalArgumentsChanged;
        this._toolbarButtons.Add(toolbarButtonDescriptor);
    }

    void IAddonBuilder.AddPreviewDecorator<TPreviewDecoratorComponent>()
    {
        var previewDecoratorDescriptor = new PreviewDecoratorDescriptor(typeof(TPreviewDecoratorComponent));
        this._previewDecorators.Add(previewDecoratorDescriptor);
    }

    internal IEnumerable<ToolbarButtonDescriptor> GetToolbarButtons(ViewMode viewMode)
    {
        return this._toolbarButtons.Where(x => x.Match(viewMode)).OrderBy(x => x.Order);
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
        this._toolbarButtons.ForEach(x => x.Globals.ArgumentsChanged -= this.OnGlobalArgumentsChanged);
    }
}
