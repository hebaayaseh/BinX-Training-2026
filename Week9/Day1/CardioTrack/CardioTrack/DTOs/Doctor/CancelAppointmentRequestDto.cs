namespace CardioTrack.DTOs.Doctor
{
    public class CancelAppointmentRequestDto
    {
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
    }
}
