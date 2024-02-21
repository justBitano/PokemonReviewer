using AutoMapper;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfies : Profile
    {
        public MappingProfies() {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Owner, OwnerDTO>();
            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<Review, ReviewDTO>();
        }
    }
}
