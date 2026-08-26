using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class MedicationOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int MedicationOrderId { get; set; }
        public int PharmacyStockId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        // Navigation Properties
        public MedicationOrder? MedicationOrder { get; set; }
        public PharmacyStock? PharmacyStock { get; set; }
    }
}