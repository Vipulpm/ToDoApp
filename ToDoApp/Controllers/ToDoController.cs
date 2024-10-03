using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application;
using ToDoApp.Domain;
using ToDoApp.DTOs;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDBContext _dbContext;
        private readonly IMapper _mapper;
        public ToDoController(ToDoDBContext dBContext, IMapper mapper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemDto>>> GetToDoItems()
        {
            var items = await _dbContext.ToDoItems.ToListAsync();
            var itemDtos = _mapper.Map<IEnumerable<ToDoItemDto>>(items);  // Map to DTOs
            return Ok(itemDtos);
        }

        // GET: api/ToDoItems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDto>> GetToDoItem(int id)
        {
            var item = await _dbContext.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var itemDto = _mapper.Map<ToDoItemDto>(item);  // Map to DTO
            return Ok(itemDto);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItemDto>> PostToDoItem(ToDoItemDto itemDto)
        {
            var item = _mapper.Map<ToDoItem>(itemDto);  // Map from DTO to entity
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();

            var createdItemDto = _mapper.Map<ToDoItemDto>(item);
            return CreatedAtAction(nameof(GetToDoItem), new { id = item.Id }, createdItemDto);
        }

        // PUT: api/ToDoItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDoItemDto itemDto)
        {
            var existingItem = await _dbContext.ToDoItems.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound();  // Return 404 if item does not exist
            }

            // Update the properties of the existing entity using the DTO
            _mapper.Map(itemDto, existingItem);

            // Mark the entity as modified
            _dbContext.Entry(existingItem).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ToDoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var item = await _dbContext.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _dbContext.ToDoItems.Remove(item);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(int id)
        {
            return _dbContext.ToDoItems.Any(e => e.Id == id);
        }
    }
}
