    using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using MelkYab.Backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesRepository propertyRepository;
        public PropertiesController(IPropertiesRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        // GET : api/properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            var properties = await propertyRepository.GetAllPropertyAsync(); 
            return Ok(properties);
        }

        // GET : api/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(string id)
        {
            var property = await propertyRepository.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound();
            return Ok(property);
        }

        // POST : api/properties
        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty(Property property)
        {
            await propertyRepository.CreatePropertyAsync(property);

            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }

        // PUT : api/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(string id, Property property)
        {
            if (id != property.Id)
                return BadRequest();

            var existing = await propertyRepository.UpdatePropertyAsync(id, property);
            if (existing == null)
                return NotFound();

            return NoContent();
        }

        // DELETE : api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(string id)
        {
            var property = await propertyRepository.DeletePropertyByIdAsync(id);
            if (property == null)
                return NotFound();

            return NoContent();
        }
    }
}
