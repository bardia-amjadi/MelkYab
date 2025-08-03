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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsRepository bookingsRepository;

        public BookingsController(IBookingsRepository bookingsRepository)
        {
            this.bookingsRepository = bookingsRepository;
        }

        // GET : api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var bookings=await bookingsRepository.GetAllBookingAsync();
            return Ok(bookings);
        }

        // GET : api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            var booking = await bookingsRepository.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        // POST : api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            await bookingsRepository.CreateBookingAsync(booking);

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // PUT : api/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, Booking booking)
        {
            if (id != booking.Id)
                return BadRequest();

            var existing = await bookingsRepository.UpdateBookingAsync(id, booking);
            if (existing == null)
                return NotFound();

            return NoContent();
        }

        // DELETE : api/bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var booking = await bookingsRepository.DeleteBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return NoContent();
        }
    }
}
