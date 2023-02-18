namespace Booklite.Models;
public record Book(string Title, string ImageUrl, float Rating, int TotalReviews, ICollection<string> Authors, string Url)
{
    // think about threshold for reviews count, after certain amount not that important
    private const double _ratingWeight = 0.92;
    private const double _totalReviewsWeight = 0.08;
    public double Score => CalculateScore();
    private double CalculateScore()
    {
        var score = Rating * _ratingWeight + Math.Log(TotalReviews) * _totalReviewsWeight;
        return double.Round(score, 2);
    }
}
