using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IGetPatients
    {
        Task<PaginatedPatientsResponseDto> GetPatientsAsync(int userId, GetPatientsQueryRequestDto query);
        Task<PatientsDto> GetPatientAsync(int userId, GetPatientRequestDto request);
    }
}
