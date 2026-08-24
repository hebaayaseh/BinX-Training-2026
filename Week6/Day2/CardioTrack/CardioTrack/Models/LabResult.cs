using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class LabResult
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int TechnicianId { get; set; }

        public string? ResultFileUrl { get; set; }
        public int? LabRequestId { get; set; }
        public string? AiSummary { get; set; }
        public LabStatus Status { get; set; } = LabStatus.Pending;
        public int? PaymentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public User Technician { get; set; } = null!;
        public LabRequest? LabRequest { get; set; }
    }
}
