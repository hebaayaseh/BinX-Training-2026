using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class EmailVerificationCode
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(50)]
        public string Purpose { get; set; } = string.Empty;
        public string? PendingValue { get; set; }

        // Navigation Proparities 
        public User? User { get; set; }
    }
}
