using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        // This will NOT go into DB (used only for API binding)
        public string? Password { get; set; }
    }
}