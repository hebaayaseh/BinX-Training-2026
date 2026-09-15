using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IQuery
{
    public interface IQuery
    {
        Task<PaginatedPatientsResponseDto> GetPatientsAsync(int userId, GetPatientsQueryRequestDto query);
    }
}
