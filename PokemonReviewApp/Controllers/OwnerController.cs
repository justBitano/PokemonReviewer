using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _repository;
        private readonly IMapper mapper;
        public OwnerController(IOwnerRepository _repository, IMapper mapper)
        {
            this._repository = _repository;
            this.mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var list = mapper.Map<List<OwnerDTO>>(_repository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int ownerId)
        {
            if (!_repository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var list = mapper.Map<OwnerDTO>(_repository.GetOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_repository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var pokemon = mapper.Map<List<PokemonDTO>>(_repository.GetPokemonByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemon);
        }


    }
}
