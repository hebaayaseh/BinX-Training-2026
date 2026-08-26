using CardioTrack.DTOs.MedicationOrder;

namespace CardioTrack.Interfaces.IMedicationOrder
{
    public interface IMedicationOrder
    {
        Task<CreateMedicationOrderResponseDto> CreateOrderAsync(int userId, CreateMedicationOrderRequestDto request);
    }
}