using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IMedicalHistory
    {
        Task<MedicalHistoryResponseDto> AddMedicalHistoryAsync(int userId, MedicalHistoryRequestDto request);
    }
}
