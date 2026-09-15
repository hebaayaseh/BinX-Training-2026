using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.Patient;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IMedicalHistory
    {
        Task<AddMedicalHistoryResponseDto> AddMedicalHistoryAsync(int userId, AddHistoryRequestDto request);
        Task<ViewMedicalHistoryResponseDto> ViewPatientMedicalHistoryAsync(int userId, GetPatientRequestDto request);
        Task<string> UpdateMedicalHistoryAsync(int userId, UpdateMedicalHistoryRequestDto request);
    }
}
