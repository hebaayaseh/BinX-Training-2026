using CardioTrack.DTOs.LabRequest;

namespace CardioTrack.Interfaces.ILabRequest
{
    public interface IManageLabRequest
    {
        Task<List<LabRequestResponseDto>> CreateLabRequestAsync(int doctorUserId, CreateLabRequestDto request);
        Task<List<LabRequestResponseDto>> GetLabRequestsAsync(int userId, GetLabRequestsRequestDto request);
        Task<string> UpdateLabRequestStatusAsync(int userId, UpdateLabRequestStatusRequestDto request);
    }
}
