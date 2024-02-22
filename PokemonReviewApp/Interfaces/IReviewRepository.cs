using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();

        Review GetReview(int reviewId);

        ICollection<Review> GetReviewOfAPokemon(int pokeId);


        bool ReviewExists(int id);

        bool CreateReview(int reviewerId, int pokeId, Review review);

        bool UpdateReview(Review review);

        bool DeleteReview(int id);

        bool DeleteListReview(List<Review> reviews);
        bool Save();
    }
}
