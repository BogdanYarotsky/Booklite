namespace Booklite.Models;
public class BookInfo
{
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public float Rating { get; set; }
    public int RatingsCount { get; set; }
    public ICollection<string> Authors { get; set; }
}
