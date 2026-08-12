using CardioTrack.DTOs.Doctor;

namespace CardioTrack.Interfaces.IDoctor
{
    public interface IManageMedication
    {
        Task<ManageMedicationResponseDto> ManageMedicationAsync(int userId , ManageMedicationRequestDto request);
    }
}
