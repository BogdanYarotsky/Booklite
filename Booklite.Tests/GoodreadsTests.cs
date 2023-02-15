using AngleSharp;
using Booklite.Services;

[TestFixture]
public class GoodreadsTests
{
    private Goodreads goodreads;

    [SetUp]
    public void Setup()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var browsingContext = BrowsingContext.New(config);
        goodreads = new Goodreads(browsingContext);
    }

    [Test]
    public async Task GetBooksTest()
    {
        // Arrange
        string query = "parenting";

        // Act
        var books = await goodreads.GetBooksAsync(query);

        // Assert
        Assert.That(books, Is.Not.Null);
        Assert.That(books.Count > 0);

        foreach (var book in books)
        {
            Assert.That(book.Title, Is.Not.Null);
            Assert.That(!string.IsNullOrEmpty(book.Title));
            Assert.IsNotNull(book.ImageUrl);
            Assert.IsNotNull(book.Authors);
        }
    }
}
