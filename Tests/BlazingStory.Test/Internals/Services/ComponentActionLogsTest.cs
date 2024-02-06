using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

public class ComponentActionLogsTest
{
    [Test]
    public void Add_Test()
    {
        // Given
        var updatedCounter = 0;
        var logs = new ComponentActionLogs();
        logs.Updated += (sender, args) => { updatedCounter++; };

        // When
        logs.Add("foo", "1");
        logs.Add("foo", "1");
        logs.Add("bar", "1");
        logs.Add("bar", "2");

        // Then
        updatedCounter.Is(4);
        logs.Count().Is(3);
        logs.Select(log => $"{log.Repeat}|{log.Name}|{log.ArgsJson}")
            .Is("1|bar|2",
                "1|bar|1",
                "2|foo|1");
    }
}
