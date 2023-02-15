namespace Booklite.Interfaces;

using Booklite.Models;

public interface IBookSource
{
    Task<ICollection<BookInfo>> GetBooksAsync(string query); 
}
