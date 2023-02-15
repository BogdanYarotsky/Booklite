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

    public Goodreads(IBrowsingContext browsingContext)
    {
        _browsingContext = browsingContext;
    }

    public async Task<ICollection<BookInfo>> GetBooksAsync(string query)
    {
        var tasks = Enumerable.Range(1, 3)
            .Select(page => ParseSearchPageAsync(page, query))
            .ToList();

        var results = await Task.WhenAll(tasks);
        var books = results.SelectMany(book => book).ToList();
        return books;
    }

    private async Task<ICollection<BookInfo>> ParseSearchPageAsync(int page, string query)
    {
        var url = BuildSearchPageUrl(page, query);
        var document = await _browsingContext.OpenAsync(url);
        return ParseBooks(document);
    }

    private static string BuildSearchPageUrl(int page, string query) => $"https://www.goodreads.com/search?page={page}&q={query}";

    private static ICollection<BookInfo> ParseBooks(IDocument document)
    {
        var elements = document.QuerySelectorAll("tr[itemtype='http://schema.org/Book']");
        var books = elements.Select(HtmlElementToBook);
        return elements.Select(HtmlElementToBook).ToList();
    }

    private static BookInfo HtmlElementToBook(IElement element)
    {
        var titleLink = element.QuerySelector(".bookTitle");
        var title = titleLink?.TextContent.Trim();
        var imageUrl = element.QuerySelector(".bookCover")?.GetAttribute("src");
        var authorLinks = element.QuerySelectorAll(".authorName");
        var authors = authorLinks.Select(authorLink => authorLink.TextContent.Trim());
        var ratingInfo = element.QuerySelector(".minirating")?.TextContent.Trim();
        var strings = ratingInfo.Split(" ");
        float.TryParse(strings[0], out var rating);
        int.TryParse(strings[4].Replace(",", ""), out var totalRatings);
        return new BookInfo
        {
            Title = title,
            ImageUrl = imageUrl,
            Authors = authors.ToList(),
            Rating = rating,
            RatingsCount = totalRatings
        };
    }

}
