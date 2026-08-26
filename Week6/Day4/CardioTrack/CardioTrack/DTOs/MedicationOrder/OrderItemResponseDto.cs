namespace CardioTrack.DTOs.MedicationOrder
{
    public class OrderItemResponseDto
    {
        public int PharmacyStockId { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
