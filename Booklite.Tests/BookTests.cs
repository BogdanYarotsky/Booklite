namespace Booklite.Tests;

using Booklite.Models;

[TestFixture]
public class BookTests
{
    [Test]
    public void BooksAreCorrectlySortedByScore()
    {
        var books = new List<Book>
        {
            New("A", 4.7f, 32000),
            New("B", 4.8f, 8000),
            New("C", 4.6f, 50000),
            New("D", 5.0f, 280),
        };

        var sortedBooks = books.OrderByDescending(b => b.Score).Select(b => b.Title).ToArray();

        Assert.That(sortedBooks[0] == "A");
        Assert.That(sortedBooks[1] == "B");
        Assert.That(sortedBooks[2] == "C");
        Assert.That(sortedBooks[3] == "D");
    }

    public static Book New(string title, float rating, int totalReviews)
    {
        return new Book(title, "", rating, totalReviews, new List<string>(), "");
    }
}
