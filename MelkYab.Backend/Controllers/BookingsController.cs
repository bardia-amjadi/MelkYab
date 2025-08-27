using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LinkGenerator _linkGenerator;
        private UnitOfWork UnitOfWork;

        public BookingsController(AppDbContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
            UnitOfWork = new(context);

        }

        // GET: api/v1/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookings()
        {
            var bookings = await _context.Bookings.Include(b => b.User).ToListAsync();

            var result = bookings.Select(b => new
            {
                booking = b,
                links = new[]
                {
                    new {
                        rel = "self",
                        href = Url.Action(nameof(GetBooking), new { version= 1, id = b.Id }),
                        method = "GET"
                    },
                    new {
                        rel = "update",
                        href = Url.Action(nameof(UpdateBooking), new { version= 1, id = b.Id }),
                        method = "PUT"
                    },
                    new {
                        rel = "delete",
                        href = Url.Action(nameof(DeleteBooking), new {  version= 1,id = b.Id }),
                        method = "DELETE"
                    }
                }
            });

            return Ok(result);
        }

        // GET: api/v1/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBooking(string id)
        {
            var booking = await _context.Bookings.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            var links = new[]
            {
                new {
                    rel = "self",
                    href = Url.Action(nameof(GetBooking), new {  version= 1, id }),
                    method = "GET"
                },
                new {
                    rel = "update",
                    href = Url.Action(nameof(UpdateBooking), new { version= 1, id }),
                    method = "PUT"
                },
                new {
                    rel = "delete",
                    href = Url.Action(nameof(DeleteBooking), new {  version= 1, id }),
                    method = "DELETE"
                }
            };

            return Ok(new { booking, links });
        }

        // POST: api/v1/bookings
        [HttpPost]
        public async Task<ActionResult<object>> CreateBooking([FromBody] Booking booking)
        {
            booking.Id = Guid.NewGuid().ToString();
            booking.CreatedAt = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var links = new[]
            {
                new {
                    rel = "self",
                    href = Url.Action(nameof(GetBooking), new {  version= 1, id = booking.Id }),
                    method = "GET"
                },
                new {
                    rel = "all-bookings",
                    href = Url.Action(nameof(GetBookings)),
                    method = "GET"
                }
            };

            return CreatedAtAction(nameof(GetBooking), new {  version= 1, id = booking.Id }, new { booking, links });
        }

        // PUT: api/v1/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] Booking booking)
        {
            if (id != booking.Id)
                return BadRequest();

            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.CheckInDate = booking.CheckInDate;
            existing.CheckOutDate = booking.CheckOutDate;
            existing.Guests = booking.Guests;
            existing.TotalPrice = booking.TotalPrice;
            existing.Status = booking.Status;
            existing.UserId = booking.UserId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
