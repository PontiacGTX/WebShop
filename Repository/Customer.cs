using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebShop.Repository
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}