using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class PharmacyStock
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string DrugName { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<MedicationOrderItem>? MedicationOrderItems { get; set; } = new List<MedicationOrderItem>();
    }
}