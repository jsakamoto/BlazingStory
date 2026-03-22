namespace BlazingStory.ToolKit.Icons;

/// <summary>
/// Identifies the SVG icon to render with the <see cref="SvgIcon"/> component.
/// </summary>
public enum SvgIconType
{
    /// <summary>Folder icon used in the navigation tree.</summary>
    Folder,
    /// <summary>Component icon used in the navigation tree.</summary>
    Component,
    /// <summary>Document icon used in the navigation tree.</summary>
    Document,
    /// <summary>Hollow bookmark icon.</summary>
    BookmarkHollow,
    /// <summary>Expand-all icon.</summary>
    ExpandAll,
    /// <summary>Collapse-all icon.</summary>
    CollapseAll,
    /// <summary>Gear / settings icon.</summary>
    Gear,
    /// <summary>Find / search icon.</summary>
    Find,

    // ---- icons on the side bar's history ----
    /// <summary>Navigate-back arrow icon.</summary>
    NavigationBack,
    /// <summary>Trash can / delete icon.</summary>
    TrashCan,

    // ---- icons on the canvas page's toolbar ----
    /// <summary>Reload icon for the canvas toolbar.</summary>
    Reload,
    /// <summary>Zoom-in icon for the canvas toolbar.</summary>
    ZoomIn,
    /// <summary>Zoom-out icon for the canvas toolbar.</summary>
    ZoomOut,
    /// <summary>Zoom-reset icon for the canvas toolbar.</summary>
    ZoomReset,
    /// <summary>Background toggle icon for the canvas toolbar.</summary>
    Background,
    /// <summary>Grid overlay icon for the canvas toolbar.</summary>
    Grid,
    /// <summary>Change-size icon for the canvas toolbar.</summary>
    ChangeSize,
    /// <summary>Swap / exchange arrows icon for the canvas toolbar.</summary>
    Swap,
    /// <summary>Measure / ruler icon for the canvas toolbar.</summary>
    Measure,
    /// <summary>Outlines toggle icon for the canvas toolbar.</summary>
    Outlines,

    // ---- icons in the canvas page's Addons > Controls ----
    /// <summary>Reset icon used in the Controls addon.</summary>
    Reset,
    /// <summary>Chevron (down arrow) icon used in the Controls addon.</summary>
    Chevron,
    /// <summary>Exchange icon used in the Controls addon color input.</summary>
    Exchange,

    /// <summary>Close (×) icon.</summary>
    Close,
    /// <summary>Circle-close icon.</summary>
    CircleClose,
    /// <summary>Right side-pane toggle icon.</summary>
    RightSidePane,
    /// <summary>Bottom side-pane toggle icon.</summary>
    BottomSidePane,
    /// <summary>Open-in-new-tab icon.</summary>
    OpenNewTab,
    /// <summary>Hyperlink icon.</summary>
    Link,
    /// <summary>Full-screen icon.</summary>
    FullScreen,
    /// <summary>Checkmark icon.</summary>
    Check,
    /// <summary>Sidebar / hamburger-menu icon.</summary>
    Sidebar,

    // ---- icons on the settings menu ----
    /// <summary>Circle-info icon used in the settings menu.</summary>
    CircleInfo,
    /// <summary>Release-notes icon used in the settings menu.</summary>
    ReleaseNotes,
    /// <summary>Keyboard-shortcuts icon used in the settings menu.</summary>
    KeyboardShortcuts,
}
