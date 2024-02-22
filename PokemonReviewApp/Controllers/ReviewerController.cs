using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController :Controller
    {
        private readonly IReviewerRepository _repository;
        private readonly IMapper mapper;
        private readonly IReviewRepository reviewRepository;

        public ReviewerController(IReviewerRepository pokemonRepository, IMapper mapper, IReviewRepository reviewRepository)
        {
            this._repository = pokemonRepository;
            this.mapper = mapper;
            this.reviewRepository=reviewRepository;
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromBody] ReviewerDTO model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);

            }
            var reviewerMap = mapper.Map<Reviewer>(model);
            var reviewer = _repository.CreateReviewer(reviewerMap);
            if (!reviewer)
            {
                return StatusCode(500, "Something went wrong while saving.");
            }
            return Ok("Successfully created.");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO model)
        {
            if (model == null)
            {
                return StatusCode(400, "Something wrong with this reviewer.");
            }
            if (reviewerId != model.Id)
            {
                return BadRequest("Not same id reviewer.");
            }
            if (!_repository.ReviewerExists(reviewerId))
            {
                return StatusCode(404, "Not found review.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = mapper.Map<Reviewer>(model);
            if (!_repository.UpdateReviewer(result))
            {
                return StatusCode(500, "Something went wrong.");
            }
            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            var model = _repository.GetReviewer(reviewerId);
            List<Review> listReview = (List<Review>)_repository.GetReviewsByReviewer(reviewerId);
            if (model == null)
            {
                return NotFound("Dont have this pokemon Id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (listReview.Count < 1)
            {
                if (!_repository.DeleteReviewer(reviewerId))
                {
                    return StatusCode(500, "Something went wrong while delete pokemon.");
                }
            }
            else
            {
                if (!reviewRepository.DeleteListReview(listReview.ToList()))
                {
                    return StatusCode(500, "Something went wrong while delete reviews.");
                }
                if (!_repository.DeleteReviewer(reviewerId))
                {
                    return StatusCode(500, "Something went wrong while delete pokemon.");
                }
            }
            return Ok("Deleted successfully");
        }

    }
}
