using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        public OwnerRepository(DataContext dataContext, IMapper mapper) {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public bool CreateOwner(int countryId, Owner owner)
        {
            var country = dataContext.Countries.FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return false;
            }
            owner.Country = country;
            dataContext.Add(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return dataContext.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();

        }

        public ICollection<Owner> GetOwners()
        {
            return dataContext.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return dataContext.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(o => o.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return dataContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
           var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
