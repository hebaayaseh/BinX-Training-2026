using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabRequest
{
    public class UpdateLabRequestStatusRequestDto
    {
        public int LabRequestId { get; set; }
        public LabRequestStatus NewStatus { get; set; }
    }
}
