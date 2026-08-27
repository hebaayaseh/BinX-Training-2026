namespace CardioTrack.DTOs.LabRequest
{
    public class CreateLabRequestDto
    {
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public List<string> TestNames { get; set; } = new List<string>();
    }
}
