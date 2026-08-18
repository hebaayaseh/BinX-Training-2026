namespace CardioTrack.DTOs.Doctor
{
    public class AddAppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId {  get; set; }
    }
}
