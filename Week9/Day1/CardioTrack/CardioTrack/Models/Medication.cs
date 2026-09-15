using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        [MaxLength(100)]
        public string DrugName { get; set; }
        [MaxLength(50)]
        public string Dosage { get; set; }
        [MaxLength(50)]
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PrescribedByDoctorId { get; set; }
        public bool IsActive { get; set; }
        // Navigation properties
        public Patient? Patient { get; set; }
        public User? PrescribedByDoctor { get; set; }
    }
}
