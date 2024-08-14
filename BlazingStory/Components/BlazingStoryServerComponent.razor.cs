using Microsoft.AspNetCore.Components;

namespace BlazingStory.Components;

/// <summary>
/// The BlazingStoryServerComponent class. This class cannot be inherited. Implements the <see
/// cref="Microsoft.AspNetCore.Components.ComponentBase" />. This class is used to determine whether
/// the current request is for the index page or the i frame page. If the request is for the index
/// page, the index page is rendered; otherwise, the i frame page is rendered.
/// </summary>
/// <typeparam name="TIndexPage">
/// The type of the index page.
/// </typeparam>
/// <typeparam name="TIFramePage">
/// The type of the i frame page.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
public partial class BlazingStoryServerComponent<TIndexPage, TIFramePage> : ComponentBase
    where TIndexPage : ComponentBase
    where TIFramePage : ComponentBase
{
}
