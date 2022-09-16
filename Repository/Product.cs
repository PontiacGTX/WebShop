using System;
using System.Collections.Generic;
namespace WebShop.Repository
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; } 
        public double Price { get; set; }  
        public ICollection<OrderProducts> OrdenesProductos { get; set; }
        public bool Enabled { get; set; }
        public bool HasCategory { get; set; }
    }
}
