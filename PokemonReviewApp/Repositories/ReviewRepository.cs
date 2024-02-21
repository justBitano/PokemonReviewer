using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        public ReviewRepository(DataContext dataContext, IMapper mapper) { 
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public Review GetReview(int reviewId)
        {
            return dataContext.Reviews.Where(o => o.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewOfAPokemon(int pokeId)
        {
            return dataContext.Reviews.Where(c => c.Pokemon.Id ==  pokeId).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return dataContext.Reviews.OrderByDescending(c => c.Id).ToList();
        }

        public bool ReviewExists(int id)
        {
           return dataContext.Reviews.Any(o => o.Id == id);
        }
    }
}
