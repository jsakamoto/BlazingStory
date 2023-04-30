using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Addons;

internal interface IAddonComponent
{
    AddonType AddonType { get; }

    int ToolbuttonOrder { get; }

    RenderFragment? ToolbarContents { get; }

    IReadOnlyDictionary<string, object?> FrameArguments { get; }
}
