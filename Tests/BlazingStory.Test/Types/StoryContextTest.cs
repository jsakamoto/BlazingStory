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
        parameterCount.Is(3);
    }
}
