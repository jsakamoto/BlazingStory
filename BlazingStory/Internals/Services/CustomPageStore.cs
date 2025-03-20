using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

public class CustomPageStore
{
    private readonly List<CustomPageContainer> _CustomPageContainers = new();

    internal IEnumerable<CustomPageContainer> CustomPageContainers => this._CustomPageContainers;

    internal CustomPageStore()
    {
    }

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
    internal bool TryGetCustomPageContainerByPath(string navigationPath, [NotNullWhen(true)] out CustomPageContainer? customPageContainer)
    {
        customPageContainer = this._CustomPageContainers.FirstOrDefault(c => c.NavigationPath == navigationPath);
        return customPageContainer != null;
    }
}