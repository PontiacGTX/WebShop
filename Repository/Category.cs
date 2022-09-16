using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebShop.Repository
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
       
    }
}