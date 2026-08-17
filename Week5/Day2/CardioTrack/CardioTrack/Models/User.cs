using CardioTrack.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Navigation Properties
        public ICollection<Patient>? PatientsAsDoctor { get; set; } = new List<Patient>();
        public Patient? LinkedPatient { get; set; }
        public ICollection<MedicalHistory>? MedicalHistories { get; set; } = new List<MedicalHistory>();
        public ICollection<VitalSign>? VitalSigns { get; set; } = new List<VitalSign>();
        public ICollection<Medication>? Medications { get; set; } = new List<Medication>();
        public ICollection<Appointment>? AppointmentsAsDoctor { get; set; } = new List<Appointment>();
        public ICollection<Appointment>? AppointmentsCreated { get; set; } = new List<Appointment>();
        public ICollection<EmailVerificationCode>? EmailVerificationCodes { get; set; } = new List<EmailVerificationCode>();
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
