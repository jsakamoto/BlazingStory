using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Components;
using BlazingStory.Types;
using BlazingStoryApp1.Stories;
using Castle.Core.Internal;
using RazorClassLib1.Components.Button;
using RazorClassLib1.Components.Rating;
using static BlazingStory.Test._Fixtures.TestHelper;

namespace BlazingStory.Test.Internals.Services.Docs;

internal class StoriesRazorSourceTest
{
    [Test]
    public async Task GetSourceCodeAsync_Rating_RateControl_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Rating_stories);
        var storyAttribute = typeofStoriesRazor.GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, typeof(Rating), "Rate Control", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        sourceCode.Is("<Rating @attributes=\"context.Args\" />");
    }

    [Test]
    public async Task GetSourceCodeAsync_Button_Danger_Test()
    {
        // Given
        var typeofStoriesRazor = typeof(Button_stories);
        var storyAttribute = typeofStoriesRazor.GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, typeof(Button), "Danger", TestHelper.StoryContext.CreateEmpty(), null, null, EmptyFragment);

        // When
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then
        var expected = string.Join('\n', """
            <h1>
                <span style="display:inline-block; padding:12px 24px; margin:24px 12px;">
                    This is Button
                </span>
            </h1>

            <Button @attributes="context.Args" />
            """.Split('\n').Select(s => s.TrimEnd('\r')));
        sourceCode.Is(expected);
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_Test()
    {
        // Given
        var sourceText = "<Button @attributes=\"context.Args\" />";
        var story = TestHelper.CreateStory<Button>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Bold), true);
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Color), ButtonColor.Default);

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<Button Bold=\"true\"\n" +
            "        Color=\"ButtonColor.Default\" />");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_Override_Test()
    {
        // Given
        var sourceText =
            "<div>\n" +
            "    <Button Text=\"One+One=Two\" @attributes=\"context.Args\">\n" +
            "    </Button>\n" +
            "</div>\n";
        var story = TestHelper.CreateStory<Button>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Text), "Hello, World");
        await story.Context.AddOrUpdateArgumentAsync(nameof(Button.Bold), false);

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<div>\n" +
            "    <Button Text=\"Hello, World\"\n" +
            "            Bold=\"false\">\n" +
            "    </Button>\n" +
            "</div>\n");
    }

    [Test]
    public void UpdateSourceTextWithArgument_EmptyArgs_Test()
    {
        // Given
        var sourceText = "<Button Text=\"One+One=Two\" @attributes=\"context.Args\"></Button>";
        var story = TestHelper.CreateStory<Button>();

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is("<Button Text=\"One+One=Two\"></Button>");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_UnoverridableParam_Test()
    {
        // Given
        var sourceText = "<Rating.Rating Color='darkyellow' @attributes='context.Args' Rate=\"5\" />";
        var story = TestHelper.CreateStory<Rating>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(Rating.Rate), 3);
        await story.Context.AddOrUpdateArgumentAsync(nameof(Rating.Color), "gold");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is("<Rating.Rating Color='gold' Rate=\"5\" />");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_ChildContent_Test()
    {
        // Given
        var sourceText =
            "<div>\n" +
            "    <SampleComponent @attributes='context.Args'/>\n" +
            "</div>\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Number1), 123);
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.ChildContent), "Mazie errata suitor");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<div>\n" +
            "    <SampleComponent Number1=\"123\">\n" +
            "        Mazie errata suitor\n" +
            "    </SampleComponent>\n" +
            "</div>\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_ChildContent_and_RenderFragments_Test()
    {
        // Given
        var sourceText =
            "<div>\n" +
            "    <SampleComponent @attributes='context.Args'>\n" +
            "    </SampleComponent>\n" +
            "</div>\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Number1), 123);
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.ChildContent), "Sed cilia invading");
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Template1), "Labore dolor stet sed");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<div>\n" +
            "    <SampleComponent Number1=\"123\">\n" +
            "        <ChildContent>\n" +
            "            Sed cilia invading\n" +
            "        </ChildContent>\n" +
            "        <Template1>\n" +
            "            Labore dolor stet sed\n" +
            "        </Template1>\n" +
            "    </SampleComponent>\n" +
            "</div>\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_UnoverridableRenderFragments_Test()
    {
        // Given
        var sourceText =
            "<SampleComponent @attributes='context.Args'>\n" +
            "    <Template1>\n" +
            "        Ut rivière dolor dolore\n" +
            "    </Template1>\n" +
            "</SampleComponent>\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.ChildContent), "Vero aliquot dolor");
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Template1), "Kasid nullar lorem junto");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<SampleComponent>\n" +
            "    <ChildContent>\n" +
            "        Vero aliquot dolor\n" +
            "    </ChildContent>\n" +
            "    <Template1>\n" +
            "        Ut rivière dolor dolore\n" +
            "    </Template1>\n" +
            "</SampleComponent>\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_UnoverridableChildContent_Test()
    {
        // Given
        var sourceText =
            "<BlazingStory.Test._Fixtures.Components.SampleComponent @attributes='context.Args'>\n" +
            "    <div>\n" +
            "        In doglores facilizes accuses\n" +
            "    </div>\n" +
            "</BlazingStory.Test._Fixtures.Components.SampleComponent>\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.ChildContent), "Kasdan sed et");
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Template1), "Diam rivière magna");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<BlazingStory.Test._Fixtures.Components.SampleComponent>\n" +
            "    <div>\n" +
            "        In doglores facilizes accuses\n" +
            "    </div>\n" +
            "</BlazingStory.Test._Fixtures.Components.SampleComponent>\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_OverridableTemplateArg_Test()
    {
        // Given
        var sourceText = "<SampleComponent Template1=\"_template\" @attributes='context.Args' />\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Template1), "Sit sed no");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<SampleComponent>\n" +
            "    <Template1>\n" +
            "        Sit sed no\n" +
            "    </Template1>\n" +
            "</SampleComponent>\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_UnoverridableTemplateArg_Test()
    {
        // Given
        var sourceText = "<SampleComponent @attributes='context.Args' Template1=\"_template\" />\n";
        var story = TestHelper.CreateStory<SampleComponent>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleComponent.Template1), "Ea et herderite");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is("<SampleComponent Template1=\"_template\" />\n");
    }

    [Test]
    public async Task UpdateSourceTextWithArgument_GenericComponent_Test()
    {
        // Given
        var sourceText =
            "<SampleGenericComponent Items=\"_items\" Context=\"repeaterContext\" ItemTemplate=\"_itemTemplate\" @attributes=\"context.Args\">\r\n" +
            "</SampleGenericComponent>\r\n";
        var story = TestHelper.CreateStory<SampleGenericComponent<string>>();
        await story.Context.AddOrUpdateArgumentAsync(nameof(SampleGenericComponent<string>.ItemTemplate), "Ipsum tempol taction");

        // When
        var codeText = StoriesRazorSource.UpdateSourceTextWithArgument(story, sourceText);

        // Then
        codeText.Is(
            "<SampleGenericComponent Items=\"_items\" Context=\"repeaterContext\">\n" +
            "    <ItemTemplate>\n" +
            "        Ipsum tempol taction\n" +
            "    </ItemTemplate>\n" +
            "</SampleGenericComponent>\r\n");
    }
}
