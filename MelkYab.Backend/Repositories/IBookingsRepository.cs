using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelkYab.Backend.Data.Tables;

namespace MelkYab.Backend.Repositories
{
    public interface IBookingsRepository
    {
        Task<List<Booking>> GetAllBookingAsync();
        Task<Booking?> GetBookingByIdAsync(string id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking?> UpdateBookingAsync(string id, Booking booking);
        Task<Booking?> DeleteBookingByIdAsync(string id);
    }
}