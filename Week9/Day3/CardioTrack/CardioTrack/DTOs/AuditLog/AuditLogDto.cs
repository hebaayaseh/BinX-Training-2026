using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.AuditLog
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; } 
        public string Action { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int PerformedByUserId { get; set; }
        public DateTime Time { get; set; }
    }
}
