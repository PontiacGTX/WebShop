using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Repository;
using WebShop.Validation;

namespace WebShop.Models
{
    public class AddProductViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public bool Enabled { get; set; }
        [MinCollectionElements(ErrorMessage ="Must contain at least one element")]
        public List<CategorySelectModel> Categories { get; set; } = new List<CategorySelectModel>(); 
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    }
}
