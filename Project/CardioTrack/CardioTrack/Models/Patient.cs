using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; } 
        [MaxLength(100)]
        public string Address { get; set; }
        public BloodType BloodType { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int DoctorId { get; set; }
        public int? LinkedUserId { get; set; }

        // Navigation Properties
        public User? Doctor { get; set; }
        public User? LinkedUser { get; set; }
        public ICollection<MedicalHistory>? MedicalHistories { get; set; } = new List<MedicalHistory>();
        public ICollection<VitalSign>? VitalSigns { get; set; } = new List<VitalSign>();
        public ICollection<VitalSignAlert>? VitalSignAlerts { get; set; } = new List<VitalSignAlert>();
        public ICollection<Medication>? Medications { get; set; } = new List<Medication>();
        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();

    }
}
