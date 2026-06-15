using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;

/// <summary>
/// Base class for all parameter controller components, providing a cascading parameter for the controller context and an input notification helper.
/// </summary>
public class ParameterControllerBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the context for the parameter controller.
    /// </summary>
    [CascadingParameter]
    public required ParameterControllerContext Context { get; set; }

    /// <summary>
    /// Invokes the <see cref="ParameterControllerContext.OnInput"/> callback with the supplied value wrapped in a <see cref="ParameterInputEventArgs"/>.
    /// </summary>
    /// <param name="value">The new value to report.</param>
    protected async Task OnInputAsync(object? value)
    {
        if (this.Context is null || this.Context.Parameter is null)
            throw new InvalidOperationException("ParameterControllerContext is not initialized (missing or invalid cascading context).");
        await this.Context.OnInput.InvokeAsync(new(value, this.Context.Parameter));
    }
}
