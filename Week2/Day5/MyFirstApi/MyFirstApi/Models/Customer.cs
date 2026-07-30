using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string fullName { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [ForeignKey("order")]

        public int orderId { get; set; }

        // Navication Proparity : 
        public List<Order> orders { get; set; } = new List<Order>();
    }
}
