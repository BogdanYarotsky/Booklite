using AngleSharp;
using Booklite.Services;

namespace Booklite.Tests
{
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
            var query = "parenting";

            // Act
            var books = await goodreads.GetBooksAsync(query);

            // Assert
            Assert.That(books, Is.Not.Null);
            Assert.That(books, Is.Not.Empty);

            foreach (var book in books)
            {
                Assert.That(book.Title, Is.Not.Null);
                Assert.That(!string.IsNullOrEmpty(book.Title));
                Assert.That(book.ImageUrl, Is.Not.Null);
                Assert.That(book.Authors, Is.Not.Null);
            }
        }
    }
}