namespace Booklite.Services;

using AngleSharp;
using AngleSharp.Dom;
using Booklite.Interfaces;
using Booklite.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Goodreads : IBookSource
{
    private readonly IBrowsingContext _browsingContext;
    private const int _totalPagesToParse = 3;

    public Goodreads(IBrowsingContext browsingContext)
    {
        _browsingContext = browsingContext;
    }

    public async Task<ICollection<Book>> GetBooksAsync(string query)
    {
        var tasks = Enumerable
            .Range(1, _totalPagesToParse)
            .Select(n => ParseSearchPageAsync(n, query))
            .ToList();

        var results = await Task.WhenAll(tasks);

        var books = results
            .SelectMany(book => book)
            .DistinctBy(b => b.Title)
            .OrderByDescending(b => b.Score)
            .ToList(); // duplicates appear here

        return books;
    }

    private async Task<ICollection<Book>> ParseSearchPageAsync(int pageNo, string query)
    {
        var url = BuildSearchPageUrl(pageNo, query);
        var document = await _browsingContext.OpenAsync(url);
        return ParseBooks(document);
    }

    private static string BuildSearchPageUrl(int page, string query) => $"https://www.goodreads.com/search?page={page}&q={query}";

    private static ICollection<Book> ParseBooks(IDocument document)
    {
        var elements = document.QuerySelectorAll("tr[itemtype='http://schema.org/Book']");
        var books = elements.Select(ToBook).ToList();
        return books;
    }

    private static Book ToBook(IElement element)
    {
        var title = element.QuerySelector(".bookTitle")?.TextContent.Trim() ?? "";

        var imageUrl = element.QuerySelector(".bookCover")?.GetAttribute("src") ?? "";

        var authors = element.QuerySelectorAll(".authorName")
            .Select(author => author.TextContent.Trim())
            .ToList();

        var ratingInfo = element.QuerySelector(".minirating")?.TextContent.Trim();
        var strings = ratingInfo.Split(" ");

        _ = float.TryParse(strings[0], out var rating);
        _ = int.TryParse(strings[4].Replace(",", ""), out var totalRatings);

        return new Book(title, imageUrl, rating, totalRatings, authors);
    }
}
