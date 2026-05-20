namespace BlazingStory.Addons.BuiltIns.Panel.Accessibility;

/// <summary>
/// Registers the Accessibility panel and its preview decorator with the addon builder.
/// </summary>
internal class AccessibilityPanelAddon : IAddon
{
    internal static class Message
    {
        public const string AskReady = "Addons.BuiltIns.AccessibilityPanel.AskReady";
        public const string Ready = "Addons.BuiltIns.AccessibilityPanel.Ready";
        public const string RunAxe = "Addons.BuiltIns.AccessibilityPanel.RunAxe";
        public const string AxeResult = "Addons.BuiltIns.AccessibilityPanel.AxeResult:";
    }

    /// <summary>
    /// Initializes the addon by registering the Accessibility panel and its preview decorator with the provided addon builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register panels and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<AccessibilityPanel>(order: 300, viewMode => viewMode is ViewMode.Story);
        builder.AddPreviewDecorator<AccessibilityPanelPreviewDecorator>();
    }
}
