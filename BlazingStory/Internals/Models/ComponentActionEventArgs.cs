namespace BlazingStory.Internals.Models;

public class ComponentActionEventArgs
{
    #region Internal Fields

    internal readonly string Name;

    internal readonly string ArgsJson;

    #endregion Internal Fields

    #region Public Constructors

    public ComponentActionEventArgs(string name, string argsJson)
    {
        this.Name = name;
        this.ArgsJson = argsJson;
    }

    #endregion Public Constructors
}
