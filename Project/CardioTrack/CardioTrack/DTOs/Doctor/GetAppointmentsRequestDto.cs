using CardioTrack.Enums;

namespace CardioTrack.DTOs.Doctor
{
    public class GetAppointmentsRequestDto
    {
        public AppointmentStatus AppointmentStatus { get; set; }
        public int DoctorId {  get; set; }
    }
}
