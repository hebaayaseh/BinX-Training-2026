using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabResult
{
    public class GetLabResultDto
    {
        public int? PatientId { get; set; }
        public LabStatus? Status { get; set; }
    }
}
