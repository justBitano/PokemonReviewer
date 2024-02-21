using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class CategoryRepository  : ICategoryRepository
    {
        private readonly  DataContext  dataContext;
        public CategoryRepository(DataContext dataContext) { 
        this.dataContext = dataContext;
        }

        public bool CategoriesExists(int categoryId)
        {
            return dataContext.Categories.Any(p =>  p.Id == categoryId); 
        }

        public ICollection<Category> GetCategories()
        {
            return dataContext.Categories.OrderBy(p => p.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return dataContext.Categories.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return dataContext.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c .Pokemon).ToList(); 

        }
    }
}
