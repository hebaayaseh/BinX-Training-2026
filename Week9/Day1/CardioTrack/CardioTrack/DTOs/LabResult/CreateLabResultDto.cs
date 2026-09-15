namespace CardioTrack.DTOs.LabResult
{
    public class CreateLabResultDto
    {
        public int PatientId { get; set; }
        public int? LabRequestId { get; set; }
        public IFormFile ResultFile { get; set; }
    }
}
