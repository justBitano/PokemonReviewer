﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        ICollection<Pokemon> GetPokemonByCategory(int categoryId);

        bool CategoriesExists(int categoryId);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool Delete(int id);
        bool Save();


    }
}
