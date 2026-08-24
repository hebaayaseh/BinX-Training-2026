using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        [MaxLength(50)]
        public string Action {  get; set; }
        public int PerformedByUserId {  get; set; }
        public DateTime Time { get; set; }
        // Navication Proparty :
        public User? PreformedByUser { get; set; } 
    }
}
