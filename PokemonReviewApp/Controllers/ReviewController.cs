using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper mapper;
        public ReviewController(IReviewRepository pokemonRepository, IMapper mapper)
        {
            this._repository = pokemonRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var list = mapper.Map<List<ReviewDTO>>(_repository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_repository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var list = mapper.Map<ReviewDTO>(_repository.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{pokeId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var list = mapper.Map<List<ReviewDTO>>(_repository.GetReviewOfAPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery]int reviewerId, [FromQuery] int pokeId,[FromBody] ReviewDTO model)
        {
            if(model == null)
            {
                return BadRequest(ModelState);

            }
            var reviewMap = mapper.Map<Review>(model);
            var pokemon = _repository.CreateReview(reviewerId, pokeId, reviewMap);
            if (!pokemon)
            {
                return StatusCode(500, "Something went wrong while saving."); 
            }
            return Ok("Successfully created.");
        }

    }
}
