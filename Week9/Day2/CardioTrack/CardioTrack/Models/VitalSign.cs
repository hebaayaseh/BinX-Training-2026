using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class VitalSign
    {
        [Key]
        public int Id { get; set; }
        public DateTime RecordedAt { get; set; }
        public double HeartRate { get; set; }
        public double BloodPressureSystolic { get; set; }
        public double BloodPressureDiastolic { get; set; }
        public decimal Temperature { get; set; }
        public double OxygenSaturation { get; set; }
        public int RecordedByUserId { get; set; }
        public int PatientId { get; set; }
        // Navigation property
        public Patient? Patient { get; set; }
        public User? RecordedByUser { get; set; }
        public ICollection<VitalSignAlert>? VitalSignAlerts { get; set; } = new List<VitalSignAlert>();
    }
}
