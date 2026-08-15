using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IManageMedication
    {
        Task<AddMedicationResponseDto> AddMedicationAsync(int userId , AddMedicationRequestDto request);
        Task<string> UpdateMedicationAsync(int userId,UpdateMedicationRequestDto request);
        Task<GetPatientMedicationResponseDto> GetPatientMedication(int userId,GetPatientRequestDto request);
        Task<string> DeactiveMedicationAsync(int userId, DeactiveMedicationRequestDto request);
        
        
    }
}
