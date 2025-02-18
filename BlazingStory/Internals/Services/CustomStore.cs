using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

public class CustomStore
{
    private readonly List<CustomContainer> _CustomContainers = new();

    internal IEnumerable<CustomContainer> CustomContainers => this._CustomContainers;

    internal CustomStore()
    {
    }

    internal void RegisterCustomContainer(CustomContainer customContainer)
    {
        var index = this._CustomContainers.FindIndex(container => container.Title == customContainer.Title);
        if (index != -1)
        {
            if (Object.ReferenceEquals(this._CustomContainers[index], customContainer) == false)
            {
                this._CustomContainers[index] = customContainer;
            }
        }
        else
        {
            this._CustomContainers.Add(customContainer);
        }
    }

    /// <summary>
    /// Try to find a component by navigationn path, such as "examples-ui-button".
    /// </summary>
    internal bool TryGetComponentByPath(string navigationPath, [NotNullWhen(true)] out CustomContainer? component)
    {
        component = this._CustomContainers.FirstOrDefault(c => c.NavigationPath == navigationPath);
        return component != null;
    }
}