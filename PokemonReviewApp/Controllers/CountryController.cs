using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController :  Controller
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper mapper;
        public CountryController(ICountryRepository _repository, IMapper mapper)
        {
            this._repository = _repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var list = mapper.Map<List<CountryDTO>>(_repository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_repository.CountryExists(countryId))
            {

                return NotFound();
            }
            var list = mapper.Map<CountryDTO>(_repository.GetCountry(countryId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country  = mapper.Map<CountryDTO>(_repository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(country);
        }
        [HttpGet("{countryId}/owner")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerFromACountry(int countryId)
        {
            var owners = _repository.GetOwnersFromACountry(countryId);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(owners);
        }
    }
}
