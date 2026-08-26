namespace CardioTrack.DTOs.MedicationOrder
{
    public class CreateMedicationOrderRequestDto
    {
        public int PatientId { get; set; }
        public List<OrderItemRequestDto> Items { get; set; } = new List<OrderItemRequestDto>();
    }

    
}