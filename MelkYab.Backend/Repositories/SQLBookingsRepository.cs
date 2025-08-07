using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Repositories
{
    public class SQLBookingsRepository : IBookingsRepository
    {
        private readonly AppDbContext dbContext;
        public SQLBookingsRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            booking.Id = Guid.NewGuid().ToString();
            booking.CreatedAt = DateTime.UtcNow;
            await dbContext.Bookings.AddAsync(booking);
            await dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> DeleteBookingByIdAsync(string id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking is null)
            {
                return null;
            }
            dbContext.Bookings.Remove(booking);
            await dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<List<Booking>> GetAllBookingAsync()
        {
            return await dbContext.Bookings.Include(x=>x.User).ToListAsync();  
        }

        public async Task<Booking?> GetBookingByIdAsync(string id)
        {
            var booking = await dbContext.Bookings.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            return booking;
        }

        public async Task<Booking?> UpdateBookingAsync(string id, Booking booking)
        {
            var existingBooking = await dbContext.Bookings.FindAsync(id);
            if (existingBooking is null)
            {
                return null;
            }
            existingBooking.CheckInDate = booking.CheckInDate;
            existingBooking.CheckOutDate = booking.CheckOutDate;
            existingBooking.Guests = booking.Guests;
            existingBooking.TotalPrice = booking.TotalPrice;
            existingBooking.Status = booking.Status;
            existingBooking.UserId = booking.UserId;
            await dbContext.SaveChangesAsync();
            return booking;
        }
    }
}