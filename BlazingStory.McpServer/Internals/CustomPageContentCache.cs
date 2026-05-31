using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BlazingStory.McpServer.Internals;

/// <summary>
/// A singleton cache that stores rendered HTML content of custom pages and maintains a lazy BM25 search index.
/// </summary>
internal class CustomPageContentCache
{
    private readonly ConcurrentDictionary<string, CustomPageCacheEntry> _cache = new();

    private readonly object _indexLock = new();

    private Bm25Index? _bm25Index;

    /// <summary>
    /// Tries to retrieve a cached entry by its navigation path.
    /// </summary>
    /// <param name="navigationPath">The navigation path to look up.</param>
    /// <param name="entry">The cached entry, if found.</param>
    /// <returns><c>true</c> if the entry was found; otherwise, <c>false</c>.</returns>
    public bool TryGetEntry(string navigationPath, [NotNullWhen(true)] out CustomPageCacheEntry? entry)
        => this._cache.TryGetValue(navigationPath, out entry);

    /// <summary>
    /// Stores a rendered page entry in the cache and invalidates the BM25 index.
    /// </summary>
    /// <param name="navigationPath">The navigation path used as the cache key.</param>
    /// <param name="entry">The entry containing the rendered page content.</param>
    public void SetEntry(string navigationPath, CustomPageCacheEntry entry)
    {
        this._cache[navigationPath] = entry;
        lock (this._indexLock) { this._bm25Index = null; }
    }

    /// <summary>
    /// Returns all cached entries.
    /// </summary>
    /// <returns>An enumerable of all cached page entries.</returns>
    public IEnumerable<CustomPageCacheEntry> GetAllEntries()
        => this._cache.Values;

    /// <summary>
    /// Returns the BM25 index, building it from cached entries on first access or after invalidation.
    /// </summary>
    /// <returns>A precomputed <see cref="Bm25Index"/> over all cached page content.</returns>
    public Bm25Index GetOrBuildIndex()
    {
        lock (this._indexLock)
        {
            if (this._bm25Index is not null) return this._bm25Index;

            var documents = this._cache.Values.Select(entry =>
            {
                var plainText = Bm25Scorer.StripHtmlTags(entry.HtmlContent);
                var searchableText = entry.Title + " " + plainText;
                return Bm25Scorer.CreateDocument(entry.NavigationPath, searchableText);
            }).ToArray();

            this._bm25Index = new Bm25Index(documents);
            return this._bm25Index;
        }
    }
}

/// <summary>
/// Represents a cached custom page with its rendered HTML content.
/// </summary>
/// <param name="Title">The title of the custom page.</param>
/// <param name="NavigationPath">The navigation path used as the cache key.</param>
/// <param name="HtmlContent">The rendered HTML content of the page.</param>
internal record CustomPageCacheEntry(string Title, string NavigationPath, string HtmlContent);
