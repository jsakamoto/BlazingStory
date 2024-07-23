using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Components.Icons;

public partial class KeyMap : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public HotKeyCombo? Key { get; set; }

    [Parameter]
    public bool FreeSize { get; set; }

    [Parameter]
    public string? Class { get; set; }

    #endregion Public Properties

    #region Private Methods

    private IEnumerable<string> EnumKeyText()
    {
        if (this.Key == null) yield break;
        if (this.Key.Modifiers.HasFlag(ModCode.Alt)) yield return "alt";
        if (this.Key.Modifiers.HasFlag(ModCode.Ctrl)) yield return "⌃";
        if (this.Key.Modifiers.HasFlag(ModCode.Shift)) yield return "⇧​";
        yield return KeyCodeMapper.GetKeyTextFromCode(this.Key.Code);
    }

    #endregion Private Methods
}
