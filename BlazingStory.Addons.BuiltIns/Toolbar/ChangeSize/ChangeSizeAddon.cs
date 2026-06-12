namespace BlazingStory.Addons.BuiltIns.Toolbar.ChangeSize;

/// <summary>
/// Registers the Change Size toolbar content with the addon builder.
/// </summary>
internal class ChangeSizeAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="ChangeSizeToolbarContent"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<ChangeSizeToolbarContent>(order: 500, viewMode => viewMode is ViewMode.Story);
    }
}
