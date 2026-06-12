using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.Internals;

/// <summary>
/// Manages the registration and retrieval of addon toolbar content, panels, and preview decorators.
/// </summary>
internal class AddonManager : IAddonBuilder, IDisposable
{
    private readonly List<ToolbarContentDescriptor> _toolbarContents = [];

    private readonly List<PanelDescriptor> _panels = [];

    private readonly List<PreviewDecoratorDescriptor> _previewDecorators = [];

    internal event EventHandler? GlobalArgumentsChanged;

    /// <summary>
    /// Initializes all addons registered in the given <see cref="AddonStore"/>.
    /// </summary>
    /// <param name="addonStore">The store containing the addons to initialize.</param>
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

    void IAddonBuilder.AddPanel<[DynamicallyAccessedMembers(All)] TPanelComponent>(int order, Func<ViewMode, bool> match)
    {
        var panelDescriptor = new PanelDescriptor(order, match, typeof(TPanelComponent));
        //panelDescriptor.Globals.ArgumentsChanged += this.OnGlobalArgumentsChanged;
        this._panels.Add(panelDescriptor);
    }

    void IAddonBuilder.AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>()
    {
        var previewDecoratorDescriptor = new PreviewDecoratorDescriptor(typeof(TPreviewDecoratorComponent));
        this._previewDecorators.Add(previewDecoratorDescriptor);
    }

    /// <summary>
    /// Returns the toolbar content descriptors that match the given view mode, ordered by <c>Order</c>.
    /// </summary>
    /// <param name="viewMode">The current view mode to filter by.</param>
    internal IEnumerable<ToolbarContentDescriptor> GetToolbarContents(ViewMode viewMode)
    {
        return this._toolbarContents.Where(x => x.Match(viewMode)).OrderBy(x => x.Order);
    }

    /// <summary>
    /// Returns the panel descriptors that match the given view mode, ordered by <c>Order</c>.
    /// </summary>
    /// <param name="viewMode">The current view mode to filter by.</param>
    internal IEnumerable<PanelDescriptor> GetPanels(ViewMode viewMode)
    {
        return this._panels.Where(x => x.Match(viewMode)).OrderBy(x => x.Order);
    }

    /// <summary>
    /// Returns all registered preview decorator descriptors.
    /// </summary>
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
