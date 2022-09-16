using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebShop.Repository
{
    public class Order
    {
        public Guid OrderId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int PaymentType { get; set; }
        public int ItemCount { get; set; }
        public ICollection<OrderProducts> OrdenesProductos { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int  PaymentId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}