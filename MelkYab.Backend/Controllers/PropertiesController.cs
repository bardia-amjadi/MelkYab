using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropertiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET : api/properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            return await _context.Properties
                .Include(p => p.CreatedByUser)
                .ToListAsync();
        }

        // GET : api/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(string id)
        {
            var property = await _context.Properties
                .Include(p => p.CreatedByUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return NotFound();

            return property;
        }

        // POST : api/properties
        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty(Property property)
        {
            property.Id = Guid.NewGuid().ToString();
            property.CreatedAt = DateTime.UtcNow;
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }

        // PUT : api/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(string id, Property property)
        {
            if (id != property.Id)
                return BadRequest();

            var existing = await _context.Properties.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Update fields
            existing.Title = property.Title;
            existing.Description = property.Description;
            existing.Type = property.Type;
            existing.MaxGuests = property.MaxGuests;
            existing.Bedrooms = property.Bedrooms;
            existing.Beds = property.Beds;
            existing.Bathrooms = property.Bathrooms;
            existing.PricePerNight = property.PricePerNight;
            existing.Address = property.Address;
            existing.IsActive = property.IsActive;
            existing.CreatedByUserId = property.CreatedByUserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE : api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(string id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return NotFound();

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
