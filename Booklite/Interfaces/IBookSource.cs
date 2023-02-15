namespace Booklite.Interfaces;

using Booklite.Models;

public interface IBookSource
{
    Task<ICollection<Book>> GetBooksAsync(string query); 
}
