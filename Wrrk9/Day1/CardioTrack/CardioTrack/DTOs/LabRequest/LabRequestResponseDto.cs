using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabRequest
{
    public class LabRequestResponseDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int RequestedByDoctorId { get; set; }
        public string DoctorName { get; set; }
        public int? AppointmentId { get; set; }
        public string TestName { get; set; }
        public LabRequestStatus Status { get; set; }
        public DateTime RequstedAt { get; set; }
    }
}
