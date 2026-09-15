namespace CardioTrack.DTOs.Patient
{
    public class AppointmentsDto
    {
        public int DoctorId { get; set; }
        public string DoctorFullName { get; set; }
        public DateTime AppointmantDate { get; set; }
    }
}
