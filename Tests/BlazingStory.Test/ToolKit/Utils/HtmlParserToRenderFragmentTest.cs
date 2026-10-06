using BlazingStory.ToolKit.Utils;
using Bunit;

namespace BlazingStory.Test.ToolKit.Utils;

internal class HtmlParserToRenderFragmentTest
{
    private static string Render(string html)
    {
        using var ctx = new BunitContext();
        var cut = ctx.Render(html.ToRenderFragment());
        return cut.Markup;
    }

    [Test]
    public void ToRenderFragment_TextOnlyElement_Test()
    {
        Render("<span class=\"a\">Add new</span>")
            .Is("<span class=\"a\">Add new</span>");
    }

    [Test]
    public void ToRenderFragment_NestedElements_Test()
    {
        Render("<div><span>one</span><b>two</b></div>")
            .Is("<div><span>one</span><b>two</b></div>");
    }

    [Test]
    public void ToRenderFragment_TextAfterChildElement_IsKept_Test()
    {
        Render("<span class=\"a\"><i class=\"icon\"></i> Add new</span>")
            .Is("<span class=\"a\"><i class=\"icon\"></i> Add new</span>");
    }

    [Test]
    public void ToRenderFragment_TextBeforeChildElement_IsKept_Test()
    {
        Render("<span>Add new <i class=\"icon\"></i></span>")
            .Is("<span>Add new <i class=\"icon\"></i></span>");
    }

    [Test]
    public void ToRenderFragment_TextBetweenChildElements_IsKept_Test()
    {
        Render("<p><b>Note</b>: read <a href=\"#x\">this</a> first.</p>")
            .Is("<p><b>Note</b>: read <a href=\"#x\">this</a> first.</p>");
    }

    [Test]
    public void ToRenderFragment_TextNextToTopLevelElements_IsKept_Test()
    {
        Render("Some <b>bold</b> and plain")
            .Is("Some <b>bold</b> and plain");
    }

    [Test]
    public void ToRenderFragment_PlainText_IsRenderedAsItIs_Test()
    {
        Render("Fish &amp; Chips")
            .Is("Fish &amp; Chips");
    }
}
