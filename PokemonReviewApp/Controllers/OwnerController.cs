﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;
using System.Net;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _repository;
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        public OwnerController(IOwnerRepository _repository, IMapper mapper, ICountryRepository countryRepository)
        {
            this._repository = _repository;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
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
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO model)
        {
            if (model == null)
            {
                return BadRequest("All field is required.");

            }
            var countries = countryRepository.GetCountry(countryId);
            if(countries == null)
            {
                return NotFound("Not found this country");
            }
            
            var ownerMap = mapper.Map<Owner>(model);
            var owner = _repository.CreateOwner(countryId,ownerMap);
            if (!owner)
            {
                return BadRequest("Something went wrong while saving.");
            }
            return Ok("Successfully created.");
        }

    }
}
