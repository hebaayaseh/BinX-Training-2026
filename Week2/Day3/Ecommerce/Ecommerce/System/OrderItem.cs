using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.System
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int orderId { get; set; }
        public int quantity { get; set; }

        // Navigation Probarity :
        public Order order { get; set; }

    }
}
