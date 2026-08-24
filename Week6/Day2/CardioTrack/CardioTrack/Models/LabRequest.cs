using Microsoft.AspNetCore.SignalR.Protocol;
using System.Numerics;

namespace CardioTrack.Models
{
    public class LabRequest
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public string? Notes { get; set; }
        public LabRequestStatus Status { get; set; } = LabRequestStatus.Pending;
        public DateTime RequstedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties :
        public User? Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Appointment? Appointment { get; set; }
        public LabResult? LabResult { get; set; }
    }
}
