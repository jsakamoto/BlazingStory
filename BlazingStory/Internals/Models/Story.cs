using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal record Story(
    string Name,
    StoryContext Context,
    RenderFragment<StoryContext> RenderFragment);