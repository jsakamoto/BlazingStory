using System.Collections;
using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

public class ComponentActionLogs : IEnumerable<ComponentActionLog>
{
    #region Private Fields

    private readonly List<ComponentActionLog> _actionLogs = new();

    #endregion Private Fields

    #region Internal Events

    internal event EventHandler? Updated;

    #endregion Internal Events

    #region Public Methods

    public IEnumerator<ComponentActionLog> GetEnumerator() => this._actionLogs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion Public Methods

    #region Internal Methods

    internal void Add(string name, string argsJson)
    {
        var lastLog = this._actionLogs.FirstOrDefault();

        if (lastLog != null && lastLog.Name == name && lastLog.ArgsJson == argsJson)
        {
            lastLog.Repeat++;
        }
        else
        {
            this._actionLogs.Insert(0, new ComponentActionLog(name, argsJson));
        }

        this.NotifyUpdated();
    }

    internal void Clear()
    {
        this._actionLogs.Clear();
        this.NotifyUpdated();
    }

    #endregion Internal Methods

    #region Private Methods

    private void NotifyUpdated()
    {
        this.Updated?.Invoke(this, EventArgs.Empty);
    }

    #endregion Private Methods
}
