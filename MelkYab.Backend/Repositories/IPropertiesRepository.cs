using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelkYab.Backend.Data.Tables;

namespace MelkYab.Backend.Repositories
{
    public interface IPropertiesRepository
    {
        Task<List<Property>> GetAllPropertyAsync();
        Task<Property?> GetPropertyByIdAsync(string id);
        Task<Property> CreatePropertyAsync(Property property);
        Task<Property?> UpdatePropertyAsync(string id, Property property);
        Task<Property?> DeletePropertyByIdAsync(string id);
    }
}