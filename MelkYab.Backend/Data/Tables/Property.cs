using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MelkYab.Backend.Data.Tables
{
    public class Property : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int MaxGuests { get; set; }
        public int Bedrooms { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public int PricePerNight { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public User? CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
