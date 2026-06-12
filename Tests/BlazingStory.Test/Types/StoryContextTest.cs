using BlazingStory.Internals.Services;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Components;
using BlazingStory.Types;

namespace BlazingStory.Test.Types;

internal class StoryContextTest
{
    [Test]
    public void GetParameterCount_Test()
    {
        // Given
        var parameters = ParameterExtractor.GetParametersFromComponentType(typeof(SampleComponent), XmlDocComment.Dummy);
        var context = new StoryContext(parameters);

        // When
        var parameterCount = context.GetNoEventParameterCount();

        // Then
        parameterCount.Is(5);
    }

    [Test]
    public async Task AddOrUpdateArgumentAsync_Fires_ArgumentChanged_When_Value_Changes_Test()
    {
        // Given
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();
        var context = (StoryContext)story.Context;
        context.InitArgument(nameof(TwoWayBindingComponent.Value), "initial");

        var argumentChangedCount = 0;
        context.ArgumentChanged += () =>
        {
            argumentChangedCount++;
            return ValueTask.CompletedTask;
        };

        // When
        await context.AddOrUpdateArgumentAsync(nameof(TwoWayBindingComponent.Value), "updated");

        // Then
        argumentChangedCount.Is(1);
        context.Args[nameof(TwoWayBindingComponent.Value)].Is("updated");
    }

    [Test]
    public async Task AddOrUpdateArgumentAsync_Does_Not_Fire_ArgumentChanged_When_Value_Is_Unchanged_Test()
    {
        // Given
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();
        var context = (StoryContext)story.Context;
        context.InitArgument(nameof(TwoWayBindingComponent.Value), "initial");

        var argumentChangedCount = 0;
        context.ArgumentChanged += () =>
        {
            argumentChangedCount++;
            return ValueTask.CompletedTask;
        };

        // When
        await context.AddOrUpdateArgumentAsync(nameof(TwoWayBindingComponent.Value), "initial");

        // Then
        argumentChangedCount.Is(0);
        context.Args[nameof(TwoWayBindingComponent.Value)].Is("initial");
    }

    [Test]
    public async Task UpdateArgumentsAsync_Updates_Multiple_Values_And_Fires_Once_Test()
    {
        // Given
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();
        var context = (StoryContext)story.Context;
        context.InitArgument(nameof(TwoWayBindingComponent.Value), "initial");
        context.InitArgument(nameof(TwoWayBindingComponent.Count), 1);

        var argumentChangedCount = 0;
        context.ArgumentChanged += () =>
        {
            argumentChangedCount++;
            return ValueTask.CompletedTask;
        };

        // When
        await context.UpdateArgumentsAsync(new Dictionary<string, object?>
        {
            [nameof(TwoWayBindingComponent.Value)] = "updated",
            [nameof(TwoWayBindingComponent.Count)] = 2,
        });

        // Then
        argumentChangedCount.Is(1);
        context.Args[nameof(TwoWayBindingComponent.Value)].Is("updated");
        context.Args[nameof(TwoWayBindingComponent.Count)].Is(2);
    }

    [Test]
    public async Task UpdateArgumentsAsync_Does_Not_Fire_When_No_Value_Changes_Test()
    {
        // Given
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();
        var context = (StoryContext)story.Context;
        context.InitArgument(nameof(TwoWayBindingComponent.Value), "initial");
        context.InitArgument(nameof(TwoWayBindingComponent.Count), 1);

        var argumentChangedCount = 0;
        context.ArgumentChanged += () =>
        {
            argumentChangedCount++;
            return ValueTask.CompletedTask;
        };

        // When
        await context.UpdateArgumentsAsync(new Dictionary<string, object?>
        {
            [nameof(TwoWayBindingComponent.Value)] = "initial",
            [nameof(TwoWayBindingComponent.Count)] = 1,
        });

        // Then
        argumentChangedCount.Is(0);
        context.Args[nameof(TwoWayBindingComponent.Value)].Is("initial");
        context.Args[nameof(TwoWayBindingComponent.Count)].Is(1);
    }
}
