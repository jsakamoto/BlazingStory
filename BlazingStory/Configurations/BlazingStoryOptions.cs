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
}
