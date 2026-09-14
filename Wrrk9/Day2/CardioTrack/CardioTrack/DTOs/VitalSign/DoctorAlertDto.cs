using CardioTrack.Enums;

namespace CardioTrack.DTOs.VitalSign
{
    public class DoctorAlertDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public Severity Severity { get; set; }
        public AlterType AlterType { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
