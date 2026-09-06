using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabResult
{
    public class UpdateLabResultStatusRequestDto
    {
        public int LabResultId { get; set; }
        public LabStatus NewStatus { get; set; }
    }
}
