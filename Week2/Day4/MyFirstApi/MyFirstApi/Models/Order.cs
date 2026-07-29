using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public int customerId { get; set; }
        public int itemId { get; set; }
        public double TotalAmount { get; set; }

        // Navigation Proparity : 
        public Customer customer { get; set; }
        public List<OrderItem> items { get; set; } = new List<OrderItem>();
    }
}
