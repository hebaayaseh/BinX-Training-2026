using CardioTrack.DTOs.LabResult;

namespace CardioTrack.Interfaces.ILabResult
{
    public interface IManageLabResult
    {
        Task<LabResultResponseDto> CreateLabResultAsync(int technicianUserId, CreateLabResultDto request);
        Task<List<LabResultResponseDto>> GetLabResultsAsync(int userId, GetLabResultDto request);
        Task<string> UpdateLabResultStatusAsync(int userId, UpdateLabResultStatusRequestDto request);
    }
}
