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
    /// <summary>
    /// Gets or sets the component parameter. The component parameter is specified by using a lambda
    /// expression. For example: (x) =&gt; x.MyParameter. The lambda expression must be a member
    /// access expression. The member access expression must be a member of the component type.
    /// </summary>
    [Parameter]
    public Expression<Func<TComponent, TParameter>>? For { get; set; }

    /// <summary>
    /// Gets or sets the type of control for the component parameter. The type of control for the
    /// component parameter. The default value is ControlType.Default. The ControlType enumeration
    /// contains the following values: Default, Radio, Select, and Color. The ControlType
    /// enumeration is defined in the BlazingStory.Types namespace.
    /// </summary>
    [Parameter]
    public ControlType Control { get; set; } = ControlType.Default;

    /// <summary>
    /// The default value of the component parameter. The default value of the component parameter.
    /// The default value is null. The default value is used when the component parameter is not specified.
    /// </summary>
    [Parameter]
    public object? DefaultValue { get; set; }

    /// <summary>
    /// The options of the component parameter. The options of the component parameter. The default
    /// value is an empty array.
    /// </summary>
    [Parameter]
    public string[] Options { get; set; } = Array.Empty<string>();

    [CascadingParameter]
    internal Stories<TComponent>? Stories { get; set; }

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
}
