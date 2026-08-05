using BlazingStory.Types;

namespace BlazingStory.Configurations;

/// <summary>
/// The configuration options for BlazingStory.
/// </summary>
public class BlazingStoryOptions
{
    /// <summary>
    /// [Preview feature] Gets or sets whether to enable hot reloading. (default: false)
    /// </summary>
    public bool EnableHotReloading { get; set; }

    /// <summary>
    /// Specifies a custom order for items in the sidebar navigation tree.
    /// </summary>
    /// <remarks>
    /// Provide an ordered sequence of <see cref="NavigationTreeOrderEntry"/> values (typically built with <c>NavigationTreeOrderBuilder.N[ ... ]</c>) to declare the preferred order at each hierarchy level.<br/>
    /// Rules:<br/>
    /// - Docs and Story items under a component keep their original order and are always listed first.<br/>
    /// - Items explicitly listed are placed in the given order. If an item is immediately followed by a <see cref="NavigationTreeOrderEntry.NodeType.SubItems"/> group, that group is applied recursively to that item's children.<br/>
    /// - Remaining items not covered by the custom order are appended in the default order (alphabetical; components before custom pages).<br/>
    /// <br/>
    /// <b>Example:</b><br/><code>
    /// @using static BlazingStory.Types.NavigationTreeOrderBuilder
    /// 
    /// &lt;BlazingStoryApp
    ///     NavigationTreeOrder="@N["Welcome", "Components", N["Layouts", N["Header", "Footer"], "Button"], "Templates"]" /&gt;
    /// </code>
    /// </remarks>
    /// <example>
    /// @using static BlazingStory.Types.NavigationTreeOrderBuilder
    ///
    /// &lt;BlazingStoryApp
    ///     NavigationTreeOrder="@N["Welcome", "Components", N["Layouts", N["Header", "Footer"], "Button"], "Templates"]" /&gt;
    /// </example>
    public IList<NavigationTreeOrderEntry>? NavigationTreeOrder { get; set; }

    /// <summary>
    /// Gets or sets the default table of contents (TOC) placement for markdown-based custom pages. (default: <see cref="TableOfContentsPlacement.None"/>)
    /// </summary>
    /// <remarks>
    /// If a custom page sets <see cref="CustomPageAttribute.TableOfContentsPlacement"/>, that per-page value takes precedence over this global default.
    /// When set to <see langword="null"/>, the built-in default value is used.
    /// </remarks>
    public TableOfContentsPlacement? CustomPageTableOfContentsPlacement { get; set; } = TableOfContentsPlacement.None;

    /// <summary>
    /// Gets or sets the default minimum heading level included in TOC for markdown-based custom pages. (default: 1)
    /// </summary>
    /// <remarks>
    /// Allowed values are 1 through 6. If a custom page sets <see cref="CustomPageAttribute.TableOfContentsMinHeadingLevel"/>, that per-page value takes precedence over this global default.
    /// When set to <see langword="null"/>, the built-in default value is used.
    /// </remarks>
    public int? CustomPageTableOfContentsMinHeadingLevel { get; set; } = 1;

    /// <summary>
    /// Gets or sets the default maximum heading level included in TOC for markdown-based custom pages. (default: 4)
    /// </summary>
    /// <remarks>
    /// Allowed values are 1 through 6. If a custom page sets <see cref="CustomPageAttribute.TableOfContentsMaxHeadingLevel"/>, that per-page value takes precedence over this global default.
    /// When set to <see langword="null"/>, the built-in default value is used.
    /// </remarks>
    public int? CustomPageTableOfContentsMaxHeadingLevel { get; set; } = 4;
}
