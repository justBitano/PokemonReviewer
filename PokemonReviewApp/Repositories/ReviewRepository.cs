using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public bool CreateReview(int reviewerId, int pokeId, Review review)
        {
            var reviewer = dataContext.Reviewers.FirstOrDefault(x => x.Id == reviewerId);
            var poke = dataContext.Pokemons.FirstOrDefault(x => x.Id == pokeId);
            if (review == null || poke  == null)
            {
                return false;
            }
            review.Reviewer = reviewer;
            review.Pokemon = poke;
            dataContext.Add(review);
            return Save();
        }

        public bool DeleteListReview(List<Review> reviews)
        {
            dataContext.RemoveRange(reviews);
            return Save();
        }

        public bool DeleteReview(int id)
        {
            var review = GetReview(id);
            dataContext.Remove(review);
            return Save();
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

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            dataContext.Update(review);
            return Save();
        }
    }
}
