using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

/// <summary>
/// Stores registered custom page containers.
/// </summary>
public class CustomPageStore
{
    private readonly List<CustomPageContainer> _CustomPageContainers = new();

    /// <summary>
    /// Gets registered custom page containers.
    /// </summary>
    internal IEnumerable<CustomPageContainer> CustomPageContainers => this._CustomPageContainers;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPageStore"/> class.
    /// </summary>
    internal CustomPageStore()
    {
    }

    /// <summary>
    /// Registers or replaces a custom page container by title.
    /// </summary>
    /// <param name="customContainer">The custom page container to register.</param>
    internal void RegisterCustomPageContainer(CustomPageContainer customContainer)
    {
        var index = this._CustomPageContainers.FindIndex(container => container.Title == customContainer.Title);
        if (index != -1)
        {
            if (Object.ReferenceEquals(this._CustomPageContainers[index], customContainer) == false)
            {
                this._CustomPageContainers[index] = customContainer;
            }
        }
        else
        {
            this._CustomPageContainers.Add(customContainer);
        }
    }

    /// <summary>
    /// Try to find a component by navigation path, such as "examples-ui-button".
    /// </summary>
    /// <param name="navigationPath">The navigation path.</param>
    /// <param name="customPageContainer">When this method returns, contains the matching custom page container if found.</param>
    /// <returns><see langword="true"/> when found; otherwise, <see langword="false"/>.</returns>
    internal bool TryGetCustomPageContainerByPath(string navigationPath, [NotNullWhen(true)] out CustomPageContainer? customPageContainer)
    {
        customPageContainer = this._CustomPageContainers.FirstOrDefault(c => c.NavigationPath == navigationPath);
        return customPageContainer != null;
    }
}