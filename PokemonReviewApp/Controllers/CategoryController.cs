using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections;

namespace PokemonReviewApp.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository  _repository;
        private readonly IMapper mapper;
        public CategoryController(ICategoryRepository _repository, IMapper mapper)
        {
            this._repository = _repository;
            this.mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var list = mapper.Map<List<CategoryDTO>>(_repository.GetCategories());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_repository.CategoriesExists(categoryId)){
                return NotFound();
            }
            var list = mapper.Map<CategoryDTO>(_repository.GetCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(list);
        }

        [HttpGet("{categoryId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemon = mapper.Map<List<PokemonDTO>>
                (_repository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);

            }
            var categories = _repository.GetCategories().Where(c => c.Name.Trim().ToUpper() ==  model.Name.Trim().ToUpper()).FirstOrDefault();
            if(categories != null)
            {
                ModelState.AddModelError("", "This category already exists.");
                return StatusCode(442, ModelState);
            }
            var reviewerMap = mapper.Map<Category>(model);
            var reviewer = _repository.CreateCategory(reviewerMap);
            if (!reviewer)
            {
                ModelState.AddModelError("", "Something went wrong while saving.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created.");
        }

    }
}
