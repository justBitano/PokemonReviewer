using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        public ReviewerRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            dataContext.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return dataContext.Reviewers.Where(c => c.Id ==  reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return dataContext.Reviewers.ToList();  
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return dataContext.Reviews.Where(c => c.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return dataContext.Reviewers.Any(c => c.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
