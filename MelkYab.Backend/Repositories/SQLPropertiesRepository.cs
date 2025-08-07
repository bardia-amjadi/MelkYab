using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Repositories
{
    public class SQLPropertiesRepository : IPropertiesRepository
    {
        private readonly AppDbContext dbContext;
        public SQLPropertiesRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Property> CreatePropertyAsync(Property property)
        {
            property.Id = Guid.NewGuid().ToString();
            property.CreatedAt = DateTime.UtcNow;
            await dbContext.Properties.AddAsync(property);
            await dbContext.SaveChangesAsync();
            return property;
        }

        public async Task<Property?> DeletePropertyByIdAsync(string id)
        {
            var property = await dbContext.Properties.FindAsync(id);
            if (property == null)
                return null;
            dbContext.Remove(property);
            await dbContext.SaveChangesAsync();
            return property;
        }

        public async Task<List<Property>> GetAllPropertyAsync()
        {
            return await dbContext.Properties.ToListAsync();
        }

        public async Task<Property?> GetPropertyByIdAsync(string id)
        {
            return await dbContext.Properties.Include(p => p.CreatedByUser).FirstOrDefaultAsync(property=>property.Id == id);
        }

        public async Task<Property?> UpdatePropertyAsync(string id, Property property)
        {
            var existingProperty = await dbContext.Properties.FindAsync(id);
            if (existingProperty is null)
            {
                return null;
            }
            existingProperty.Title = property.Title;
            existingProperty.Description = property.Description;
            existingProperty.Type = property.Type;
            existingProperty.MaxGuests = property.MaxGuests;
            existingProperty.Bedrooms = property.Bedrooms;
            existingProperty.Beds = property.Beds;
            existingProperty.Bathrooms = property.Bathrooms;
            existingProperty.PricePerNight = property.PricePerNight;
            existingProperty.Address = property.Address;
            existingProperty.IsActive = property.IsActive;
            existingProperty.CreatedByUserId = property.CreatedByUserId;
            await dbContext.SaveChangesAsync();
            return existingProperty;
        }
    }
}