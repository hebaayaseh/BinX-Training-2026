using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IManageMedication
    {
        Task<ManageMedicationResponseDto> AddMedicationAsync(int userId , ManageMedicationRequestDto request);
    }
}
