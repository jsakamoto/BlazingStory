namespace BlazingStoryMcpApp1.Components;

/// <summary>
/// How prominent an <see cref="AlertBox"/> should be.
/// </summary>
public enum AlertSeverity
{
    /// <summary>Neutral, informational message.</summary>
    Info,

    /// <summary>Something needs the user's attention.</summary>
    Warning,

    /// <summary>Something went wrong.</summary>
    Error,
}
