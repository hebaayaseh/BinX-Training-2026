namespace BookNest.Models
{
    public enum ReservationStatus
    {
        Active,
        Returned
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        public Book? Book { get; set; }
        public ApplicationUser? Member { get; set; }
    }
}