using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController :Controller
    {
        private readonly IReviewerRepository _repository;
        private readonly IMapper mapper;
        public ReviewerController(IReviewerRepository pokemonRepository, IMapper mapper)
        {
            this._repository = pokemonRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var list = mapper.Map<List<ReviewerDTO>>(_repository.GetReviewers());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }


        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var list = mapper.Map<Reviewer>(_repository.GetReviewer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var list = mapper.Map<List<ReviewDTO>>(_repository.GetReviewsByReviewer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }


    }
}
