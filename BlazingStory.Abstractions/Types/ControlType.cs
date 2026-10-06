namespace BlazingStory.Types;

/// <summary>
/// Specifies the type of UI control used to edit a story parameter in the Controls panel.
/// </summary>
public enum ControlType
{
    /// <summary>
    /// Uses the default control automatically determined by the parameter type.
    /// </summary>
    Default,

    /// <summary>
    /// Uses radio buttons for selection.
    /// </summary>
    Radio,

    /// <summary>
    /// Uses a dropdown select box for selection.
    /// </summary>
    Select,

    /// <summary>
    /// Uses a color picker control.
    /// </summary>
    Color,

    /// <summary>
    /// Uses no control. The parameter is still listed in the Controls panel, and its current value is shown as text, but it can not be edited.
    /// </summary>
    None
}
