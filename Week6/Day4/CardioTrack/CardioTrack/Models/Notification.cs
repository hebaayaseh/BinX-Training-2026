using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public NotificationType NotificationType {  get; set; }
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //  Navigation Property :
        public User? User { get; set; } 

    }
}
