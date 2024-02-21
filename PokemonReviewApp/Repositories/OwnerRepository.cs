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
    }
}
