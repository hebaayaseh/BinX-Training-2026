using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabResult
{
    public class LabResultResponseDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public int? LabRequestId { get; set; }
        public string? TestName { get; set; }
        public string? ResultFileUrl { get; set; }
        public LabStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
