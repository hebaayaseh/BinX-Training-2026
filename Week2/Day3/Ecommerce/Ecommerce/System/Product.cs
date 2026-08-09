using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.System
{
    public class Product
    {
        public int id { private get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
    }
}
