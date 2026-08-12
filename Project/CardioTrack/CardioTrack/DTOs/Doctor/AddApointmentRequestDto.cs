using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.DTOs.Doctor
{
    public class AddApointmentRequestDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
    }
}
