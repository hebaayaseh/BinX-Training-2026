using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }
        public AppointmentStatus Status { get; set; }
        public int CreatedByUserId { get; set; }
        // Navigation properties
        public Patient? Patient { get; set; }
        public User? Doctor { get; set; }
        public User? CreatedByUser { get; set; }
    }

    
}
