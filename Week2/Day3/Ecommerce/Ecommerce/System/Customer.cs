using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.System
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string fullName { get; set; }
        [EmailAddress]
        [Required]
        public string email {  get; set; }
        [Required]
        public string password {  get; set; }
        [ForeignKey("order")]

        public int orderId { get; set; }
        
        // Navication Proparity : 
        public List<Order> orders { get; set; } = new List<Order>();
        // Constrocter :
        public Customer(int Id ,string fullName , string email , string password)
        {
            this.Id = Id;
            this.fullName = fullName;
            this.email = email;
            this.password = password;

        }
    }
}
