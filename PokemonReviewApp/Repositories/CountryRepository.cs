using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public CountryRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper=mapper;
        }

        public bool CountryExists(int id)
        {
            return dataContext.Countries.Any(p => p.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return dataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return dataContext.Countries.Where(c => c.Id  == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return dataContext.Owners.Where(p => p.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return dataContext.Owners.Where(c => c.Country.Id == countryId).ToList();
        }
    }
}
