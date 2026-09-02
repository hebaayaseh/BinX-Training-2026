namespace BookNest.Models
{
    public class MemberProfile
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public int MaxBooksAllowed { get; set; } = 3;   

        public int ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}