namespace CardioTrack.DTOs.Admin
{
    public class DoctorPatientDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<PatientDto> Patients { get; set; }
    }
}
