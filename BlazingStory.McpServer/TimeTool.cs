using System.ComponentModel;
using ModelContextProtocol.Server;

namespace BlazingStory.McpServer;

[McpServerToolType]
internal class TimeTool
{
    [McpServerTool(Name = "getCurrentTime")]
    [Description("Returns the current date and time in the 'yyyy-MM-dd HH:mm:ss' format.")]
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
