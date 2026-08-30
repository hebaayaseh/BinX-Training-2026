using Microsoft.AspNetCore.Identity;

namespace BookNest.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;

        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}