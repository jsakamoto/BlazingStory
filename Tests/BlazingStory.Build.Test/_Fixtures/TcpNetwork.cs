using System.Net.NetworkInformation;

namespace BlazingStory.Build.Test._Fixtures;

/// <summary>
/// Provides methods to work with TCP network operations.
/// </summary>
internal static class TcpNetwork
{
    /// <summary>
    /// Gets a free TCP port number.
    /// </summary>
    /// <returns>A free TCP port number.</returns>
    public static int GetAvailableTCPv4Port() => EnumCandidatePorts().First(port => GetUsedTcpPorts().All(usedPort => usedPort != port));

    /// <summary>
    /// Gets the list of used TCP ports.
    /// </summary>
    /// <returns>An enumerable of used TCP port numbers.</returns>
    private static IEnumerable<int> GetUsedTcpPorts() => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Select(tcp => tcp.LocalEndPoint.Port);

    private static int _portNumber = 6000;

    /// <summary>
    /// Enumerates candidate TCP port numbers.
    /// </summary>
    /// <returns>An enumerable of candidate TCP port numbers.</returns>
    private static IEnumerable<int> EnumCandidatePorts()
    {
        for (; ; ) { yield return Interlocked.Increment(ref _portNumber); }
    }
}
