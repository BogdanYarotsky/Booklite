namespace Booklite.Models;

using System;

public class Book
{
    public BookInfo Info { get; }
    public double Score { get; }

    public Book(BookInfo bookInfo)
    {
        Info = bookInfo;
        Score = CalculateScore(bookInfo);
    }

    private double CalculateScore(BookInfo book)
    {
        var weighted = book.Rating * 0.3 + Math.Log(book.RatingsCount) * 0.7; 
        return double.Round(weighted, 2);
    }
}
