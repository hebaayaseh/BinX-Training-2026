using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class MedicalHistory
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Condition { get; set; }
        [MaxLength(200)]
        public string Note { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public int PatientId { get; set; }
        public int RecordedByDoctorId { get; set; }
        // Navigation Properties
        public Patient? Patient { get; set; }
        public User? RecordedByDoctor { get; set; }
    }
}
