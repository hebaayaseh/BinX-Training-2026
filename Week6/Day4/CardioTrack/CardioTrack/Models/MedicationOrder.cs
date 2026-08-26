using CardioTrack.Enums;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Models
{
    public class MedicationOrder
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int OrderedByUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        // Navigation Properties
        public Patient? Patient { get; set; }
        public User? OrderedByUser { get; set; }
        public ICollection<MedicationOrderItem>? OrderItems { get; set; } = new List<MedicationOrderItem>();
    }
}