
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _repository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        public PokemonController(IPokemonRepository pokemonRepository, DataContext dataContext, IMapper  mapper
            , IOwnerRepository ownerRepository) 
        {
            this._repository = pokemonRepository;
            this.dataContext = dataContext;
            this.mapper = mapper;
            this._ownerRepository = ownerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDTO>>(_repository.GetPokemons());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200,  Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if(!_repository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = mapper.Map<PokemonDTO>(_repository.GetPokemon(pokeId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_repository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var ratting = _repository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(ratting);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId ,[FromBody] PokemonDTO pokemon)
        {
            if(pokemon == null)
            {
                return BadRequest(ModelState);
            }
            var listPokemon = _repository.GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemon.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if( listPokemon != null)
            {
                return BadRequest("This pokemon already exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = mapper.Map<Pokemon>(pokemon); 

            if(!_repository.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState); 
            }
            return Ok("Successfully created.");
        }
    }   
}
