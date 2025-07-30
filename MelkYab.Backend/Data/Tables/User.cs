using Microsoft.AspNetCore.Identity;

namespace MelkYab.Backend.Data.Tables
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
