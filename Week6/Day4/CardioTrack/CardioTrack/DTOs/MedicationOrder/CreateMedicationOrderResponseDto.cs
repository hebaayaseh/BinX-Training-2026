using CardioTrack.Enums;

namespace CardioTrack.DTOs.MedicationOrder
{
    public class CreateMedicationOrderResponseDto
    {
        public int OrderId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>();
    }
}
