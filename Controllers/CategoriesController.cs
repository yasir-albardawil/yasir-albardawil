using AutoMapper;
using PlateWebAPI.DTOs;
using PlateWebAPI.Entities;
using PlateWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PlateWebAPI.Controllers
{
    //[Authorize(Roles = "SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories(string? search, string? sortOrder)
        {
            try
            {
                var categories = _categoryRepository.AllCategories;

                if (!string.IsNullOrEmpty(search))
                {
                    categories = categories.Where(c => c.CategoryName.Contains(search));
                }

                categories = sortOrder switch
                {
                    "name_desc" => categories.OrderByDescending(p => p.CategoryName),
                    "id_desc" => categories.OrderByDescending(p => p.CategoryId),
                    _ => categories.OrderBy(p => p.CategoryName),
                };


                return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int? id)
        {
            try
            {
                var category = await _categoryRepository.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<CategoryDTO>(category));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(CategoryCreatingDTO categoryCreatingDto)
        {
            try
            {
     
                var category = _mapper.Map<Category>(categoryCreatingDto);
                await _categoryRepository.AddAsync(category);

                var createdCategoryDto = _mapper.Map<CategoryDTO>(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategoryDto.CategoryId }, createdCategoryDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CategoryUpdatingDTO categoryUpdatingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = await _categoryRepository.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                _mapper.Map(categoryUpdatingDto, category);

                await _categoryRepository.SaveChangesAsync();

                var updatedCategoryDto = _mapper.Map<CategoryDTO>(category);
                return Ok(updatedCategoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            bool hasItems = await _categoryRepository.DoesCategoryHaveItemsAsync(id);
            if (hasItems)
            {
                return BadRequest(new {message = "The category cannot be deleted because it has associated items." });
            }

            await _categoryRepository.DeleteAsync(category.CategoryId);
            return NoContent();
        }
    }
}
