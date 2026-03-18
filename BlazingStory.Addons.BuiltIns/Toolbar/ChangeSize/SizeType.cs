namespace BlazingStory.Addons.BuiltIns.Toolbar.ChangeSize;

/// <summary>
/// Identifies the predefined viewport size preset for the story canvas.
/// </summary>
internal enum SizeType
{
    /// <summary>No size constraint; the canvas uses its natural size.</summary>
    None,
    /// <summary>Small mobile preset (320 x 568).</summary>
    SmallMobile,
    /// <summary>Large mobile preset (414 x 896).</summary>
    LargeMobile,
    /// <summary>Tablet preset (834 x 1112).</summary>
    Tablet
}
