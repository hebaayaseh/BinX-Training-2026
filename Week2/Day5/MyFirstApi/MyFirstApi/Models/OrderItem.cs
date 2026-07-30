using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int? orderId { get; set; }
        public int quantity { get; set; }

        // Navigation Probarity :
        public Order order { get; set; }
    }
}
