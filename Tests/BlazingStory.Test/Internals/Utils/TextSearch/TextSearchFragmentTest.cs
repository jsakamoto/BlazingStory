using BlazingStory.Internals.Utils.TextSearch;

namespace BlazingStory.Test.Internals.Utils.TextSearch;

internal class TextSearchFragmentTest
{
    [Test]
    public void CreateFragments_Empty_Test()
    {
        TextSearchFragment.CreateFragments(text: "", keywords: new[] { "foo", "bar" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|");
    }

    [Test]
    public void CreateFragments_NothingToMatch_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: Enumerable.Empty<string>())
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|Default");

        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "foo", "bar" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|Default");
    }

    [Test]
    public void CreateFragments_Match_StartsWith_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "def" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("Match|Def", "None|ault");
    }

    [Test]
    public void CreateFragments_Match_EndsWith_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "ult" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|Defa", "Match|ult");
    }

    [Test]
    public void CreateFragments_Match_Includes_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "fau" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|De", "Match|fau", "None|lt");
    }

    [Test]
    public void CreateFragments_Match_MultiTimes_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "ul", "ef" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|D", "Match|ef", "None|a", "Match|ul", "None|t");
    }

    [Test]
    public void CreateFragments_Match_SameKeywords_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "De", "ul", "De" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("Match|De", "None|fa", "Match|ul", "None|t");
    }

    [Test]
    public void CreateFragments_Match_Overwrapped_Test()
    {
        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "fau", "Def" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("Match|Defau", "None|lt");

        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "au", "lt" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("None|Def", "Match|ault");

        TextSearchFragment.CreateFragments(text: "Default", keywords: new[] { "De", "au", "ult" })
            .Select(f => $"{f.Type}|{f.Text}")
            .Is("Match|De", "None|f", "Match|ault");
    }
}
