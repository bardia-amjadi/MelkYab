using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET : api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .ToListAsync();
        }

        // GET : api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return booking;
        }

        // POST : api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            booking.Id = Guid.NewGuid().ToString();
            booking.CreatedAt = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // PUT : api/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, Booking booking)
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

        // DELETE : api/bookings/{id}
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
