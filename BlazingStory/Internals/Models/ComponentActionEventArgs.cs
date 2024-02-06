namespace BlazingStory.Internals.Models;

public class ComponentActionEventArgs
{
    internal readonly string Name;

    internal readonly string ArgsJson;

    public ComponentActionEventArgs(string name, string argsJson)
    {
        this.Name = name;
        this.ArgsJson = argsJson;
    }
}
