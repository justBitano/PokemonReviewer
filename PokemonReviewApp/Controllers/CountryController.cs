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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO model)
        {
            if (model == null)
            {
                return StatusCode(400, "This field is required.");

            }
            var countries = _repository.GetCountries().Where(c => c.Name.Trim().ToUpper() == model.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (countries != null)
            {
                return StatusCode(442, "This country already exists.");
            }
            var countryMap = mapper.Map<Country>(model);
            var country = _repository.CreateCountry(countryMap);
            if (!country)
            {
                return StatusCode(500, "Something went wrong while saving.");
            }
            return Ok("Successfully created.");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO country)
        {
            if (country == null)
            {
                return StatusCode(400, "Something wrong with category.");
            }
            if (countryId != country.Id)
            {
                return BadRequest("Not same id country.");
            }
            if (!_repository.CountryExists(countryId))
            {
                return StatusCode(404, "Not found country.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = mapper.Map<Country>(country);
            if (!_repository.UpdateCountry(countryMap))
            {
                return StatusCode(500, "Something went wrong.");
            }
            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteCountry(int countryId)
        {
            var country = _repository.GetCountry(countryId);
            if (country == null)
            {
                return NotFound("Dont have this country Id");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_repository.DeleteCountry(countryId))
            {
                return StatusCode(500, "Something went wrong.");
            }
            return Ok("Deleted successfully");
        }
    }
}
