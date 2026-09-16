using CardioTrack.Enums;

namespace CardioTrack.DTOs.LabRequest
{
    public class GetLabRequestsRequestDto
    {
        public int? PatientId { get; set; }
        public LabRequestStatus? Status { get; set; }
    }
}
