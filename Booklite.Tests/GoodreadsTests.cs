using AngleSharp;
using Booklite.Services;
namespace Booklite.Tests;

[TestFixture]
public class GoodreadsTests
{
    private readonly Goodreads _goodreads = new(BrowsingContext.New());

    [Test]
    public async Task GetBooksTest()
    {
        // Arrange
        var query = "parenting";

        // Act
        var books = await _goodreads.GetBooksAsync(query);

        // Assert
        Assert.That(books, Is.Not.Null);
        Assert.That(books, Is.Not.Empty);

        var sameBooks = books.Where(b1 => books.Count(b2 => b1.Title == b2.Title) > 1);
        Assert.That(sameBooks.Count(), Is.EqualTo(0));

        foreach (var book in books)
        {
            Assert.That(book.Title, Is.Not.Null);
            Assert.That(!string.IsNullOrEmpty(book.Title));
            Assert.That(book.ImageUrl, Is.Not.Null);
            Assert.That(book.Authors, Is.Not.Null);
        }
    }
}