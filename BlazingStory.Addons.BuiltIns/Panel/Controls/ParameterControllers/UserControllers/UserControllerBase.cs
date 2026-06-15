using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.UserControllers;

/// <summary>
/// Base class for user provided controllers
/// </summary>
public class UserControllerBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the context for the controller.
    /// </summary>
    [CascadingParameter(Name = "Context")]
    public required UserControllerContext Context { get; set; }

    /// <summary>
    /// Invokes the <see cref="UserControllerContext.OnInput"/> callback with the supplied value wrapped in a <see cref="ParameterInputEventArgs"/>.
    /// </summary>
    /// <param name="value">The new value to report.</param>
    protected async Task OnInputAsync(object? value)
    {
        if (this.Context is null || this.Context.Parameter is null)
            throw new InvalidOperationException("UserControllerContext is not initialized (missing or invalid cascading context).");
        await this.Context.OnInput.InvokeAsync(new ParameterInputEventArgs(value, this.Context.Parameter));
    }
}
