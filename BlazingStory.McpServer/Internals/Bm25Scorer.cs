using System.Text.RegularExpressions;

namespace BlazingStory.McpServer.Internals;

/// <summary>
/// Provides BM25 Okapi ranking for full-text search over custom page content.
/// </summary>
internal static partial class Bm25Scorer
{
    private const double K1 = 1.5;
    private const double B = 0.75;

    /// <summary>
    /// Scores documents in the index against the given query, returning results ordered by relevance.
    /// </summary>
    /// <param name="index">The precomputed BM25 index containing documents and their term frequencies.</param>
    /// <param name="query">The search query string.</param>
    /// <returns>A list of results with positive scores, sorted by descending relevance.</returns>
    public static IReadOnlyList<Bm25Result> Search(Bm25Index index, string query)
    {
        var docs = index.Documents;
        if (docs.Length == 0) return [];

        var queryTerms = Tokenize(query);
        if (queryTerms.Length == 0) return [];

        var avgDocLength = index.AvgDocLength;
        var totalDocs = docs.Length;

        var results = new List<Bm25Result>();
        foreach (var doc in docs)
        {
            var score = 0.0;
            foreach (var term in queryTerms)
            {
                var df = index.DocumentFrequencies.GetValueOrDefault(term, 0);
                var idf = Math.Log((totalDocs - df + 0.5) / (df + 0.5) + 1.0);

                var tf = doc.TermFrequencies.GetValueOrDefault(term, 0);
                var numerator = tf * (K1 + 1.0);
                var denominator = tf + K1 * (1.0 - B + B * doc.TokenCount / avgDocLength);

                score += idf * numerator / denominator;
            }

            if (score > 0)
            {
                results.Add(new Bm25Result(doc.Key, score));
            }
        }

        results.Sort((a, b) => b.Score.CompareTo(a.Score));
        return results;
    }

    /// <summary>
    /// Creates a BM25 document from raw text by tokenizing and computing term frequencies.
    /// </summary>
    /// <param name="key">A unique key identifying the document.</param>
    /// <param name="text">The plain text content to index.</param>
    /// <returns>A <see cref="Bm25Document"/> ready for inclusion in a <see cref="Bm25Index"/>.</returns>
    public static Bm25Document CreateDocument(string key, string text)
    {
        var tokens = Tokenize(text);
        var termFrequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var token in tokens)
        {
            termFrequencies[token] = termFrequencies.GetValueOrDefault(token, 0) + 1;
        }
        return new Bm25Document(key, tokens.Length, termFrequencies);
    }

    /// <summary>
    /// Strips HTML tags and comments from the given HTML string, returning plain text.
    /// </summary>
    /// <param name="html">The HTML content to strip.</param>
    /// <returns>The plain text with HTML tags and comments replaced by spaces.</returns>
    public static string StripHtmlTags(string html)
    {
        var withoutComments = HtmlCommentRegex().Replace(html, " ");
        return HtmlTagRegex().Replace(withoutComments, " ");
    }

    /// <summary>
    /// Extracts a text snippet centered around the first occurrence of a query term in the given plain text.
    /// </summary>
    /// <param name="plainText">The plain text to extract a snippet from.</param>
    /// <param name="query">The search query used to locate the snippet center.</param>
    /// <param name="contextChars">The number of characters to include before and after the match.</param>
    /// <returns>A snippet string, prefixed/suffixed with "..." if truncated.</returns>
    public static string ExtractSnippet(string plainText, string query, int contextChars = 200)
    {
        var queryTerms = Tokenize(query);
        if (queryTerms.Length == 0) return Truncate(plainText, contextChars * 2);

        var bestIndex = -1;
        foreach (var term in queryTerms)
        {
            var index = plainText.IndexOf(term, StringComparison.OrdinalIgnoreCase);
            if (index >= 0 && (bestIndex < 0 || index < bestIndex))
            {
                bestIndex = index;
            }
        }

        if (bestIndex < 0) return Truncate(plainText, contextChars * 2);

        var start = Math.Max(0, bestIndex - contextChars);
        var end = Math.Min(plainText.Length, bestIndex + contextChars);

        var snippet = plainText[start..end];
        if (start > 0) snippet = "..." + snippet;
        if (end < plainText.Length) snippet += "...";

        return CollapseWhitespace(snippet);
    }

    private static string Truncate(string text, int maxLength)
    {
        var collapsed = CollapseWhitespace(text);
        if (collapsed.Length <= maxLength) return collapsed;
        return collapsed[..maxLength] + "...";
    }

    private static string CollapseWhitespace(string text)
    {
        return WhitespaceRegex().Replace(text, " ").Trim();
    }

    private static string[] Tokenize(string text)
    {
        return TokenRegex().Matches(text)
            .Select(m => m.Value.ToLowerInvariant())
            .Where(t => t.Length >= 2)
            .ToArray();
    }

    [GeneratedRegex(@"<!--.*?-->", RegexOptions.Singleline)]
    private static partial Regex HtmlCommentRegex();

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"\w+")]
    private static partial Regex TokenRegex();
}

/// <summary>
/// Represents a tokenized document with precomputed term frequencies for BM25 scoring.
/// </summary>
/// <param name="Key">A unique key identifying the document.</param>
/// <param name="TokenCount">The total number of tokens in the document.</param>
/// <param name="TermFrequencies">A dictionary mapping each token to its occurrence count.</param>
internal record Bm25Document(string Key, int TokenCount, Dictionary<string, int> TermFrequencies);

/// <summary>
/// Represents a BM25 search result with a document key and relevance score.
/// </summary>
/// <param name="Key">The key of the matched document.</param>
/// <param name="Score">The BM25 relevance score (higher is more relevant).</param>
internal record Bm25Result(string Key, double Score);

/// <summary>
/// A precomputed BM25 index containing documents, average document length, and document frequencies.
/// </summary>
internal class Bm25Index
{
    /// <summary>
    /// Gets the indexed documents.
    /// </summary>
    public Bm25Document[] Documents { get; }

    /// <summary>
    /// Gets the average document length across all documents.
    /// </summary>
    public double AvgDocLength { get; }

    /// <summary>
    /// Gets the document frequency for each term (number of documents containing the term).
    /// </summary>
    public Dictionary<string, int> DocumentFrequencies { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bm25Index"/> class, precomputing document frequencies and average length.
    /// </summary>
    /// <param name="documents">The documents to index.</param>
    public Bm25Index(Bm25Document[] documents)
    {
        this.Documents = documents;
        this.AvgDocLength = documents.Length > 0 ? Math.Max(1.0, documents.Average(d => d.TokenCount)) : 1.0;

        this.DocumentFrequencies = new(StringComparer.OrdinalIgnoreCase);
        foreach (var doc in documents)
        {
            foreach (var term in doc.TermFrequencies.Keys)
            {
                this.DocumentFrequencies[term] = this.DocumentFrequencies.GetValueOrDefault(term, 0) + 1;
            }
        }
    }
}
