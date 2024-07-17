using System.Linq.Expressions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Components;

/// <summary>
/// You can configure the type of control for the component parameters via the ArgType component
/// inside the Stories component. The For lambda expression parameter of the ArgType component
/// indicates which parameters to be applied the control type configuration. And the Control
/// parameter of the ArgType component is used for specifying which type of control uses in the
/// "Control" panel to input parameter value.
/// </summary>
/// <typeparam name="TComponent">
/// The type of the component.
/// </typeparam>
/// <typeparam name="TParameter">
/// The type of the parameter.
/// </typeparam>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
public partial class ArgType<TComponent, TParameter> : ComponentBase where TComponent : notnull
{
    #region Public Properties

    [Parameter]
    public Expression<Func<TComponent, TParameter>>? For { get; set; }

    [Parameter]
    public ControlType Control { get; set; } = ControlType.Default;

    [Parameter]
    public object? DefaultValue { get; set; }

    [Parameter]
    public string[] Options { get; set; } = Array.Empty<string>();

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal Stories<TComponent>? Stories { get; set; }

    #endregion Internal Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        if (this.Stories == null)
        {
            throw new InvalidOperationException($"The Stories cascading parameter is required.");
        }

        var parameterName = ParameterExtractor.GetParameterName(this.For);

        if (this.Stories.ComponentParameters.TryGetByName(parameterName, out var parameter))
        {
            parameter.Control = this.Control;
            parameter.DefaultValue = this.DefaultValue;
            parameter.Options = this.Options;
        }

        this.Stories.ArgProps.Add(new ArgProp() { Name = parameterName, Control = this.Control, DefaultValue = this.DefaultValue, Options = this.Options });
    }

    #endregion Protected Methods
}
