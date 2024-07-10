using AutoMapper;
using PlateWebAPI.Entities;
using PlateWebAPI.Models;
using PlateWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PlateWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
       // private readonly ILogger<ItemsController> _logger;
        public ItemsController(IItemRepository itemRepository, ICategoryRepository categoryRepository, IMapper mapper, ILogger<ItemsController> logger)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
          //  _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems(string? search, string? sortOrder)
        {
            try
            {

                var items = await _itemRepository.GetItemsAsync();

            //    _logger.LogInformation("Test log");

                if (!string.IsNullOrEmpty(search))
                {
                    items = items.Where(p => 
                    p.Name.Contains(search) || 
                    p.LongDescription.Contains(search));
                }

                items = sortOrder switch
                {
                    "name_desc" => items.OrderByDescending(p => p.Name),
                    "id_desc" => items.OrderByDescending(p => p.Id),
                    _ => items.OrderBy(p => p.Name),
                };


                if (items == null || !items.Any())
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<ItemDTO>>(items));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            try
            {
                var item = await _itemRepository.GetItemByIdAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ItemDTO>(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> Create(ItemCreatingDTO itemCreatingDto)
        {
            try
            {
                var category = await _categoryRepository.FindAsync(itemCreatingDto.CategoryId);

                if (category == null)
                {
                    return BadRequest($"Category with ID {itemCreatingDto.CategoryId} not found.");
                }

                var item = _mapper.Map<Item>(itemCreatingDto);

                item.CategoryId = itemCreatingDto.CategoryId;
                item.Category = category;

                if (!TryValidateModel(item))
                {
                    return BadRequest(ModelState);
                }

                await _itemRepository.CreateAsync(item);

                var createdItemDto = _mapper.Map<ItemDTO>(item);

                return CreatedAtAction(nameof(GetItem), new { id = createdItemDto.Id }, createdItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDTO>> Update(ItemUpdatingDTO itemUpdatingDto, int id)
        {
            try
            {
                if (! await _itemRepository.ExistsAsync(id))
                {
                    return NotFound();
                }

                var category = await _categoryRepository.FindAsync(itemUpdatingDto.CategoryId);

                if (category == null)
                {
                    return BadRequest(new { categoryId = new { message = $"Category with ID {itemUpdatingDto.CategoryId} not found."}
                    });
                }

                var item = _mapper.Map<Item>(itemUpdatingDto);

                item.CategoryId = itemUpdatingDto.CategoryId;
                item.Category = category;

                if (!TryValidateModel(item))
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(itemUpdatingDto, item);
                await _itemRepository.SaveChangesAsync();

                var createdItemDto = _mapper.Map<ItemDTO>(item);

                return CreatedAtAction(nameof(GetItem), new { id = createdItemDto.Id }, createdItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (!await _itemRepository.ExistsAsync(id))
                {
                    return NotFound();
                }

                await _itemRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
