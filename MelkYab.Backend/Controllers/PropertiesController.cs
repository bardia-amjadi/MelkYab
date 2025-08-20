using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/properties")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropertiesController(AppDbContext context)
        {
            _context = context;
        }

        private string ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString() ?? "1";

        // GET: api/v1/properties
        [HttpGet]
        public async Task<ActionResult> GetProperties()
        {
            var properties = await _context.Properties
                .Include(p => p.CreatedByUser)
                .ToListAsync();

            var result = properties.Select(p => new
            {
                property = p,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetProperty), new { version = ApiVersion, id = p.Id }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(UpdateProperty), new { version = ApiVersion, id = p.Id }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(DeleteProperty), new { version = ApiVersion, id = p.Id }), method = "DELETE" }
                }
            });

            return Ok(new
            {
                count = properties.Count,
                items = result,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetProperties), new { version = ApiVersion }), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(CreateProperty), new { version = ApiVersion }), method = "POST" }
                }
            });
        }

        // GET: api/v1/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProperty(string id)
        {
            var property = await _context.Properties
                .Include(p => p.CreatedByUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return NotFound();

            return Ok(new
            {
                property,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetProperty), new { version = ApiVersion, id = property.Id }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(UpdateProperty), new { version = ApiVersion, id = property.Id }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(DeleteProperty), new { version = ApiVersion, id = property.Id }), method = "DELETE" },
                    new { rel = "list", href = Url.Action(nameof(GetProperties), new { version = ApiVersion }), method = "GET" }
                }
            });
        }

        // POST: api/v1/properties
        [HttpPost]
        public async Task<ActionResult> CreateProperty([FromBody] Property property)
        {
            property.Id = Guid.NewGuid().ToString();
            property.CreatedAt = DateTime.UtcNow;

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProperty), new { version = ApiVersion, id = property.Id }, new
            {
                property,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetProperty), new { version = ApiVersion, id = property.Id }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(UpdateProperty), new { version = ApiVersion, id = property.Id }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(DeleteProperty), new { version = ApiVersion, id = property.Id }), method = "DELETE" },
                    new { rel = "list", href = Url.Action(nameof(GetProperties), new { version = ApiVersion }), method = "GET" }
                }
            });
        }

        // PUT: api/v1/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(string id, [FromBody] Property property)
        {
            if (id != property.Id)
                return BadRequest(new { message = "ID in URL does not match ID in body." });

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

        // DELETE: api/v1/properties/{id}
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
